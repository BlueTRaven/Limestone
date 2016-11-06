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
using Limestone.Buffs;

namespace Limestone.Entities.Enemies
{
    public class EnemyDarkBrute : Enemy
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        private int shot0, shot1, prevcount;
        public EnemyDarkBrute(Vector2 position) : base()
        {
            this.position = position;
            SetDefaults();
        }

        private void SetDefaults()
        {
            name = "Dark Brute";
            texture = Assets.GetTexture("darkBrute");
            hitSound = Assets.GetSoundEffect("squeakImpact1");
            dieSound = Assets.GetSoundEffect("deathMonster1");

            speed = 2;
            xpGive = 200;
            health = 1750;
            scale = 4;
            shadowScale = 4;
            flips = true;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(32)));
            activeDistance = 512;

            lootItem = new LootItem(0);

            setSize = new Rectangle(0, 0, 8, 8);
            frameCollections.Add(new FrameCollection(false,
                new Frame(15, new Rectangle(0, 0, 8, 8)),
                new Frame(15, new Rectangle(8, 0, 8, 8)),
                new Frame(15, new Rectangle(16, 0, 8, 8))));

            frameCollections.Add(new FrameCollection(true,
                new Frame(15, new Rectangle(40, 0, 16, 8)),
                new Frame(15, new Rectangle(32, 0, 8, 8))));

            frameCollections[0].active = true;

            frameBehavior = WalkShoot;

            maxHealth = health;
        }

        public override void Update(World world)
        {
            base.Update(world);

            if (!stunned)
            {
                shot0--;
            }

            if (distance >= 32)
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

            if (distance < 192)
            {
                if (shot0 <= 0)
                {
                    Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 0, 0), new Color(77, 58, 84), 4, center, Vector2.Zero, new Vector2(8, 4), rotToPlayer, -45, 5, 320, 120)
                        //new Projectile(Assets.GetTexFromSource("projectilesFull", 0, 0), new Color(77, 58, 84), position - new Vector2(16), new Vector2(8, 4), true, 4, rotToPlayer, 5, -45, 320, 120);
                    .GiveBuff(new Buff("Paralyzed", 30, Buff.EffectParalyzed));
                    world.CreateProjectile(p);
                    for (float i = -7.5f; i <= 7.5f; i += 5)
                    {
                        p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 0, 1), new Color(77, 58, 84), 3, center, Vector2.Zero, new Vector2(8, 4), rotToPlayer + i, 180, 8, 320, 50);
                            //new Projectile(Assets.GetTexFromSource("projectilesFull", 0, 1), new Color(77, 58, 84), position - new Vector2(16), new Vector2(8, 4), true, 3, rotToPlayer + i, 8, 180, 320, 50);
                        world.CreateProjectile(p);
                    }
                    shot0 = 60;
                    SetFrame(1);
                }
            }
        }

        public override Enemy Copy()
        {
            return new EnemyDarkBrute(position);
        }
    }
}
