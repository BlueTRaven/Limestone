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
    public class EnemyWisp : Enemy
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        private bool activated;
        private int shot0, shotcount;
        public EnemyWisp(Vector2 position) : base()
        {
            this.position = position;
            SetDefaults();
        }

        private void SetDefaults()
        {
            name = "Wisp";
            texture = Assets.GetTexture("wisp");
            hitSound = Assets.GetSoundEffect("squeakImpact1");
            dieSound = Assets.GetSoundEffect("deathMonster1");
            shadowTextureColor = new Color(225, 225, 0, 127);

            speed = 2;
            xpGive = 650;
            health = 4500;
            scale = 4;
            shadowScale = 32;
            height = 64;
            flips = false;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(32)));
            activeDistance = 512;

            lootItem = new LootItem(0);

            setSize = new Rectangle(0, 0, 8, 8);
            frameCollections.Add(new FrameCollection(false,
                new Frame(30, new Rectangle(0, 0, 8, 8)),
                new Frame(30, new Rectangle(8, 0, 8, 8)),
                new Frame(30, new Rectangle(16, 0, 8, 8)), 
                new Frame(30, new Rectangle(24, 0, 8, 8))));

            frameCollections.Add(new FrameCollection(true,
                new Frame(5, new Rectangle(32, 0, 8, 8)), 
                new Frame(5, new Rectangle(40, 0, 8, 8)),
                new Frame(5, new Rectangle(48, 0, 8, 8))));

            frameCollections[0].active = true;

            frameBehavior = WalkShoot;

            maxHealth = health;

        }

        public override void Update(World world)
        {
            base.Update(world);

            shadowScale = height / 2;
            if (distance < 128)
            {
                if (flashTotalDuration <= 0 && !activated)
                    SetFlash(Color.Gold, 30, 90);

                if (!activated)
                    activated = true;
            }
            if (!activated)
            {
                if (distance >= 128)
                    SetIdle(15, 1);

                if (height <= 64)
                    height++;
            }

            if (flashTotalDuration <= 0 && activated)
            {
                if (height > 8)
                    height--;
                else
                {
                    if (!stunned)
                        shot0--;

                    if (shot0 <= 0)
                    {
                        shot0 = 60;
                        SetFrame(1);

                        for (float i = 0; i < 360; i += 360 / 25)
                        {
                            Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 8, 0), new Color(77, 58, 84), 4.5f, position, Vector2.Zero, new Vector2(4, 8), rotToPlayer + i, 135, 3.25f, 128, 40);
                            //new Projectile(Assets.GetTexFromSource("projectilesFull", 8, 0), new Color(77, 58, 84), position - new Vector2(16), new Vector2(4, 8), true, 4.5f, rotToPlayer + i, 3.25f, 135, 128, 40);
                            world.CreateProjectile(p);
                        }
                        shotcount++;
                    }
                    if (shot0 == 30)
                    {
                        for (float i = 0; i < 360; i += 360 / 15)
                        {
                            Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 11, 0), Color.White, 4, position, Vector2.Zero, new Vector2(8, 4), i, 135, 4.5f, 256, 25); 
                                //new Projectile(Assets.GetTexFromSource("projectilesFull", 11, 0), Color.White, position - new Vector2(16), new Vector2(8, 4), true, 4, i, 4.5f, 135, 256, 25);
                            world.CreateProjectile(p);
                        }
                    }

                    if (shotcount >= 3)
                    {
                        world.CreateEnemy(new EnemyShade(center));
                        activated = false;
                        shotcount = 0;
                        shot0 = 0;
                    }
                }
            }
        }
    }
}
