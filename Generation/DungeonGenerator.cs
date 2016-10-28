using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Tiles;
using Limestone.Entities;

namespace Limestone.Generation
{
    public class DungeonGenerator
    {
        int type;

        private World world;
        public Rectangle maxbounds;
        private Rectangle startBounds;
        private Rectangle roomBounds;
        private Rectangle endBounds;
        private string endSides = "";
        private int doorWidth;

        private int maxIterations;

        public List<Room> rooms = new List<Room>();
        public Room start;

        private List<TileWall> wallTiles = new List<TileWall>();
        private List<TileFloor> floorTiles = new List<TileFloor>();
        public DungeonGenerator(World world, int type)
        {
            this.world = world;
            this.type = type;

            SetDefaults();
        }


        private void SetDefaults()
        {
            if (type == 0)
            {
                maxbounds = new Rectangle(0, 0, 256, 256);
                startBounds = new Rectangle(0, 0, 25, 25);
                roomBounds = new Rectangle(0, 0, 20, 20);
                endBounds = new Rectangle(0, 0, 15, 15);
                doorWidth = 4;
                maxIterations = 50;
                endSides = "up down left right";

                floorTiles.Add(TileFloor.Create(Coordinate.Zero, "rocksAncientlands2", Biomes.Beach));

                wallTiles.Add(TileWall.Create(Coordinate.Zero,"brickTop", "brick1", Biomes.Rock));
                wallTiles.Add(TileWall.Create(Coordinate.Zero, "brickTop", "brick2", Biomes.Rock));
            }
            else if (type == 1)
            {
                maxbounds = new Rectangle(0, 0, 256, 256);
                startBounds = new Rectangle(0, 0, 35, 35);
                roomBounds = new Rectangle(0, 0, 20, 20);
                endBounds = new Rectangle(0, 0, 15, 15);
                doorWidth = 4;
                maxIterations = 50;
                endSides = "up down left right";

                floorTiles.Add(TileFloor.Create(Coordinate.Zero, "purple0", Biomes.AncientLands));
                floorTiles.Add(TileFloor.Create(Coordinate.Zero, "purple0", Biomes.AncientLands));
                floorTiles.Add(TileFloor.Create(Coordinate.Zero, "purple0", Biomes.AncientLands));
                floorTiles.Add(TileFloor.Create(Coordinate.Zero, "purple0", Biomes.AncientLands));
                floorTiles.Add(TileFloor.Create(Coordinate.Zero, "purple1", Biomes.AncientLands));
                floorTiles.Add(TileFloor.Create(Coordinate.Zero, "purple1", Biomes.AncientLands));
                floorTiles.Add(TileFloor.Create(Coordinate.Zero, "purple1", Biomes.AncientLands));
                floorTiles.Add(TileFloor.Create(Coordinate.Zero, "purple2", Biomes.AncientLands));

                wallTiles.Add(TileWall.Create(Coordinate.Zero, "brickTop", "brick1", Biomes.Rock));
                wallTiles.Add(TileWall.Create(Coordinate.Zero, "brickTop", "brick1", Biomes.Rock));
                wallTiles.Add(TileWall.Create(Coordinate.Zero, "brickTop", "brick1", Biomes.Rock));
                wallTiles.Add(TileWall.Create(Coordinate.Zero, "brickTop", "brick1", Biomes.Rock));
                wallTiles.Add(TileWall.Create(Coordinate.Zero, "brickTop", "brick1", Biomes.Rock));
                wallTiles.Add(TileWall.Create(Coordinate.Zero, "brickTop", "brick2", Biomes.Rock));
                wallTiles.Add(TileWall.Create(Coordinate.Zero, "brickTop", "brick2", Biomes.Rock));
            }
        }

