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

using Limestone.Interface;
using Limestone.Guis;

namespace Limestone.Entities.Enemies
{
    public class EnemyStalactite : Enemy
    {
        private float gravity = -.5f, velZ, activateDistance;

        private bool falling = false;

        public EnemyStalactite(Vector2 position, float activateDistance) : base(position)
        {
            scale = 4;
            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(32)));
            activeDistance = 1024;
            health = 1;
            maxHealth = 1;

            height = 512;

            setSize = new Rectangle(0, 0, 8, 8);

            baseTexture = texture;

            untargetTicks = 9999;

            this.activateDistance = activateDistance;
        }

        public override void Update(World world)
        {
            base.Update(world);

            if (distFromPlayer < activateDistance)
                falling = true;

            if (falling)
            {
                velZ += gravity;
                height += velZ;

                if (height < 32)
                {
                    Projectile p = new Projectile(Assets.GetTexture("whitePixel"), Color.White, 0, center, Vector2.Zero, new Vector2(32, 32), 0, 0, .5f, 1, 1);
                    world.CreateProjectile(p);
                }

                if (height < 0)
                {
                    for (int i = 0; i < Main.rand.Next(4, 16); i++)
                        world.CreateParticle(new Particle(null, center, (float)Main.rand.NextDouble(0, 360), 2, 32, Color.Gray, 4));

                    Die(world);
                }
            }
        }

        public override Entity Copy()
        {
            return new EnemyStalactite(position, activateDistance);
        }
    }
}
