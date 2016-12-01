using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;

using Limestone.Interface;
using Limestone.Tiles;
using Limestone.Entities.Collectables;

namespace Limestone.Entities
{
    public class Player2 : EntityLiving
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        private float facingAngle;

        public float speedScale;
        public Vector2 gravityDirection;
        public Vector2 gravityVelocity;     //NOTE: USE *only* IN CARDINAL DIRECTIONS!!!

        private float healthBarWidth;


        private Projectile attackProjectile;
        private Vector2 initialPos, initialProjRotPos, projRotPos;
        private float attackProjTimer = 15, attackProjTimerMax = 15, initialAngle, attackProjCooldown = 0;

        private float regainTimer = 720, regainTimerMax = 720;
        private List<CollectableBlood> bloods = new List<CollectableBlood>();
        public int collectedBloods = 0;
        private bool canRecover = false;

        private bool recovering;
        private float recoveryRingRadius;
        private float recoveryRingMaxRadius = 256;
        private float recoveryRingPercent;
        private Color recoveryRingColorCurrent;

        private bool rolling = false;
        private float rollDuration;

        private float rollCooldown;

        private float velZ = 0;
        private const float zGravity = -1.5f;

        private float stamina, staminaCooldown; //staminacooldown is how long before stamina can regen.
        private const float maxStamina = 100;

