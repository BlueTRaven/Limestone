using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Tiles;
using Limestone.Entities.Enemies;

namespace Limestone.Entities
{
    public class EnemySpawner : Entity
    {
        public static bool DEBUGDRAWSPAWNERS = true;
        public override Vector2 center { get { return Vector2.Zero; } set { } }
        public int type, rate;
        List<Enemy> enemies = new List<Enemy>();
        int types;
        int cooldown;

        private bool rerolls;

        private bool spawned;

        private int spawnDistance;  ///Distance from spawner that entities are spawned at.
        
        public EnemySpawner(Vector2 position, int type)
        {
            this.tType = EntityType.Spawner;
            this.position = position;
            this.type = type;
            this.tileCollides = false;
            SetDefaults();
        }

        private void SetDefaults()
        {
            rerolls = false;
            if (type == 0)
            {
                spawnDistance = 512;
                enemies.Add(new EnemyBossMonolith(position));
            }
        }

        public void RerollEnemies()
        {
            enemies.Clear();
            spawned = false;
            SetDefaults();
        }

        public void SpawnEnemies(World world, bool force = false)
        {
            if (force || cooldown <= 0)
            {
                foreach (Enemy e in enemies)
                {
                    world.CreateEnemy(e.Copy());
                }

                if (rerolls)
                    RerollEnemies();
            }
        }

        public override void Die(World world)
        {   //NOOP
        }

        public override void Draw(SpriteBatch batch)
        {   //NOOP
            if (DEBUGDRAWSPAWNERS)
                DrawGeometry.DrawHollowRectangle(batch, new Rectangle(position.ToPoint(), new Point(48)), 1, Color.White);
        }

        public override void DrawOutline(SpriteBatch batch)
        {   //NOOP
        }

        public override void Update(World world)
        {   //NOOP
            if ((world.player.position - position).Length() <= spawnDistance && !spawned)
            {
                SpawnEnemies(world, true);
                spawned = true;
            }
        }
    }
}