        public void Generate()
        {
            int startX = Main.rand.Next(0, maxbounds.Width - startBounds.Width);
            int startY = Main.rand.Next(0, maxbounds.Height - startBounds.Height);

            start = AddRoom(new Room(new Rectangle(startX, startY, startBounds.Width, startBounds.Height)));
            start.FindValidAdjacents(maxbounds, roomBounds, doorWidth, rooms);
            start.isStartRoom = true;

            int iterations = 0;
            Room currentRoom = start;
            int currentRoomIndex = 1;
            while (true)
            {
                if (iterations > maxIterations * 4)
                {
                    Console.WriteLine("Tried to generate too many rooms. Stopping...");
                    break;
                }
                if (iterations >= maxIterations - 1)
                {   //after maxiterations it starts trying to generate an endroom.
                    currentRoom.FindValidEndRoom(maxbounds, endBounds, doorWidth, rooms, endSides, true);
                    if (currentRoom.foundEnd)
                    {   //if it has found one
                        int index = Main.rand.Next(0, currentRoom.adjancentsBounds.Count);

                        Room newRoom = new Room(currentRoom.adjancentsBounds[index]);
                        newRoom.isEndRoom = true;
                        currentRoom.AddAdjacent(newRoom, new Door(currentRoom.adjancentsDoorsBounds[index]));
                        newRoom.AddAdjacent(currentRoom, new Door(currentRoom.adjancentsDoorsBounds[index]));
                        //newRoom.adjacentRooms.Add(currentRoom);
                        AddRoom(newRoom);
                        break;
                    }   //if it hasn't, it continues - thus generating another room.
                    else
                        currentRoom.FindValidAdjacents(maxbounds, roomBounds, doorWidth, rooms);
                }
                if (currentRoom.hasValidAdjacent)
                {
                    int index = Main.rand.Next(0, currentRoom.adjancentsBounds.Count);
                    Room newRoom = new Room(currentRoom.adjancentsBounds[index]);
                    newRoom.FindValidAdjacents(maxbounds, roomBounds, doorWidth, rooms);

                    currentRoom.AddAdjacent(newRoom, new Door(currentRoom.adjancentsDoorsBounds[index]));
                    newRoom.AddAdjacent(currentRoom, new Door(currentRoom.adjancentsDoorsBounds[index]));

                    currentRoom = AddRoom(newRoom);
                    currentRoomIndex++;
                }
                else if (rooms.Count == 0)
                {
                    Console.WriteLine("Couldn't generate any more rooms - was not given a starting room!");
                    break;
                }
                else
                {
                    currentRoomIndex--;

                    if (currentRoomIndex < 0)
                    {
                        Console.WriteLine("Reached start room and could not generate any more rooms. Maxbounds is filled!");
                        break;
                    }
                    currentRoom.isDeadEndRoom = true;
                    currentRoom = rooms[currentRoomIndex];
                    currentRoom.FindValidAdjacents(maxbounds, roomBounds, doorWidth, rooms);
                    Console.WriteLine("Could not generate any more rooms here, going back...");
                }
                iterations++;
            }

            foreach (Room r in rooms.ToList())
            {
                if (r.adjancentsDoorsBounds.Count == 0 && r.adjacentRooms.Count == 0)
                    rooms.Remove(r);
            }

            GenerateRooms(world);

            world.player = new Player(Vector2.Zero, Class.Archer);
            world.entities.Add(world.player);

            world.player.position = start.bounds.ToCoordinateScale().Center.ToVector2();
        }

