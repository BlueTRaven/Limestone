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
    public class EnemySwampSlime : Enemy
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        private int shot0, shot1, prevcount;
        public EnemySwampSlime(Vector2 position) : base(position)
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            name = "Swamp Slime";
            texture = Assets.GetTexture("swampSlime");
            hitSound = Assets.GetSoundEffect("squeakImpact1");
            dieSound = Assets.GetSoundEffect("deathMonster1");

            health = 2000;
            scale = 4;
            shadowScale = 4;
            flips = false;

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

            if (distFromPlayer >= 96)
            {
                if (!idleMove)
                {
                    moving = true;
                    Vector2 moveDirec = Vector2.Normalize(world.player.center - center);
                    Move(moveDirec, 2.5f);
                }
            }
            else
                SetIdle(10, 3);

            if (distFromPlayer < 192)
            {
                if (shot0 <= 0)
                {
                    for (float i = -10; i <= 10; i += 10)
                    {
                        Projectile p = new Projectile(Assets.GetTexFromSource("projectilesFull", 10, 0), Color.White, 3, position, Vector2.Zero, new Vector2(8, 4), rotToPlayer + i, 0, 8, 128, 25);
                            //new Projectile(Assets.GetTexFromSource("projectilesFull", 10, 0), Color.White, position - new Vector2(16), new Vector2(8, 4), true, 3, rotToPlayer + i, 8, 0, 128, 25);
                        world.CreateProjectile(p);
                    }
                    shot0 = 35;
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
            return new EnemySwampSlime(position);
        }
    }
}
