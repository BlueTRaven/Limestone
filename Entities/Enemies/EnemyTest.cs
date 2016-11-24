using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Tiles;
using Limestone.Items;


namespace Limestone.Entities.Enemies
{
    public class EnemyTest : Enemy
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        int bombTimer = 0;

        float dist;
        public EnemyTest(Vector2 position) : base(position)
        {
            this.position = position;
            frameConfiguration = new FrameConfiguration();
            SetDefaults();
        }

        private void SetDefaults()
        {
            name = "Test";
            hitSound = Assets.GetSoundEffect("woodImpact2");
            dieSound = Assets.GetSoundEffect("woodImpact1");

            scale = 4;
            shadowScale = 2;

            health = 1;
            maxHealth = health;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(32)));
            activeDistance = 1024;

            setSize = new Rectangle(0, 0, 8, 8);
        }

        public override void Update(World world)
        {
            base.Update(world);

            bombTimer--;

            /*if (bombTimer <= 0)
            {
                Bomb b = new Bomb(null, Color.Red, 4, position, new Vector2(2), 64, 0, 0, -rotToPlayer, 0, 3.5f, distance, 0);
                world.CreateBomb(b);
                bombTimer = 15;
            }*/

            float start = center.X - 768;

            if (bombTimer <= 0)
            {
                float offset = (float)Main.rand.NextDouble(0, 360);
                float count = Main.rand.Next(6, 12);
                for (float i = 0; i < 360f; i += (360f / count))
                {
                    Bomb b = new Bomb(null, Color.Red, 4, position, new Vector2(2), 32, 0, 0, i + offset, 0, 2, dist, 0);
                    world.CreateBomb(b);
                }
                bombTimer = 32;

                dist += 16;

                if (dist > 256)
                    dist = 32;
            }
        }

        protected override void RunFrameConfiguration()
        {
        }

        public override Entity Copy()
        {
            return new EnemyTest(position);
        }
    }
}