        public void GenerateRooms(World world)
        {
            foreach (Room r in rooms)
            {
                if (r.adjacentDoors.Count > 0)
                {
                    for (int x = 0; x < r.bounds.Width; x++)
                    {
                        for (int y = 0; y < r.bounds.Height; y++)
                        {
                            int xpos = x + r.bounds.Location.X;
                            int ypos = y + r.bounds.Location.Y;
                            if (x == 0 || y == 0 || x == r.bounds.Width - 1 || y == r.bounds.Height - 1)
                            {
                                TileWall tile = wallTiles[Main.rand.Next(0, wallTiles.Count)];
                                world.tiles[xpos, ypos] = tile.Copy(new Coordinate(xpos, ypos));//new TileWall(new Coordinate(xpos, ypos), Assets.GetTexture("rocksAncientlands2"), Assets.GetTexture("rocksAncientlands2"), Biomes.LowLands, false);
                            }
                            else
                            {
                                TileFloor tile = floorTiles[Main.rand.Next(0, floorTiles.Count)];
                                world.tiles[xpos, ypos] = tile.Copy(new Coordinate(xpos, ypos));//new TileFloor(new Coordinate(xpos, ypos), Assets.GetTexture("beach2"), Biomes.LowLands, false);
                            }
                        }
                    }

                    if (type == 1)
                    {
                        for (int x = 0; x < r.bounds.Width; x++)
                        {
                            for (int y = 0; y < r.bounds.Height; y++)
                            {
                                int xpos = x + r.bounds.Location.X;
                                int ypos = y + r.bounds.Location.Y;

                                if (x == 1 || y == 1 || x == r.bounds.Width - 2 || y == r.bounds.Height - 2 || Main.rand.Next(0, 6) == 0)
                                {
                                    TileFloor tile = new TileFloor(Coordinate.Zero, "grassAncientlands1", Biomes.AncientLands);
                                    world.tiles[xpos, ypos] = tile.Copy(new Coordinate(xpos, ypos));
                                }
                            }
                        }
                    }
                    for (int x = 0; x < r.bounds.Width; x++)
                    {
                        for (int y = 0; y < r.bounds.Height; y++)
                        {
                            int xpos = x + r.bounds.Location.X;
                            int ypos = y + r.bounds.Location.Y;
                            if (x == 0 || y == 0 || x == r.bounds.Width - 1 || y == r.bounds.Height - 1)
                            {
                                TileWall tile = wallTiles[Main.rand.Next(0, wallTiles.Count)];
                                world.tiles[xpos, ypos] = tile.Copy(new Coordinate(xpos, ypos));//new TileWall(new Coordinate(xpos, ypos), Assets.GetTexture("rocksAncientlands2"), Assets.GetTexture("rocksAncientlands2"), Biomes.LowLands, false);
                            }
                        }
                    }
                }

                Vector2 roomCenter = r.bounds.ToCoordinateScale().Center.ToVector2();

                if (!r.isStartRoom)
                {
                    //EnemySpawner spawner = new EnemySpawner(roomCenter, 0);
                    //world.CreateSpawner(spawner);
                    //spawner.SpawnEnemies(world, true);
                }

                if (r.isStartRoom)
                {
                    TileWall tile = wallTiles[Main.rand.Next(0, wallTiles.Count)];

                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            tile = wallTiles[Main.rand.Next(0, wallTiles.Count)];
                            world.tiles[r.bounds.Location.X + 4 + x, r.bounds.Location.Y + 4 + y] = tile.Copy(new Coordinate(r.bounds.Location.X + 4 + x, r.bounds.Location.Y + 4 + y));
                        }
                    }

                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            tile = wallTiles[Main.rand.Next(0, wallTiles.Count)];
                            world.tiles[(r.bounds.Location.X + r.bounds.Width - 5) - x, r.bounds.Location.Y + 4 + y] = tile.Copy(new Coordinate((r.bounds.Location.X + r.bounds.Width - 5) - x, r.bounds.Location.Y + 4 + y));
                        }
                    }

                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            tile = wallTiles[Main.rand.Next(0, wallTiles.Count)];
                            world.tiles[(r.bounds.Location.X + r.bounds.Width - 5) - x, (r.bounds.Location.Y + r.bounds.Height - 5) - y] = tile.Copy(new Coordinate((r.bounds.Location.X + r.bounds.Width - 5) - x, (r.bounds.Location.Y + r.bounds.Height - 5) - y));
                        }
                    }

                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            tile = wallTiles[Main.rand.Next(0, wallTiles.Count)];
                            world.tiles[r.bounds.Location.X + 4 + x, (r.bounds.Location.Y + r.bounds.Height - 5) - y] = tile.Copy(new Coordinate(r.bounds.Location.X + 4 + x, (r.bounds.Location.Y + r.bounds.Height - 5) - y));
                        }
                    }

                    /*//Enemy shu = world.CreateEnemy(Enemy.Create(0, roomCenter - new Vector2(Coordinate.coordSize + 16), true));
                    //Enemy tefnut = world.CreateEnemy(Enemy.Create(1, roomCenter - new Vector2(Coordinate.coordSize + 16), true));

                    List<Enemy2> sarcs = new List<Enemy2>();
                    for (int i = 0; i < rooms.Count; i++)
                    { 
                        if (rooms[i].isEndRoom)
                        {
                            sarcs.Add(world.CreateEnemy(Enemy2.Create(9, new Vector2(rooms[i].bounds.Center.X * Coordinate.coordSize, rooms[i].bounds.Center.Y * Coordinate.coordSize), false)));
                        }
                    }
                    //shu.children.AddRange(sarcs);
                    //tefnut.children.AddRange(sarcs);*/
                }

                if (r.isEndRoom)
                {

                }
            }

            foreach (Room r in rooms)
            {
                foreach (Door door in r.adjacentDoors)
                {
                    for (int x = 0; x < door.bounds.Width; x++)
                    {
                        for (int y = 0; y < door.bounds.Height; y++)
                        {
                            int xpos = x + door.bounds.Location.X;
                            int ypos = y + door.bounds.Location.Y;

                            world.tiles[xpos, ypos] = floorTiles[Main.rand.Next(0, floorTiles.Count)].Copy(new Coordinate(xpos, ypos));//new TileFloor(new Coordinate(xpos, ypos), Assets.GetTexture("beach2"), Biomes.LowLands, false);
                            if (door.bounds.Height > door.bounds.Width)
                            {
                                if (y == 0 || y == door.bounds.Height - 1)
                                {
                                    world.tiles[xpos, ypos] = wallTiles[Main.rand.Next(0, wallTiles.Count)].Copy(new Coordinate(xpos, ypos));//new TileWall(new Coordinate(xpos, ypos), Assets.GetTexture("rocksAncientlands2"), Assets.GetTexture("rocksAncientlands2"), Biomes.LowLands, false);
                                    world.tiles[xpos - 1, ypos] = wallTiles[Main.rand.Next(0, wallTiles.Count)].Copy(new Coordinate(xpos - 1, ypos));//new TileWall(new Coordinate(xpos - 1, ypos), Assets.GetTexture("rocksAncientlands2"), Assets.GetTexture("rocksAncientlands2"), Biomes.LowLands, false);
                                    world.tiles[xpos + 1, ypos] = wallTiles[Main.rand.Next(0, wallTiles.Count)].Copy(new Coordinate(xpos + 1, ypos)); //new TileWall(new Coordinate(xpos + 1, ypos), Assets.GetTexture("rocksAncientlands2"), Assets.GetTexture("rocksAncientlands2"), Biomes.LowLands, false);
                                }
                                else
                                {
                                    world.tiles[xpos - 1, ypos] = floorTiles[Main.rand.Next(0, floorTiles.Count)].Copy(new Coordinate(xpos - 1, ypos));//new TileFloor(new Coordinate(xpos - 1, ypos), Assets.GetTexture("beach2"), Biomes.LowLands, false);
                                    world.tiles[xpos + 1, ypos] = floorTiles[Main.rand.Next(0, floorTiles.Count)].Copy(new Coordinate(xpos + 1, ypos));//new TileFloor(new Coordinate(xpos + 1, ypos), Assets.GetTexture("beach2"), Biomes.LowLands, false);
                                }
                            }
                            else if (door.bounds.Width > door.bounds.Height)
                            {
                                if (x == 0 || x == door.bounds.Width - 1)
                                {
                                    world.tiles[xpos, ypos] = wallTiles[Main.rand.Next(0, wallTiles.Count)].Copy(new Coordinate(xpos, ypos));//new TileWall(new Coordinate(xpos, ypos), Assets.GetTexture("rocksAncientlands2"), Assets.GetTexture("rocksAncientlands2"), Biomes.LowLands, false);
                                    world.tiles[xpos, ypos - 1] = wallTiles[Main.rand.Next(0, wallTiles.Count)].Copy(new Coordinate(xpos, ypos - 1));//new TileWall(new Coordinate(xpos, ypos - 1), Assets.GetTexture("rocksAncientlands2"), Assets.GetTexture("rocksAncientlands2"), Biomes.LowLands, false);
                                    world.tiles[xpos, ypos + 1] = wallTiles[Main.rand.Next(0, wallTiles.Count)].Copy(new Coordinate(xpos, ypos + 1)); //new TileWall(new Coordinate(xpos, ypos + 1), Assets.GetTexture("rocksAncientlands2"), Assets.GetTexture("rocksAncientlands2"), Biomes.LowLands, false);
                                }
                                else
                                {
                                    world.tiles[xpos, ypos - 1] = floorTiles[Main.rand.Next(0, floorTiles.Count)].Copy(new Coordinate(xpos, ypos - 1));//new TileFloor(new Coordinate(xpos, ypos - 1), Assets.GetTexture("beach2"), Biomes.LowLands, false);
                                    world.tiles[xpos, ypos + 1] = floorTiles[Main.rand.Next(0, floorTiles.Count)].Copy(new Coordinate(xpos, ypos + 1));//new TileFloor(new Coordinate(xpos, ypos + 1), Assets.GetTexture("beach2"), Biomes.LowLands, false);
                                }
                            }
                        }
                    }
                }
            }
        }

        public Room AddRoom(Room room)
        {
            rooms.Add(room);
            return room;
        }
    }
}
