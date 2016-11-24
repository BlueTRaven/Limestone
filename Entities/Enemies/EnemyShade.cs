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
    public class EnemyShade : Enemy
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        private int shot0, shot1, prevcount;
        public EnemyShade(Vector2 position) : base(position)
        {
            this.position = position;
            SetDefaults();
        }

        private void SetDefaults()
        {
            name = "Shade";
            texture = Assets.GetTexture("shade");
            hitSound = Assets.GetSoundEffect("squeakImpact1");
            dieSound = Assets.GetSoundEffect("deathMonster1");
            //shadowTextureColor = new Color(255, 255, 0, 127);

            speed = 2;
            health = 4500;
            scale = 4;
            shadowScale = 4;
            flips = true;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(48)));
            activeDistance = 512;

            setSize = new Rectangle(0, 0, 8, 8);

            frameConfiguration = new FrameConfiguration(FrameConfiguration.FrameActionPresetactive1inactive2, this,
                new FrameCollection(false,
                new Frame(15, new Rectangle(0, 0, 8, 8)),
                new Frame(15, new Rectangle(8, 0, 8, 8)),
                new Frame(15, new Rectangle(16, 0, 8, 8))),

                new FrameCollection(true,
                new Frame(15, new Rectangle(40, 0, 16, 8)),
                new Frame(15, new Rectangle(32, 0, 8, 8)))
                );

            maxHealth = health;
        }

        public override void Update(World world)
        {
            base.Update(world);

            if (!stunned)
            {
                shot0--;
                shot1--;
            }

            if (distFromPlayer >= 96)
            {
                if (!idleMove)
                {
                    moving = true;
                    Vector2 moveDirec = Vector2.Normalize(world.player.center - center);
                    Move(moveDirec, 2);
                }
            }
            else
            {
                SetIdle(15, 2);
            }

            if (distFromPlayer < 192)
            {
                if (shot0 <= 0)
                {
                    int rand = Main.rand.Next(1, 4);
                    while (rand == prevcount)
                        rand = Main.rand.Next(1, 4);
                    for (float i = -rand; i <= rand; i++)
                    {
                        //Projectile p = new Projectile(Assets.GetTexFromSource("projectilesFull", 8, 0), new Color(77, 58, 84), position - new Vector2(16), new Vector2(4, 8), true, 4.5f, rotToPlayer + (i * 10), 3.25f, 135, 128, 40);
                        Projectile p = new Projectile(Assets.GetTexFromSource("projectilesFull", 8, 0), new Color(77, 58, 84), 4.5f, position, Vector2.Zero, new Vector2(4, 8), rotToPlayer + (i * 10), 135, 3.25f, 128, 40);
                        world.CreateProjectile(p);

                        p = new Projectile(Assets.GetTexFromSource("projectilesFull", 0, 1), new Color(77, 58, 84), 4.5f, position, Vector2.Zero, new Vector2(4, 8), rotToPlayer + (i * 10), 135, 3.25f, 128, 40);
                        world.CreateProjectile(p);
                    }
                    prevcount = rand;
                    shot0 = 60;
                    frameConfiguration.SetFrame(1);
                }
            }
        }

        protected override void RunFrameConfiguration()
        {
            frameConfiguration.Update();
        }

        public override Entity Copy()
        {
            return new EnemyShade(position);
        }
    }
}
