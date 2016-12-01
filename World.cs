using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Tiles;
using Limestone.Entities;
using Limestone.Entities.Enemies;
using Limestone.Items;
using Limestone.Generation;
using Limestone.Guis;
using Limestone.Serialization;
using Limestone.Entities.Collectables;

namespace Limestone
{
    [JsonObject(MemberSerialization.OptIn)]
    public class World
    {
        public static int worldTimer;
        public Player2 player;

        Effect testEffect;
        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Serialize)]
        public Tile[,] tiles;
        public List<TileWall> wallTiles = new List<TileWall>();
        private List<Tile> drawTiles = new List<Tile>();
        private List<Tile> spawnCheckTiles = new List<Tile>();

        public List<Projectile> projectiles = new List<Projectile>();
        public List<Enemy> enemies = new List<Enemy>();
        public List<EntitySpawner> spawners = new List<EntitySpawner>();
        public List<Particle> particles = new List<Particle>();
        public List<Entity> entities = new List<Entity>();
        public List<Collectable> collectables = new List<Collectable>();
        public List<Bomb> bombs = new List<Bomb>();
        public List<NPC> npcs = new List<NPC>();

        public List<Rectangle> islandRects = new List<Rectangle>();

        private DungeonGenerator gd;
        public Coordinate spawnLocation = Coordinate.Zero;

        private int respawnTimer = -20;

        private bool sorted = false;

        private bool takeshot = false, takeshot1 = false;

        private Texture2D minimap;

        public Thread mapLoadThread;
        private bool doneGen = false;

        public World()
        {
            worldTimer = 0;
        }

        public void Update()
        {
            worldTimer++;

            if (!mapLoadThread.IsAlive)
            {
                if (!Main.camera.activeGui.stopsWorldInput)
                {
                    entities = entities.OrderBy(x =>
                    {
                        if (x.tType != EntityType.Spawner)
                        {
                            Vector2 yRot = Vector2.Transform(x.hitbox.center, Matrix.CreateRotationZ(Main.camera.Rotation));
                            return yRot.Y;
                        }
                        return 0;
                    }).ToList();

                    if (player.moving)
                    {
                        List<Tile> nearTiles = nearTiles = GetNearTiles(player, 16);

                        foreach (Tile t in nearTiles)
                        {
                            if (!t.revealed)
                            {
                                t.revealed = true;
                                t.OnReveal(this);
                            }
                        }
                    }
                    if (player != null)
                    {
                        Main.camera.clamp = new Rectangle(0, 0, tiles.GetLength(0) * Coordinate.coordSize, tiles.GetLength(1) * Coordinate.coordSize); //new Rectangle(0, 0, 3200, 600);
                        doneGen = true;
                    }
                    if (doneGen)
                    {
                        if (Main.keyboard.KeyPressed(Keys.F))
                        {
                            //CreateEnemy(new EnemyTest(player.center));
                            //CreateBomb(new Bomb(null, Color.White, 4, player.center, Vector2.Zero, 128, 1, 0, 0, 0, 4, 128, 2));
                            //player.GiveXp(90000);
                        }
                        if (Main.keyboard.KeyPressed(Keys.G))
                            takeshot = true;

                        foreach (Entity entity in entities.ToList())
                        {
                            if (entity.tType == EntityType.Player)
                                entity.Update(this);
                            else if (entity.tType == EntityType.Projectile)
                                entity.Update(this);
                            else if (entity.tType == EntityType.Bag)
                                entity.Update(this);
                            else if (entity.tType == EntityType.Spawner)
                                entity.Update(this);
                            else if (entity.tType == EntityType.Particle)
                                entity.Update(this);
                            else if (entity.tType == EntityType.Collectable)
                                entity.Update(this);
                            else if (entity.tType == EntityType.Bomb)
                                entity.Update(this);
                            else if (entity.tType == EntityType.NPC)
                                entity.Update(this);
                            else if (entity.tType == EntityType.Enemy)
                            {
                                Enemy e = (Enemy)entity;

                                float dist = Vector2.Distance(e.center, player.center);
                                if (dist < e.activeDistance)
                                    e.active = true;
                                else e.active = false;

                                if (e.active)
                                    e.Update(this);
                            }

                            #region If Projectile
                            if (entity.tType == EntityType.Projectile)
                            {
                                Projectile p = (Projectile)entity;
                                if (!p.friendly)
                                {
                                    if (!player.untargetable)
                                    {
                                        if (player.hitbox.Intersects(p.hitbox))
                                        {
                                            if (!p.hitEntities.Contains(p))
                                                player.TakeDamage((int)p.damage, p, this);

                                            if (!p.piercing)
                                                p.Die(this);
                                            else
                                                p.hitEntities.Add(p);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (Enemy e in enemies)
                                    {
                                        if (!e.untargetable)
                                        {
                                            if (e.hitbox.Intersects(p.hitbox))
                                            {
                                                if (!p.hitEntities.Contains(e))
                                                    e.TakeDamage((int)p.damage, p, this);

                                                if (!p.piercing)
                                                    entity.Die(this);
                                                else
                                                    p.hitEntities.Add(e);
                                            }
                                        }
                                    }
                                }
                            }
                            if (entity.tType == EntityType.Collectable)
                            {
                                Collectable collectable = (Collectable)entity;
                                if (player.hitbox.Intersects(entity.hitbox))
                                {
                                    if (collectable.collectable)
                                        collectable.OnPickup(this);
                                }
                            }
                            #endregion
                            if (entity.tileCollides)
                            {
                                List<Tile> nearTiles = GetNearTiles(entity);

                                foreach (Tile t in nearTiles)
                                {
                                    if (t.collidable)
                                    {
                                        if (entity.hitbox.Intersects(t.bounds))
                                        {
                                            TileCollidable tC = (TileCollidable)t;
                                            tC.OnEntityCollide(this, entity);
                                            entity.OnTileCollide(this, t);
                                        }
                                    }
                                }
                            }

                            #region deletion
                            if (entity.dead)
                                entities.Remove(entity);

                            if (entity.tType == EntityType.Projectile)
                            {
                                for (int i2 = projectiles.Count - 1; i2 >= 0; i2--)
                                {
                                    if (projectiles[i2].dead)
                                        projectiles.RemoveAt(i2--);
                                }
                            }
                            else if (entity.tType == EntityType.Enemy)
                            {
                                for (int i2 = enemies.Count - 1; i2 >= 0; i2--)
                                {
                                    if (enemies[i2].dead)
                                        enemies.RemoveAt(i2--);
                                }
                            }
                            else if (entity.tType == EntityType.Particle)
                            {
                                for (int i2 = particles.Count - 1; i2 >= 0; i2--)
                                {
                                    if (particles[i2].dead)
                                        particles.RemoveAt(i2--);
                                }
                            }
                            else if (entity.tType == EntityType.Collectable)
                            {
                                for (int i2 = collectables.Count - 1; i2 >= 0; i2--)
                                {
                                    if (collectables[i2].dead)
                                        collectables.RemoveAt(i2--);
                                }
                            }
                            else if (entity.tType == EntityType.Bomb)
                            {
                                for (int i2 = bombs.Count - 1; i2 >= 0; i2--)
                                {
                                    if (bombs[i2].dead)
                                        bombs.RemoveAt(i2--);
                                }
                            }
                            else if (entity.tType == EntityType.NPC)
                            {
                                for (int i2 = npcs.Count - 1; i2 >= 0; i2--)
                                {
                                    if (npcs[i2].dead)
                                        npcs.RemoveAt(i2--);
                                }
                            }
                        }
                        #endregion

                        Main.camera.target = player.center;

                        if (enemies.FindAll(x => x.active).Count < 16)
                        {
                            foreach (EntitySpawner spawner in spawners)
                            {
                                float distance = (player.position - spawner.position).Length();

                                if (distance > 1024 && distance <= 1280)
                                {
                                    if (Main.rand.Next(spawner.rate) == 0)
                                    {
                                        spawner.SpawnEnemies(this);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        List<Vector2> points = new List<Vector2>();
        public void Draw(GameCamera camera, SpriteBatch batch)
        {
            if (doneGen)
            {
                if (!Main.camera.activeGui.stopsWorldDraw)
                {
                    if (drawTiles.Count == 0)
                        GetTilesToDraw(true);
                    else GetTilesToDraw();
                    foreach (Tile t in drawTiles)
                    {
                        if (!t.setCardinals)
                            t.SetCardinalTiles(this);

                        t.Draw(batch);

                        if (t is TileRock)
                        {
                            TileRock tC = (TileRock)t;
                            tC.DrawBeneath(batch);
                        }
                    }

                    foreach (Entity e in entities)
                    {
                        e.DrawOutline(batch);

                        e.Draw(batch);
                    }

                    foreach (Tile t in drawTiles)
                    {
                        if (t.collidable)
                        {
                            TileCollidable tC = (TileCollidable)t;
                            tC.DrawOutline(batch);
                            tC.Draw(batch);
                        }
                    }

                    foreach (EntitySpawner spawner in spawners)
                        spawner.Draw(batch);

                    Vector2 prev = Vector2.Zero;
                    foreach (Vector2 p in points)
                    {
                        if (prev != Vector2.Zero)
                        {
                            DrawGeometry.DrawLine(batch, prev, p, Microsoft.Xna.Framework.Color.White);
                        }
                        prev = p;
                    }

                    if (takeshot)
                    {
                        if (minimap == null)
                            minimap = new Texture2D(batch.GraphicsDevice, tiles.GetLength(0), tiles.GetLength(1));
                        Color[] color2 = new Color[tiles.GetLength(0) * tiles.GetLength(1)];
                        for (int x = 0; x < tiles.GetLength(0); x++)
                        {
                            for (int y = 0; y < tiles.GetLength(1); y++)
                            {
                                if (tiles[x, y] != null)
                                    color2[x + (y * tiles.GetLength(0))] = tiles[x, y].MinimapColor();
                                else
                                    color2[x + (y * tiles.GetLength(0))] = Color.Black;
                            }
                        }
                        Coordinate playerPos = player.position.ToCoordinate();
                        if ((playerPos.x < tiles.GetLength(0) && playerPos.y < tiles.GetLength(1)) && (playerPos.x >= 0 && playerPos.y >= 0))
                            color2[playerPos.x + (playerPos.y * tiles.GetLength(0))] = Color.Red;
                        minimap.SetData(color2);

                        Stream stream = File.OpenWrite("test1.jpg");
                        minimap.SaveAsPng(stream, tiles.GetLength(0), tiles.GetLength(1));
                        stream.Close();
                        takeshot = false;
                    }
                }
            }
            else
            {
                //batch.GraphicsDevice.Clear(Color.Black);
                //batch.DrawString(Assets.GetFont("munro12"), WorldGenerator.worldGenText, Vector2.Zero, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            }
        }

        #region Tile Getters
        public List<Tile> GetTilesAlongLine(Vector2 pointA, Vector2 pointB, float width) //NOTE Width in pixels to test; not tiles 
        {
            List<Tile> tiles = new List<Tile>();
            Vector2 distance = pointB - pointA;

            distance.Normalize();

            List<Coordinate> used = new List<Coordinate>();
            for (int i = 0; i < (pointA - pointB).Length(); i++)
            {
                Coordinate currentCoord = Coordinate.VectorToCoord(pointA + (distance * i));
                
                if (!used.Contains(currentCoord))    //so it doesn't give multiple of the same tile
                {
                    tiles.Add(GetTile(currentCoord));
                    if (width > 1)
                    {
                        Coordinate currentCoordLeft = Coordinate.VectorToCoord(pointA + (distance * i) + (VectorHelper.GetPerp(distance) * width / 2));
                        Coordinate currentCoordRight = Coordinate.VectorToCoord(pointA + (distance * i) + (VectorHelper.GetPerp(distance, true) * width / 2));

                        tiles.Add(GetTile(currentCoordLeft));
                        tiles.Add(GetTile(currentCoordRight));
                    }
                    used.Add(currentCoord);
                }
            }
            return tiles;
        }

        public static List<Coordinate> GetCoordsAlongLine(Tile[,] tiles, Vector2 pointA, Vector2 pointB, float width) //NOTE Width in pixels to test; not tiles 
        {
            List<Coordinate> coords = new List<Coordinate>();
            Vector2 distance = pointB - pointA;

            distance.Normalize();

            List<Coordinate> used = new List<Coordinate>();
            for (int i = 0; i < (pointA - pointB).Length(); i++)
            {
                Coordinate currentCoord = Coordinate.VectorToCoord(pointA + (distance * i));
                coords.Add(currentCoord);
                if (width > 1)
                {
                    Coordinate currentCoordLeft = Coordinate.VectorToCoord(pointA + (distance * i) + (VectorHelper.GetPerp(distance) * width / 2));
                    Coordinate currentCoordRight = Coordinate.VectorToCoord(pointA + (distance * i) + (VectorHelper.GetPerp(distance, true) * width / 2));

                    if (!used.Contains(currentCoordLeft))
                    {
                        coords.Add(currentCoordLeft);
                        used.Add(currentCoordLeft);
                    }
                    if (!used.Contains(currentCoordRight))
                    {
                        coords.Add(currentCoordRight);
                        used.Add(currentCoordRight);
                    }
                }
            }
            return coords;
        }

        public static List<Tile> GetTilesAlongLine(Tile[,] tiles, Vector2 pointA, Vector2 pointB, float width) //NOTE Width in pixels to test; not tiles 
        {
            List<Tile> tiles2 = new List<Tile>();
            Vector2 distance = pointB - pointA;

            distance.Normalize();

            List<Coordinate> used = new List<Coordinate>();
            for (int i = 0; i < (pointA - pointB).Length(); i++)
            {
                Coordinate currentCoord = Coordinate.VectorToCoord(pointA + (distance * i));
                
                tiles2.Add(GetTile(tiles, currentCoord));
                if (width > 1)
                {
                    Coordinate currentCoordLeft = Coordinate.VectorToCoord(pointA + (distance * i) + (VectorHelper.GetPerp(distance) * width / 2));
                    Coordinate currentCoordRight = Coordinate.VectorToCoord(pointA + (distance * i) + (VectorHelper.GetPerp(distance, true) * width / 2));

                    if (!used.Contains(currentCoordLeft))
                    {
                        tiles2.Add(GetTile(tiles, currentCoordLeft));
                        used.Add(currentCoordLeft);
                    }
                    if (!used.Contains(currentCoordRight))
                    {
                        tiles2.Add(GetTile(tiles, currentCoordRight));
                        used.Add(currentCoordRight);
                    }
                    //tiles2.Add(GetTile(tiles, currentCoordLeft));
                    //tiles2.Add(GetTile(tiles, currentCoordRight));
                }
                
            }
            return tiles2;
        }

        public List<Tile> GetTilesToDraw(bool moveOverride = false)
        {
            List<Tile> tiles = new List<Tile>();

            if (player.moving || moveOverride)
            {
                Coordinate start = Coordinate.VectorToCoord(Main.camera.worldCenter);

                for (int x = -15; x < 16; x++)
                {
                    for (int y = -17; y < 16; y++)
                    {
                        Tile t = GetTile(new Coordinate(start.x + x, start.y + y));
                        if (t != null && t.texture != null)
                            tiles.Add(t);
                    }
                }
                drawTiles = tiles;
            }
            return drawTiles;
        }

        public List<Tile> GetNearTiles(Entity entity, int distance = 1)
        {
            List<Tile> tiles = new List<Tile>();

            Coordinate start = Coordinate.VectorToCoord(entity.center);

            for (int x = -distance; x <= distance; x++)
            {
                for (int y = -distance; y <= distance; y++)
                {
                    Tile t = GetTile(new Coordinate(start.x + x, start.y + y));
                    if (t != null)
                        tiles.Add(t);
                }
            }
            return tiles;
        }

        public Tile GetTile(Coordinate position)
        {
            if ((position.x >= 0 && position.y >= 0) && (position.x < tiles.GetLength(0) && position.y < tiles.GetLength(1)))
                return tiles[position.x, position.y];
            else return null;
        }

        public static Tile GetTile(Tile[,] tiles, Coordinate position)
        {
            if ((position.x >= 0 && position.y >= 0) && (position.x < tiles.GetLength(0) && position.y < tiles.GetLength(1)))
                return tiles[position.x, position.y];
            else return null;
        }
        #endregion

        public void LoadWorld(Player2 player)
        {
            GuiLoading.DEBUGLOADINGINFO = "Loading save file...";
            PlayerSave save = SerializeHelper.LoadSave(@"Saves/save.json");

            GuiLoading.DEBUGLOADINGINFO = "Loading world file: " + save.map;

            if (!Directory.Exists("Maps"))
                Directory.CreateDirectory("Maps");

            SerWorld w = SerializeHelper.LoadWorld("Maps/" + save.map);

            tiles = new Tile[w.bounds.Width / 48, w.bounds.Height / 48];

            for (int i = 0; i < w.serTiles.Count; i++)
            {
                GuiLoading.DEBUGLOADINGINFO = "Adding tiles " + w.serTiles.Count + " to world...\n" + i + "/" + w.serTiles.Count;
                Tile t = w.serTiles[i];

                if (t != null)
                {
                    tiles[(int)(t.realPosition.X / 48), (int)(t.realPosition.Y / 48)] = t;
                }
            }

            entities.AddRange(w.spawners);
            spawners.AddRange(w.spawners);
            this.player = new Player2(player.position);

            foreach(EntitySpawner findPlayerSpawner in spawners)
            {
                if (findPlayerSpawner.type == 0)
                    player.position = findPlayerSpawner.position;
            }
            entities.Add(this.player);
        }

        public void UnloadWorld()
        {
            entities.Clear();
            enemies.Clear();
            projectiles.Clear();
            //player = null;
        }

        #region creation
        
        public Thread CreateWorld(Player2 player)
        {
            tiles = new Tile[256, 256];
            return mapLoadThread;
        }

        public Tile CreateTile(Tile tile)
        {
            if (tile != null && (tile.position.x >= 0 && tile.position.y >= 0 && tile.position.x < tiles.GetLength(0) && tile.position.y < tiles.GetLength(1)))
            {
                tiles[tile.position.x, tile.position.y] = tile;
                if (tile is TileWall)
                    wallTiles.Add((TileWall)tile);
            }
            //else
                //Logger.Log("Could not create tile", false);
            return tile; 
        }

        public Particle CreateParticle(Particle p)
        {
            entities.Add(p);
            particles.Add(p);
            return p;
        }

        public Enemy CreateEnemy(Enemy e)
        {
            entities.Add(e);
            enemies.Add(e);
            return e;
        }

        public EntitySpawner CreateSpawner(EntitySpawner e)
        {
            entities.Add(e);
            return e;
        }

        public Projectile CreateProjectile(Projectile p)
        {
            entities.Add(p);
            projectiles.Add(p);
            return p;
        }

        public Collectable CreateCollectable(Collectable c)
        {
            entities.Add(c);
            collectables.Add(c);
            return c;
        }

        public Bomb CreateBomb(Bomb b)
        {
            entities.Add(b);
            bombs.Add(b);
            return b;
        }

        public NPC CreateNPC(NPC n)
        {
            entities.Add(n);
            npcs.Add(n);
            return n;
        }
        #endregion
    }
}