        public Player2(Vector2 position) : base(position)
        {
            texture = Assets.GetTexture("notex");
            tType = EntityType.Player;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(32)));

            speedScale = 1;
            speed = 3.25f;

            maxHealth = 3;
            health = 3;

            scale = 4;
            shadowScale = 4;
            shadowTextureColor = new Color(Color.Black, 127);
            setSize = new Rectangle(0, 0, 8, 8);
        }

        public override void Update(World world)
        {
            base.Update(world);
            Control(world);

            staminaCooldown--;

            if (staminaCooldown <= 0)
                stamina += 2;

            if (stamina > 100)
                stamina = maxStamina;
            else if (stamina < 0)
                stamina = 0;

            velZ += zGravity;
            height += velZ;

            if (height < 0)
                height = 0;

            #region HP recover and attack code
            if (canRecover)
                regainTimer--;
            if (regainTimer > 0 && collectedBloods >= 3 && health < 3 && canRecover)
            {
                health++;
                canRecover = false;

                for (int i = 0; i < Main.rand.Next(5, 12); i++)
                    world.CreateParticle(new Particle(null, center, new Vector2((float)Main.rand.NextDouble(-5, 5), (float)Main.rand.NextDouble(-5, 5)), Color.Red, 4, 18));

                recovering = true;
            }

            if (regainTimer <= 0)
            {
                bloods.Clear();
                canRecover = false;
            }

            foreach (CollectableBlood blood in bloods.ToArray())
            {
                if (blood.dead)
                    bloods.Remove(blood);
            }

            if (recovering)
            {
                if (recoveryRingRadius > recoveryRingMaxRadius)
                    recovering = false;
                else
                {
                    recoveryRingRadius += 4;
                    recoveryRingPercent = recoveryRingRadius / recoveryRingMaxRadius;

                    recoveryRingColorCurrent = Color.Lerp(Color.Red, Color.Transparent, recoveryRingPercent);
                }
            }

            attackProjTimer--;
            if (attackProjectile == null || attackProjectile.dead)
                attackProjCooldown--;

            if (attackProjectile != null && !attackProjectile.dead)
            {
                attackProjectile.position = Vector2.Zero;

                float rotP = ((attackProjTimer / attackProjTimerMax) * -157.5f) + 157.5f;
                projRotPos = initialProjRotPos;
                projRotPos = Vector2.Transform(projRotPos, Matrix.CreateRotationZ(MathHelper.ToRadians(rotP)));

                attackProjectile.angle = -initialAngle - rotP;
                attackProjectile.position += position + projRotPos;

                if (regainTimer > 0)
                {
                    if (attackProjectile.hitEntities.Count > 0)
                    {
                        bool hasKilled = false, isboss = false;
                        bool hasHit = attackProjectile.hitEntities.Count > 0;

                        if (hasHit)
                        {
                            foreach (Enemy el in attackProjectile.hitEntities)
                            {
                                if (el.dead)
                                {
                                    hasKilled = true;

                                    if (el.quest)
                                        isboss = true;
                                    break;
                                }
                            }
                        }

                        if (hasKilled)
                        {
                            Main.SlowDown(SlowDownMode.Stop, isboss ? 30 : 5);
                            Main.camera.SetQuake(isboss ? 8 : 2, 5);
                            invulnTicks = 60;

                            bloods.ForEach(x => x.collectOverride = false);

                            if (health < maxHealth && regainTimer > 0)
                                canRecover = true;

                            attackProjectile.hitEntities.Clear();
                        }
                        else if (hasHit)
                        {
                            //Main.SlowDown(SlowDownMode.Stop, 3); //Not sure how to implement this properly
                        }
                    }
                }

                if (attackProjTimer <= 0 || rolling)
                    attackProjectile.Die(world);
            }
            #endregion
        }

        private void Control(World world)
        {
            gravityVelocity -= gravityDirection;

            if (!rolling)
            {
                velocity = Vector2.Zero;

                if (Main.keyboard.KeyPressedContinuous(Main.options.KEYMOVEUP))
                {
                    velocity += Main.camera.up;
                    moving = true;
                }

                if (Main.keyboard.KeyPressedContinuous(Main.options.KEYMOVELEFT))
                {
                    velocity += Main.camera.left;
                    moving = true;
                }

                if (Main.keyboard.KeyPressedContinuous(Main.options.KEYMOVEDOWN))
                {
                    velocity += Main.camera.down;
                    moving = true;
                }

                if (Main.keyboard.KeyPressedContinuous(Main.options.KEYMOVERIGHT))
                {
                    velocity += Main.camera.right;
                    moving = true;
                }

                facingAngle = VectorHelper.GetVectorAngle(velocity);

                velocity *= speed * speedScale;

                velocity -= gravityVelocity;

                rollCooldown--;
                if (Main.keyboard.KeyPressed(Keys.Space) && rollCooldown <= 0 && velocity != Vector2.Zero && stamina > 30)
                {
                    rolling = true;
                    rollDuration = 10;

                    velocity *= 4;

                    untargetTicks = 12;

                    attackProjCooldown = 45;

                    rollCooldown = 45;

                    velZ = (-zGravity / 2) * rollDuration;

                    UseStamina(30);
                }
            }
            else
            {
                rollDuration--;

                if (rollDuration <= 0)
                    rolling = false;

                if (rollDuration <= 2)
                    velocity *= .5f;
            }

            Move(velocity);

            if ((attackProjectile == null || attackProjectile.dead) && !rolling && attackProjCooldown <= 0)
            {
                if (Main.mouse.MouseKeyPressContinuous(Inp.MouseButton.Left) && Main.isActive && stamina > 10)
                {
                    //The sword projectile is just a projectile with a *really* low speed, so it lasts forever until I want to manually kill it.
                    Projectile p = new Projectile(Assets.GetTexFromSource("projectilesFull", 0, 0), Color.White, 4, position + new Vector2(-32, 0), Vector2.Zero, new Vector2(8, 32), 0, 45, .0001f, 16, 1);
                    world.CreateProjectile(p);
                    p.friendly = true;
                    p.piercing = true;
                    initialPos = p.position;

                    attackProjectile = p;

                    float angle = VectorHelper.GetAngleBetweenPoints(center, VectorHelper.ConvertScreenToWorldCoords(Main.mouse.currentState.Position.ToVector2())) - 78.75f;
                    initialAngle = angle;
                    initialProjRotPos = new Vector2(-48, 0);
                    initialProjRotPos = Vector2.Transform(initialProjRotPos, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
                    projRotPos = initialProjRotPos;

                    attackProjTimer = attackProjTimerMax;
                    attackProjCooldown = 7;

                    UseStamina(10);
                }
            }
        }

        private void UseStamina(float amt)
        {
            stamina -= amt;

            if (stamina < 0)
                stamina = 0;

            staminaCooldown = 60;
        }

        public override void OnTileCollide(World world, Tile tile)
        {
            if (velocity == Vector2.Zero)
                gravityVelocity = Vector2.Zero;
        }

        public override void TakeDamage(int amt, IDamageDealer source, World world)
        {
            if (!invulnerable)
            {
                Main.SlowDown(SlowDownMode.Stop, 15);
                invulnTicks = 120;
                SetFlash(FlashType.Solid, Color.Red, 2, 120);

                bloods.ForEach(x => x.Die(world));
                bloods.Clear();

                collectedBloods = 0;

                for (int i = 0; i < 3; i++)
                {
                    CollectableBlood c = new CollectableBlood(this, center, (float)Main.rand.NextDouble(0, 360), Main.rand.Next(16, 128), 3);
                    world.CreateCollectable(c);
                    bloods.Add(c);
                }

                if (!canRecover)
                {
                    regainTimer = regainTimerMax;
                    canRecover = true;
                }

                int numparticles = Main.rand.Next(3, 8);

                if (world.particles.Count + numparticles < 32)
                {
                    for (int i = 0; i <= numparticles; i++)
                    {
                        Color c = texture.GetPixels().GetPixel(Main.rand.Next(0, texture.Width), Main.rand.Next(0, texture.Height), 1);
                        if (c != Color.Transparent)
                        {
                            float randVelX = (float)Main.rand.NextDouble(-2, 2);

                            float randVelY = (float)Main.rand.NextDouble(-2, 2);

                            world.CreateParticle(new Particle(null, hitbox.center + (Main.camera.down * 4), new Vector2(randVelX, randVelY), c, 4, Main.rand.Next(12, 21)));
                        }
                    }
                }

                if (health - amt > 0)
                {
                    Main.camera.SetQuake(8, 15);
                    if (hitSound != null) hitSound.Play();

                    health -= amt;
                }
                else if (!dead)
                {
                    //dieSound.Play();
                    Die(world);
                }
            }
        }

        protected override void RunFrameConfiguration()
        {
            frameConfiguration.Update();
        }

        public override void Die(World world)
        {
            //dead = true;
        }

        public void DrawHealthBar(SpriteBatch batch)
        {
            DrawGeometry.DrawRectangle(batch, new Rectangle(0, 0, 48 * 3, 26), Color.Black);

            DrawGeometry.DrawRectangle(batch, new Rectangle(0, 1, 48 * 3, 24), Color.Gray);

            for (int i = 0; i < 3; i++)
                DrawGeometry.DrawLine(batch, new Vector2(48 * i, 1), new Vector2(48 * i, 24), Color.Black);

            for (int i = 0; i < health; i++)
            {
                batch.Draw(Assets.GetTexture("healthBarChunk"), new Rectangle(48 * i, 1, 48, 24), Color.White);

                if (health < 3 && canRecover && i == health - 1)
                {
                    DrawGeometry.DrawRectangle(batch, new Rectangle(48 * i + 48, 1, 48, 24), new Color(Color.Red, 63));
                }
            }

            DrawGeometry.DrawRectangle(batch, new Rectangle(0, 25, 144, 18), Color.Black);
            DrawGeometry.DrawRectangle(batch, new Rectangle(1, 26, 142, 16), Color.Gray);
            float staminap = stamina / maxStamina;

            for (int i = 0; i < Math.Floor(143f * staminap); i++)
            {
                batch.Draw(Assets.GetTexture("staminaBarSegment"), new Vector2(i, 26), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                //DrawGeometry.DrawRectangle(batch, new Rectangle(0, 64, (int)stamina, 16), Color.Green);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (frameConfiguration.currentFrame == null)
                frameConfiguration.currentFrame = new Frame(-1, new Rectangle(0, 0, 8, 8));

            batch.Draw(shadowTexture, position, null, shadowTextureColor, -Main.camera.Rotation, ShadowOffset(), shadowScale / 8, 0, 0);

            Vector2 offset = Main.camera.up * ((shadowTexture.Height * scale) / 32) + Main.camera.up * height;
            Vector2 flipOffset = Main.camera.right * (frameConfiguration.currentFrame.size.Width - setSize.Width) * scale + Main.camera.down * (frameConfiguration.currentFrame.size.Height - setSize.Height) * scale;
            batch.Draw(texture, position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, color, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);

            foreach (DamageText dt in texts)
                dt.Draw(batch);

            if (Main.options.DEBUGDRAWNPCHITBOXES)
                hitbox.DebugDraw(batch);

            if (recovering)
                DrawGeometry.DrawCircle(batch, center, recoveryRingRadius, recoveryRingColorCurrent, 2, 32);
        }

        public override void DrawOutline(SpriteBatch batch)
        {
        }

        public override Entity Copy()
        {
            throw new NotImplementedException();
        }
    }
}
