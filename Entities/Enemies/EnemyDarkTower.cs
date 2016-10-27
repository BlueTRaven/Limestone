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
    public class EnemyDarkTower : Enemy
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        private int shot0, angle, spawnRate;
        private bool activated, reversed, dying;
        public EnemyDarkTower(Vector2 position) : base()
        {
            this.position = position;
            SetDefaults();
        }

        private void SetDefaults()
        {
            name = "Dark Tower";
            texture = Assets.GetTexture("darkTower");
            hitSound = Assets.GetSoundEffect("squeakImpact1");
            dieSound = Assets.GetSoundEffect("deathMonster1");

            xpGive = 650;
            health = 3500;
            scale = 4;
            shadowScale = 4;
            flips = false;
            height = 16;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(32)));
            activeDistance = 1024;

            lootItem = new LootItem(0);

            setSize = new Rectangle(0, 0, 16, 16);
            frameCollections.Add(new FrameCollection(false, new Frame(15, new Rectangle(0, 0, 16, 16))));

            frameCollections.Add(new FrameCollection(true, new Frame(15, new Rectangle(0, 0, 0, 0))));

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

            if (health < maxHealth && !activated && health > maxHealth / 10)
            {
                SetFlash(Color.Blue, 60, 60);
                activated = true;
            }
            if (distance < 512)
            {
                if (shot0 <= 0 && activated && flashTotalDuration <= 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 1, 0), Color.White, 4, position, Vector2.Zero, new Vector2(8), angle + i * 90, 0, 5, 512, 120);
                            //new Projectile(Assets.GetTexFromSource("projectilesFull", 1, 0), Color.White, position - new Vector2(16), new Vector2(8, 8), true, 4, angle + i * 90, 5, 0, 512, 120);
                        world.CreateProjectile(p);
                    }

                    if (!reversed)
                    {
                        angle += 10;

                        if (angle > 180)
                            reversed = true;
                    }
                    else
                    {
                        angle -= 10;

                        if (angle <= 0)
                        {
                            activated = false;
                            reversed = false;
                        }
                    }
                    shot0 = 10;
                }

                if (health <= maxHealth / 2)
                {
                    spawnRate--;

                    if (spawnRate <= 0)
                    {
                        spawnRate = 360;

                        world.CreateEnemy(new EnemyDarkBrute(center));
                    }
                }

                if (health <= maxHealth / 10)
                {
                    if (!dying)
                        SetFlash(Color.Red, 30, 120);

                    invulnTicks = 120;

                    dying = true;
                    if (flashTotalDuration <= 0)
                    {
                        for (float i = 0; i < 360f; i += 360f / 25f)
                        {
                            Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 0, 1), new Color(77, 58, 84), 4, position, Vector2.Zero, new Vector2(8, 4), i, 180, 5, 512, 50).SetWavy(16, .03f, false)
                                //new Projectile(Assets.GetTexFromSource("projectilesFull", 0, 1), new Color(77, 58, 84), position - new Vector2(16), new Vector2(8, 4), true, 4, i, 5, 180, 512, 50).SetWavy(16, .03f, false)
                                .SetParticleTrail(new Particle(Vector2.Zero, Vector2.Zero, new Color(77, 58, 84), 4, 10), 1.5f, 3, 1, new Vector2(-.5f, .5f));
                            world.CreateProjectile(p);
                        }
                        Die(world);
                    }
                }
            }
        }
    }
}
