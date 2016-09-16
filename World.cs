using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Component;

namespace Limestone
{
    public class World
    {
        public Player player;
        public Camera2D camera;

        Effect testEffect;

        public List<Projectile> projectiles = new List<Projectile>();
        public List<Enemy> enemies = new List<Enemy>();
        public List<Entity> entities = new List<Entity>();

        RenderTarget2D enemyRenderTarget;
        RenderTarget2D projectileRenderTarget;

        public RotationRect rectr;
        public Line testLine;
        public World(Camera2D camera)
        {
            this.camera = camera;
            player = new Player(new Vector2(32, 128), Vector2.Zero);
            entities.Add(player);
            testEffect = Assets.GetEffect("test");

            rectr = new RotationRect(Vector2.Zero, new Vector2(64), 0);
            testLine = new Line(new Vector2(64, 0), new Vector2(64, 64));
        }

        public void Update()
        {
            player.Update(this);

            if (Main.KeyPress(Keys.F))
            {
                CreateEnemy(0, new Vector2(0, 0), 4);
                CreateEnemy(0, new Vector2(128, 64), 4);
            }

            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(this);

                if (entities[i] is Projectile)
                {
                    Projectile p = (Projectile)entities[i];
                    if (player.hitbox.Intersects(p.hitbox))
                    {
                        if (!p.hitPlayer)
                            player.TakeDamage(p.damage, p);

                        if (!p.piercing)
                            entities[i].Die(this);
                        else
                            p.hitPlayer = true;
                    }
                }

                if (entities[i].dead)
                    entities.RemoveAt(i);
            }

            camera.center = player.center;
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (Entity e in entities)
            {
                e.DrawOutline(batch);
            }
            foreach (Entity e in entities)
            {
                e.Draw(batch);
            }

            player.Draw(batch);

            //rectr.Draw(batch);
            //testLine.Draw(batch);
        }

        public void ChangeWorld(string toLoc)
        {
            UnloadWorld();
            LoadWorld(toLoc);
        }

        public void LoadWorld(string loadFrom)
        {

        }

        public void UnloadWorld()
        {

        }

        public Enemy CreateEnemy(int type, Vector2 position, float scale)
        {
            Enemy e = new Enemy(type, position, scale);
            entities.Add(e);
            enemies.Add(e);
            return e;
        }

        public Projectile CreateProjectile(int type, int damage, Vector2 position, float angle, float speed, float distance, float scale = 1, Projectile parent = null)
        {
            Projectile p = new Projectile(type, damage, position, scale, angle, speed, distance, parent);
            entities.Add(p);
            projectiles.Add(p);
            return p;
        }

        public Projectile CreateProjectile(out Projectile projectile)
        {//With this method, you can assign to all variables manually.
            Projectile p = new Projectile(0, 0, Vector2.Zero, 1, 0, 0, 0, null);
            projectile = p;
            entities.Add(p);
            projectiles.Add(p);
            return p;
        }
    }
}
