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
using Limestone.Entities.NPCs;
using Limestone.Entities.Collectables;
using Limestone.Serialization;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Limestone.Entities
{
    [Serializable]
    public class EntitySpawner : Entity, ISerializable
    {
        public static bool DEBUGDRAWSPAWNERS = true;
        public override Vector2 center { get { return Vector2.Zero; } set { } }
        public int type, rate;
        List<Entity> entities = new List<Entity>();
        int types;
        int cooldown;

        private bool rerolls;

        private bool spawned;

        private int spawnDistance;  ///Distance from spawner that entities are spawned at.
        
        public EntitySpawner(Vector2 position, int type) : base(position)
        {
            this.tType = EntityType.Spawner;
            this.type = type;
            this.tileCollides = false;
            SetDefaults();
        }

        protected EntitySpawner(SerializationInfo info, StreamingContext context) : base(Vector2.Zero)
        {
            type = info.GetInt32("type");
            position = ((SerVector)info.GetValue("position", typeof(SerVector))).ToVector2();

            tType = EntityType.Spawner;
            tileCollides = false;
            SetDefaults();
        }

        private void SetDefaults()
        {
            rerolls = false;    //set within typing
            if (type == 1)
            {
                spawnDistance = 512;
                entities.Add(new EnemyStalactite(position, 32));
            }
        }

        public void RerollEnemies()
        {
            entities.Clear();
            spawned = false;
            SetDefaults();
        }

        public void SpawnEnemies(World world, bool force = false)
        {
            if (force || cooldown <= 0)
            {
                foreach (Entity e in entities)
                {
                    if (e.tType == EntityType.Enemy)
                        world.CreateEnemy((Enemy)e.Copy());
                    else if (e.tType == EntityType.Collectable)
                        world.CreateCollectable((Collectable)e.Copy());
                    else if (e.tType == EntityType.NPC)
                        world.CreateNPC((NPC)e.Copy());
                }

                if (rerolls)
                    RerollEnemies();
            }
        }

        public override void Die(World world)
        {   //NOOP
        }

        protected override void RunFrameConfiguration()
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

        public override void OnTileCollide(World world, Tile tile)
        {   //NOOP
        }

        public override Entity Copy()
        {   //NOOP
            return null;
        }
    }
}
