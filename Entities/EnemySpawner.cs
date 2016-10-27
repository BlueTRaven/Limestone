using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Tiles;

namespace Limestone.Entities
{
    public class EnemySpawner : Entity
    {
        public override Vector2 center { get { return Vector2.Zero; } set { } }
        public int type, rate;
        List<Enemy> enemies = new List<Enemy>();
        int types;
        int cooldown;
        public EnemySpawner(Vector2 position, int type)
        {
            this.tType = EntityType.Spawner;
            this.position = position;
            this.type = type;
            SetDefaults();
        }
        public void RerollEnemies()
        {
            enemies.Clear();
            SetDefaults();
        }
        private void SetDefaults()
        {
            /*if (type == 0)
            {
                rate = 0;
                int chance = Main.rand.Next(types = 8);

                if (chance == 0)
                {
                    enemies.Add(Enemy2.Create(3, position));
                    enemies.Add(Enemy2.Create(1, position + new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(8 * Coordinate.coordSize, -(8 * Coordinate.coordSize))));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(-(8 * Coordinate.coordSize), 8 * Coordinate.coordSize)));
                }
                else if (chance == 1)
                {
                    enemies.Add(Enemy2.Create(2, position + new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(2, position - new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(8 * Coordinate.coordSize, -(8 * Coordinate.coordSize))));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(-(8 * Coordinate.coordSize), 8 * Coordinate.coordSize)));
                }
                else if (chance == 2)
                {
                    enemies.Add(Enemy2.Create(1, position + new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(2, position - new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(8 * Coordinate.coordSize, -(8 * Coordinate.coordSize))));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(-(8 * Coordinate.coordSize), 8 * Coordinate.coordSize)));
                }
                else if (chance == 3)
                {
                    enemies.Add(Enemy2.Create(2, position + new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(8 * Coordinate.coordSize, -(8 * Coordinate.coordSize))));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(-(8 * Coordinate.coordSize), 8 * Coordinate.coordSize)));
                }
                else if (chance == 4)
                {
                    enemies.Add(Enemy2.Create(4, position));
                    enemies.Add(Enemy2.Create(1, position + new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(8 * Coordinate.coordSize, -(8 * Coordinate.coordSize))));
                    enemies.Add(Enemy2.Create(2, position - new Vector2(-(8 * Coordinate.coordSize), 8 * Coordinate.coordSize)));
                }
                else if (chance == 5)
                {
                    enemies.Add(Enemy2.Create(4, position));
                    enemies.Add(Enemy2.Create(5, position + new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(5, position - new Vector2(8 * Coordinate.coordSize)));
                }
                else if (chance == 6)
                {
                    enemies.Add(Enemy2.Create(4, position));
                    enemies.Add(Enemy2.Create(3, position + new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(3, position - new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(3, position - new Vector2(8 * Coordinate.coordSize, -(8 * Coordinate.coordSize))));
                    enemies.Add(Enemy2.Create(3, position - new Vector2(-(8 * Coordinate.coordSize), 8 * Coordinate.coordSize)));
                }
                else
                {
                    enemies.Add(Enemy2.Create(1, position + new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(1, position - new Vector2(8 * Coordinate.coordSize)));
                    enemies.Add(Enemy2.Create(2, position - new Vector2(8 * Coordinate.coordSize, -(8 * Coordinate.coordSize))));
                    enemies.Add(Enemy2.Create(2, position - new Vector2(-(8 * Coordinate.coordSize), 8 * Coordinate.coordSize)));
                }
            }
            else if (type == 1)
            {
                rate = 2000;
                int chance = Main.rand.Next(types = 4);

                if (chance == 0)
                {
                    enemies.Add(Enemy2.Create(3, position));
                    enemies.Add(Enemy2.Create(1, position + new Vector2(48)));
                }
                else if (chance == 1)
                {
                    enemies.Add(Enemy2.Create(1, position));
                    enemies.Add(Enemy2.Create(3, position + new Vector2(48)));
                }
                else if (chance == 2)
                {
                    enemies.Add(Enemy2.Create(3, position + new Vector2(48, 0)));
                    enemies.Add(Enemy2.Create(3, position + new Vector2(0, 48)));
                }
                else if (chance == 3)
                {
                    enemies.Add(Enemy2.Create(3, position));
                    enemies.Add(Enemy2.Create(1, position + new Vector2(48)));
                    enemies.Add(Enemy2.Create(3, position + new Vector2(96)));
                }
            }*/
        }

        public void SpawnEnemies(World world, bool force = false)
        {
            if (force || cooldown <= 0 && (world.player.position - position).Length() > 1024)
            {
                foreach (Enemy e in enemies)
                {
                    //world.CreateEnemy(e.Copy());
                }
                RerollEnemies();
            }
        }

        public override void Die(World world)
        {   //NOOP
        }

        public override void Draw(SpriteBatch batch)
        {   //NOOP
            DrawGeometry.DrawHollowRectangle(batch, new Rectangle(position.ToPoint(), new Point(48)), 1, Color.White);
        }

        public override void DrawOutline(SpriteBatch batch)
        {   //NOOP
        }

        public override void Update(World world)
        {   //NOOP
        }
    }
}
