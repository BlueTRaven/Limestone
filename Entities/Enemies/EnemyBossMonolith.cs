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
    public class EnemyBossMonolith : Enemy
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        private bool start = false;
        private int phase = 0;
        private int shot0 = 60, shot0duration = 90, shot1 = 60, shot2 = 60, shot3 = 60, shot4 = 60, shot5 = 60, prevhp;
        private int deathTimer = 300;
        private int spawnerCounter = /*3*/1200;
        private float angle;

        private float tempHeight;
        public EnemyBossMonolith(Vector2 position) : base()
        {
            this.position = position;
            SetDefaults();
        }

        private void SetDefaults()
        {
            name = "Monolith of Babylon";
            texture = Assets.GetTexture("darkTower");
            hitSound = Assets.GetSoundEffect("woodImpact2");
            dieSound = Assets.GetSoundEffect("woodImpact1");

            xpGive = 2000;
            health = 30000;
            //defense = 45;
            scale = 12;
            shadowScale = 12;
            flips = false;
            height = 48;
            //height = -144;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(32)));
            activeDistance = 1024;

            lootItem = new LootItem(0);

            setSize = new Rectangle(0, 0, 16, 16);
            frameCollections.Add(new FrameCollection(false, new Frame(15, new Rectangle(0, 0, 16, 0))));

            frameCollections.Add(new FrameCollection(true, new Frame(15, new Rectangle(0, 0, 0, 0))));

            frameCollections[0].active = true;

            frameBehavior = WalkShoot;

            maxHealth = health;
            healthBarDefault = new Vector2(-24, 48);
            flip = true;
        }

        public override void Update(World world)
        {
            base.Update(world);

            if (distance < 256 && !start)
            {
                SetFlash(Color.Black, 60, 360);
                invulnTicks = 360;
                start = true;
            }

            if (currentFrame.size.Height < 16 && start)
            {
                Main.camera.SetQuake(4, 1);
                tempHeight += (16f / 360f);
                currentFrame.size.Height = (int)tempHeight;

                if (alive % 2 == 0)
                    world.CreateParticle(new Particle(center + new Vector2((float)Main.rand.NextDouble(-16, 16), (float)Main.rand.NextDouble(-16, 16)), new Vector2((float)Main.rand.NextDouble(-4, 4), (float)Main.rand.NextDouble(-4, 4)), Color.Gray, (float)Main.rand.NextDouble(4f, 8.5f), 15));
            }

            if (start && flashTotalDuration <= 0)
            {
                if (!stunned)
                {
                    shot0--;
                    shot1--;
                    shot2--;
                    shot3--;
                    shot4--;
                    shot5--;
                }

                if (shot0duration > 0)
                    shot0duration--;

                bool shoot0 = false;
                bool shoot1 = false;
                bool shoot2 = false;
                bool shoot3 = false;
                bool shoot4 = false;
                bool shoot5 = false;
                if (health < maxHealth && health >= maxHealth - 2500)//30000-27500 2500
                {
                    //shoot5 = true;
                    shoot2 = true;
                    if (phase == 0)
                    {
                        phase++;
                        SetFlash(new Color(92, 192, 0), 15, 90);
                        invulnTicks = 90;
                    }
                }
                else if (health < maxHealth - 2500 && health >= maxHealth - 5000)//27500-25000 2500
                {
                    shoot1 = true;
                    shoot2 = true;
                    shoot3 = true;
                    //shoot5 = true;
                    if (phase == 1)
                    {
                        phase++;
                        SetFlash(new Color(92, 192, 0), 15, 90);
                        invulnTicks = 90;
                    }
                }
                else if (health < maxHealth - 5000 && health >= maxHealth - 10000)//25000-20000 5000
                {
                    shoot1 = true;
                    shoot2 = true;
                    shoot3 = true;
                    shoot4 = true;
                    shoot5 = true;
                    if (phase == 2)
                    {
                        phase++;
                        SetFlash(new Color(92, 192, 0), 15, 90);
                        invulnTicks = 90;
                    }
                }
                else if (health < maxHealth - 10000 && health >= maxHealth - 20000)//20000-10000 10000
                {
                    shoot0 = true;
                    shoot1 = true;
                    shoot2 = true;
                    shoot3 = true;
                    shoot4 = true;
                    shoot5 = true;
                    if (phase == 3)
                    {
                        phase++;
                        SetFlash(new Color(92, 192, 0), 15, 90);
                        invulnTicks = 90;
                    }
                }
                else if (health < maxHealth - 20000 && health >= 500)//10000 - 500
                {
                    shoot2 = true;
                    shoot3 = true;
                    shoot4 = true;
                    if (scale < 16)
                    {
                        scale += 0.0625f;
                        shadowScale += 0.0625f;
                        height += .25f;
                    }

                    if (phase == 4)
                    {
                        phase++;
                        SetFlash(Color.Red, 90, 90);
                        invulnTicks = 90;

                        CreateChild(world.CreateEnemy(new EnemyShadeEye(center, true)));
                        CreateChild(world.CreateEnemy(new EnemyShadeEye(center, true, 180)));
                        CreateChild(world.CreateEnemy(new EnemyShadeEye(center, false)));
                    }
                    else if (phase == 5)
                    {
                        defense = 60;
                        if (children.Count > 0)
                        {
                            invulnerable = true;
                            invulnOverride = true;
                        }
                        else
                        {
                            invulnerable = false;
                            invulnOverride = false;
                            shoot0 = true;
                            shoot1 = true;
                            shoot5 = true;

                            if (spawnerCounter > 0)
                                spawnerCounter--;
                            else
                            {
                                spawnerCounter = 1200;// 3600;
                                phase = 4;
                            }
                        }
                    }
                }
                else if (health < 500)
                {
                    invulnerable = true;
                    invulnOverride = true;
                    if (deathTimer > 0)
                    {
                        deathTimer--;
                        if (deathTimer <= 0)
                        {
                            shot0 = 0;
                            shot2 = 0;
                            shot3 = 0;
                            shot4 = 0;
                            shot5 = 0;
                        }

                        if (shot0 <= 0)
                        {
                            float numshots = Main.rand.Next(8, 12);
                            for (float i = 0; i < 360f; i += 360f / numshots)
                            {
                                Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 1, 0), Color.White, 4, center, Vector2.Zero, new Vector2(8), i, 180, 5, 512, 50);
                                //p.armorPiercing = true;
                                world.CreateProjectile(p);
                            }
                            shot0 = 60;
                        }
                        if (shot2 <= 0)
                        {
                            float numshots = Main.rand.Next(8, 12);
                            for (float i = 0; i < 360f; i += 360f / numshots)
                            {
                                Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 7, 1), new Color(255, 192, 0), 4, center, Vector2.Zero, new Vector2(8), i, Main.rand.Next(360), 8, 1024, 80)
                                        .SetSpin(-12)
                                        .GiveBuff(new Buff("Weak", 600, Buff.EffectWeakness));
                                    world.CreateProjectile(p);
                            }
                            shot2 = Main.rand.Next(45, 90);
                        }
                        if (shot3 <= 0)
                        {
                            float numshots = Main.rand.Next(8, 12);
                            for (float i = 0; i < 360f; i += 360f / numshots)
                            {
                                Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 7, 1), new Color(255, 14, 94), 4, center, Vector2.Zero, new Vector2(8), i, Main.rand.Next(360), 6, 1024, 60)
                                .SetSpin(12)
                                .GiveBuff(new Buff("Stunned", 240, Buff.EffectStunned));
                                world.CreateProjectile(p);
                            }
                            shot3 = Main.rand.Next(60, 80);
                        }
                        if (shot4 <= 0)
                        {
                            float numshots = Main.rand.Next(5, 7);
                            for (float i = 0; i < 360f; i += 360f / numshots)
                            {
                                Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 0, 1), Color.White, 4, center, Vector2.Zero, new Vector2(8, 16),  i, 180, 9.5f, 768, 140);
                                world.CreateProjectile(p);
                            }
                            shot4 = Main.rand.Next(60, 80);
                        }
                        if (shot5 <= 0)
                        {
                            float numshots = Main.rand.Next(5, 7);
                            for (float i = 0; i < 360f; i += 360f / numshots)
                            {
                                Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 8, 1), Color.White, 12, center, Vector2.Zero, new Vector2(24, 64), i, 225, 16, 1024, 400);
                                //.SetPredictive(world.player);
                                world.CreateProjectile(p);
                            }
                            shot5 = 60;
                        }
                    }
                    else Die(world);
                }

                if (flashTotalDuration <= 0)
                {
                    if (shot0 <= 0 && shoot0)
                    {
                        float numshots = Main.rand.Next(10, 20);
                        for (float i = 0; i <= 360f; i += 360f / numshots)
                        {
                            Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 1, 0), Color.White, 4, center, Vector2.Zero, new Vector2(8), i , 180, 5, 512, 50);
                            //p.armorPiercing = true;
                            world.CreateProjectile(p);
                        }
                        //angle += 24;
                        shot0 = 60;//12
                    }

                    if (shot1 <= 0 && shoot1)
                    {
                        for (float i = -80; i < 80; i += (80 / phase))
                        {
                            Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 2, 1), new Color(92, 192, 0), 8, center, Vector2.Zero, new Vector2(32), rotToPlayer + i, Main.rand.Next(360), 3.5f, 512, 250)
                                .SetSpin(-16)
                                .SetBoomerang()
                                .GiveBuff(new Buff("Slowed", 120, Buff.EffectSlowed));
                            world.CreateProjectile(p);
                        }
                        shot1 = Main.rand.Next(60, 240);
                    }

                    if (shot2 <= 0 && shoot2)
                    {
                        for (float i = -7.5f; i <= 7.5f; i += 5)
                        {
                            Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 7, 1), new Color(255, 192, 0), 4, center, Vector2.Zero, new Vector2(8), rotToPlayer + i, Main.rand.Next(360), 8, 1024, 80)
                                .SetSpin(-12)
                                .GiveBuff(new Buff("Weak", 600, Buff.EffectWeakness));
                            world.CreateProjectile(p);
                        }
                        shot2 = Main.rand.Next(45, 90);
                    }

                    if (shot3 <= 0 && shoot3)
                    {
                        for (float i = -3.75f; i <= 3.75f; i += 2.5f)
                        {
                            Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 7, 1), new Color(255, 14, 94), 4, center, Vector2.Zero, new Vector2(8), rotToPlayer + i, Main.rand.Next(360), 6, 1024, 60)
                                .SetSpin(12)
                                .GiveBuff(new Buff("Stunned", 240, Buff.EffectStunned));
                            world.CreateProjectile(p);
                        }
                        shot3 = Main.rand.Next(60, 80);
                    }

                    if (shot4 <= 0 && shoot4)
                    {
                        for (float i = -4; i <= 4; i += 4f)
                        {
                            Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 0, 1), Color.White, 4, center, Vector2.Zero, new Vector2(8, 16), rotToPlayer + i, 180, 9.5f, 768, 140);
                            world.CreateProjectile(p);
                        }
                        shot4 = Main.rand.Next(60, 80);
                    }

                    if (shot5 <= 0 && shoot5)
                    {
                        Projectile2 p = new Projectile2(Assets.GetTexFromSource("projectilesFull", 8, 1), Color.White, 12, center, Vector2.Zero, new Vector2(24, 64), rotToPlayer, 225, 16, 1024, 400);
                            //.SetPredictive(world.player);
                        world.CreateProjectile(p);

                        shot5 = 60;
                    }
                }
            }
            prevhp = health;
        }
    }
}
