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
    public class EnemyShadeEye : Enemy
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        private int shot0, shot1;
        private float rotAngle;
        public bool rotating = false, low = false;
        public EnemyShadeEye(Vector2 position, bool rotating, float angle = 0) : base()
        {
            this.position = position;
            this.startLocation = position;
            this.rotating = rotating;
            this.rotAngle = angle;
            SetDefaults();

        }

        private void SetDefaults()
        {
            name = "Dark Brute";
            texture = Assets.GetTexture("shadeEye");
            hitSound = Assets.GetSoundEffect("squeakImpact1");
            dieSound = Assets.GetSoundEffect("deathMonster1");

            shot0 = Main.rand.Next(60); //to prevent stacked shots
            shot1 = Main.rand.Next(60);

            speed = 2;
            xpGive = 200;
            defense = 30;
            health = 7500;
            scale = 4;
            shadowScale = 4;
            flips = false;

            height = 32;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(32)));
            activeDistance = 5120;

            lootItem = new LootItem(0);

            setSize = new Rectangle(0, 0, 16, 16);
            frameCollections.Add(new FrameCollection(false,
                new Frame(5, new Rectangle(0, 0, 16, 16)),
                new Frame(5, new Rectangle(16, 0, 16, 16))));

            frameCollections.Add(new FrameCollection(true,
                new Frame(15, new Rectangle(32, 0, 16, 16))));

            frameCollections[0].active = true;

            frameBehavior = WalkShoot;

            maxHealth = health;

            //will always be moving
            moving = true;
        }

        public override void Update(World world)
        {
            base.Update(world);

            if (health <= maxHealth / 2 && !low)
            {
                low = true;
                shot0 = 0;
                shot1 = 0;
            }
            if (health <= maxHealth / 2 && flashTotalDuration <= 0)
                SetFlash(Color.IndianRed, 60, 60);

            if (!rotating)
            {
                if (distance >= 128)
                {
                    Vector2 moveDirec = Vector2.Normalize(world.player.center - center);
                    Move(moveDirec, 3.5f);
                }
            }
            else
            {
                Vector2 pos = new Vector2(-512, 0);
                pos = Vector2.Transform(pos, Matrix.CreateRotationZ(MathHelper.ToRadians(rotAngle += .5f)));

                Vector2 moveDirec = Vector2.Normalize((startLocation + pos) - center);
                Move(moveDirec, 2);
            }

            if (!stunned)
            {
                shot0--;
                shot1--;
            }

            if (shot0 <= 0)
            {
                SetFrame(1);
                Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 0, 0), new Color(95, 50, 50), 6, center, Vector2.Zero, new Vector2(12, 32), rotToPlayer, 45, 4, 512, 250)
                    .GiveBuff(new Buff("Paralyzed", 45, Buff.EffectParalyzed));
                world.CreateProjectile(p);
                shot0 = Main.rand.Next(100, 140);
            }

            if (shot1 <= 0)
            {
                for (float i = 0; i <= 360f; i += 360f / 16f)
                {
                    Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 9, 1), Color.White, 4, center, Vector2.Zero, new Vector2(16), i, 0, 1, 1024, 200);
                    world.CreateProjectile(p);
                }
                shot1 = Main.rand.Next(low ? 240 : 300,  low ? 330 : 420);
            }
        }

        public override Enemy Copy()
        {
            return new EnemyShadeEye(position, rotating, angle);
        }
    }
}
