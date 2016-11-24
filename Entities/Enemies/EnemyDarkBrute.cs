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
    public class EnemyDarkBrute : Enemy
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        private int shot0, shot1, prevcount;
        public EnemyDarkBrute(Vector2 position) : base(position)
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            name = "Dark Brute";
            texture = Assets.GetTexture("darkBrute");
            hitSound = Assets.GetSoundEffect("squeakImpact1");
            dieSound = Assets.GetSoundEffect("deathMonster1");

            speed = 2;
            health = 1;
            scale = 4;
            shadowScale = 4;
            flips = true;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(32)));
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
            }

            if (distFromPlayer >= 32)
            {
                if (!idleMove)
                {
                    moving = true;
                    Vector2 moveDirec = Vector2.Normalize(world.player.center - center);
                    Move(moveDirec, 5.2f);
                }
            }
            else
                SetIdle(10, 3);

            if (distFromPlayer < 192)
            {
                if (shot0 <= 0)
                {
                    FrameConfiguration conf = new FrameConfiguration(FrameConfiguration.FrameActionPreset3, null,
                        new FrameCollection(false,
                        new Frame(5, new Rectangle(0, 0, 8, 8)),
                        new Frame(5, new Rectangle(8, 0, 8, 8)),
                        new Frame(5, new Rectangle(16, 0, 8, 8)),
                        new Frame(5, new Rectangle(24, 0, 8, 8)),
                        new Frame(5, new Rectangle(32, 0, 8, 8)),
                        new Frame(5, new Rectangle(40, 0, 8, 8)),
                        new Frame(5, new Rectangle(48, 0, 8, 8)),
                        new Frame(5, new Rectangle(56, 0, 8, 8))));

                    Projectile p = new Projectile(conf, Assets.GetTexture("framesCard1"), Color.White, 4, center, Vector2.Zero, new Vector2(8, 4), rotToPlayer, -135, 5, 320, 1);
                        //new Projectile(Assets.GetTexFromSource("projectilesFull", 0, 0), new Color(77, 58, 84), position - new Vector2(16), new Vector2(8, 4), true, 4, rotToPlayer, 5, -45, 320, 120);
                    //.GiveBuff(new Buff("Paralyzed", 30, Buff.EffectParalyzed));
                    world.CreateProjectile(p);
                    /*for (float i = -7.5f; i <= 7.5f; i += 5)
                    {
                        p = new Projectile(Assets.GetTexFromSource("projectilesFull", 0, 1), new Color(77, 58, 84), 3, center, Vector2.Zero, new Vector2(8, 4), rotToPlayer + i, 180, 8, 320, 50);
                            //new Projectile(Assets.GetTexFromSource("projectilesFull", 0, 1), new Color(77, 58, 84), position - new Vector2(16), new Vector2(8, 4), true, 3, rotToPlayer + i, 8, 180, 320, 50);
                        world.CreateProjectile(p);
                    }*/
                    shot0 = 127;
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
            return new EnemyDarkBrute(position);
        }
    }
}
