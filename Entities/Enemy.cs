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

namespace Limestone.Entities
{
    public delegate void FrameBehavior();
    public abstract class Enemy : EntityLiving
    {
        protected Vector2 startLocation;

        public string name;

        protected FrameBehavior frameBehavior;
        protected List<FrameCollection> frameCollections = new List<FrameCollection>();
        protected Frame currentFrame;

        protected Rectangle setSize;

        public bool invulnerable = false;
        public bool invulnOverride = false;
        public bool untargetable = false;
        public bool untOverride = false;
        private int _invulnTicks;
        public int invulnTicks { get { return _invulnTicks; } set { _invulnTicks = value; invulnerable = true; } }
        private int _untargetTicks;
        public int untargetTicks { get { return _untargetTicks; } set { _untargetTicks = value; untargetable = true; } }

        public int xpGive;
        public float shadowScale;
        public LootItem lootItem;

        protected int alive;

        protected bool idleMove = false;
        protected Vector2 idleDirection;
        protected int idleTimer, idleTimerMax;
        protected float idleSpeed;

        protected Color baseColor;
        protected Color flashColor;
        protected float flashDuration, flashDurationMax;
        protected int flashTotalDuration = -1;

        protected float distance, rotToPlayer;

        protected bool flips;
        public bool quest;

        protected Vector2 healthBarDefault = Vector2.Zero;
        private Vector2 healthBarPos = Vector2.Zero;
        float healthBarWidth = 48;

        protected float height = 0;

        public bool moving = false;

        public List<Enemy> children = new List<Enemy>();
        public Enemy parent;

        public Enemy() : base()
        {
            tType = EntityType.Enemy;
            startLocation = position;

            healthBarDefault = new Vector2(-24, 20);

            shadowTextureColor = new Color(0, 0, 0, 127);
            baseColor = Color.White;
        }

        public abstract Enemy Copy();

        public override void Update(World world)
        {
            foreach (Enemy c in children.ToArray())
                if (c.dead) children.Remove(c);
            hitbox.MoveTo(position);

            speed = 1;
            alive++;

            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                buffs[i].RunEffect(world);

                if (!buffs[i].active)
                    buffs.RemoveAt(i--);
            }

            frameBehavior?.Invoke();

            rotToPlayer = VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center);
            distance = (center - world.player.center).Length();

            if (flashTotalDuration >= 0)
            {
                flashTotalDuration--;
                flashDuration--;

                if (flashDuration <= 0)
                    flashDuration = flashDurationMax;

                if (flashDuration <= flashDurationMax / 2)
                    color = Color.Lerp(baseColor, flashColor, flashDuration / flashDurationMax);
                else
                    color = Color.Lerp(flashColor, baseColor, flashDuration / flashDurationMax);
            }

            if (idleMove)
            {
                moving = true;
                idleTimer--;
                if (idleTimer <= 0)
                {
                    idleDirection = Vector2.Normalize(new Vector2((float)Main.rand.NextDouble(-2, 2), (float)Main.rand.NextDouble(-2, 2)));
                    idleTimer = idleTimerMax;
                    idleMove = false;
                }

                position += (idleDirection * idleSpeed) * speed;
                hitbox.MoveTo(position);
            }

            if (flips)
            {
                float angle = VectorHelper.FindAngleBetweenTwoPoints(center, world.player.center) + MathHelper.ToDegrees(Main.camera.Rotation);

                if (angle >= 360)
                    angle -= 360;
                if (angle < 0)
                    angle += 360;

                if (angle >= 90 && angle <= 270)
                    flip = false;
                else
                    flip = true;
            }

            if (invulnTicks > 0)
                invulnTicks--;
            if (invulnTicks <= 0 && !invulnOverride)
                invulnerable = false;

            if (untargetTicks > 0)
                untargetTicks--;
            if (untargetTicks <= 0 && !untOverride)
                untargetable = false;

            for (int i = texts.Count - 1; i >= 0; i--)
            {
                texts[i].center = center;
                texts[i].Update();
                if (texts[i].dead)
                    texts.RemoveAt(i--);
            }
        }

        protected Enemy CreateChild(Enemy e)
        {
            e.parent = this;
            children.Add(e);
            return this;
        }

        public override void TakeDamage(int amt, Projectile2 source, World world)
        {
            if (!invulnerable)
            {
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

                            world.CreateParticle(new Particle(hitbox.center + (Main.camera.down * 4), new Vector2(randVelX, randVelY), c, 4, Main.rand.Next(12, 21)));
                        }
                    }
                }

                if (source != null)
                {
                    if (source.giveBuffs != null)
                    {
                        bool foundBuff = false;
                        foreach (Buff gb in source.giveBuffs)
                        {
                            foreach (Buff b in buffs)
                            {
                                if (b.name == gb.name)
                                {
                                    if (b.active)
                                    {
                                        foundBuff = true;
                                        if (gb.duration > b.duration)
                                        {
                                            b.duration = gb.duration;  //Refresh the buff's duration
                                        }
                                        break;
                                    }
                                }
                            }
                            if (!foundBuff)
                            {
                                texts.Add(new DamageText(gb.name, center + new Vector2(0, -texture.Height), Color.Red));
                                gb.entity = this;
                                buffs.Add(gb);
                            }
                        }
                    }
                }
                int damagePost = CalculateDefenseResist(amt, source.armorPiercing);

                if (source.armorPiercing)
                    texts.Add(new DamageText(damagePost.ToString(), center + new Vector2(0, -texture.Height), Color.Purple));
                else
                    texts.Add(new DamageText(damagePost.ToString(), center + new Vector2(0, -texture.Height), Color.Red));
                if (health - damagePost > 0)
                {
                    if (hitSound != null) hitSound.Play();

                    health -= damagePost;
                    float healthP = (float)health / (float)maxHealth;
                    healthBarWidth = 48 * healthP;
                }
                else if (!dead)
                {
                    dieSound.Play();
                    Die(world);
                }
            }
        }

        public int CalculateDefenseResist(int amt, bool armorpeirce)
        {
            return armorpeirce ? amt : (int)MathHelper.Clamp((amt - ((defense * .65f) * 1.6f)), 0, 999);
        }

        public override void Die(World world)
        {
            texts.Clear();

            world.CreateBag(Bag.Create(center, lootItem.Roll()));
            world.player.GiveXp(xpGive);
            dead = true;
        }

        public void DrawHealthBar(SpriteBatch batch)
        {
            Color healthbarcolor = Color.Red;

            if (Main.camera.oldCameraRotation != Main.camera.Rotation || alive <= 20)
                healthBarPos = Vector2.Transform(healthBarDefault, Matrix.CreateRotationZ(-Main.camera.Rotation));
            if (!untargetable)
            {
                if (invulnerable)
                    healthbarcolor = Color.Black;
                batch.Draw(Assets.GetTexture("whitePixel"), center + healthBarPos, null, Color.Gray, -Main.camera.Rotation, Vector2.Zero, new Vector2(48, 8), SpriteEffects.None, 0);
                batch.Draw(Assets.GetTexture("whitePixel"), center + healthBarPos, null, healthbarcolor, -Main.camera.Rotation, Vector2.Zero, new Vector2(healthBarWidth, 8), SpriteEffects.None, 0);
            }
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            if (currentFrame == null)
                currentFrame = new Frame(-1, new Rectangle(0, 0, 8, 8));

            Vector2 offset = Main.camera.up * ((shadowTexture.Height * scale) / 32) + Main.camera.up * height;
            Vector2 flipOffset = Main.camera.right * (currentFrame.size.Width - setSize.Width) * scale + Main.camera.down * (currentFrame.size.Height - setSize.Height) * scale;

            batch.Draw(texture, Main.camera.up + position + offset - (flip ? flipOffset : Vector2.Zero), currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.down + position + offset - (flip ? flipOffset : Vector2.Zero), currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);

            batch.Draw(texture, Main.camera.up + Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.up + Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.down + Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.down + Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
        }
        public static bool DEBUGDRAWENEMYHITBOXES = false;
        public override void Draw(SpriteBatch batch)
        {
            if (currentFrame == null)
                currentFrame = new Frame(-1, new Rectangle(0, 0, 8, 8));

            batch.Draw(shadowTexture, position, null, shadowTextureColor, -Main.camera.Rotation, ShadowOffset(), shadowScale / 8, 0, 0);

            Vector2 offset = Main.camera.up * ((shadowTexture.Height * scale) / 32) + Main.camera.up * height;
            Vector2 flipOffset = Main.camera.right * (currentFrame.size.Width - setSize.Width) * scale + Main.camera.down * (currentFrame.size.Height - setSize.Height) * scale;
            batch.Draw(texture, position + offset - (flip ? flipOffset : Vector2.Zero), currentFrame.size, color, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);

            foreach (DamageText dt in texts)
                dt.Draw(batch);

            if (DEBUGDRAWENEMYHITBOXES)
                hitbox.DebugDraw(batch);
        }

        protected void Move(Vector2 moveDirec, float speed)
        {
            position += (moveDirec * speed) * this.speed;
            hitbox.MoveTo(position);
            idleMove = false;
        }

        protected void SetFlash(Color color, int flashDuration, int totalDuration, bool invuln = false)
        {
            flashColor = color;
            flashDurationMax = flashDuration;
            this.flashDuration = flashDuration;
            flashTotalDuration = totalDuration;
        }

        protected void SetIdle(int timer, float speed)
        {
            if (!idleMove)
            {
                idleMove = true;
                idleTimer = timer;
                idleTimerMax = timer;
                idleSpeed = speed;
            }
        }

        protected void SetFrame(int frameCollectionIndex)
        {
            foreach (FrameCollection fc in frameCollections)
                fc.SetInactive();

            frameCollections[frameCollectionIndex].active = true;
            currentFrame = frameCollections[frameCollectionIndex].currentFFrame;
        }

        protected Vector2 ShadowOffset()
        {
            return new Vector2(shadowTexture.Width / 2, shadowTexture.Height / 2);
        }
        protected Vector2 TextureOffset()
        {
            Vector2 final = new Vector2(setSize.Width / 2, setSize.Height / 2);//new Vector2(currentFrame.size.Size.ToVector2().X / 2, currentFrame.size.Size.ToVector2().Y / 2);
            return final;
        }

        protected void WalkShoot()
        {
            FrameCollection walkFrames = frameCollections[0];
            FrameCollection shootFrames = frameCollections[1];
            if (moving)
            {
                if (!shootFrames.active)
                {
                    walkFrames.Update();
                    currentFrame = walkFrames.currentFFrame;
                }
            }
            else
            {
                if (!shootFrames.active)
                currentFrame = walkFrames.frames[0];
            }

            if (shootFrames.active)
            {
                shootFrames.Update();
                currentFrame = shootFrames.currentFFrame;
            }
        }

        protected void GeometricShootPattern(World world, Vector2 startPoint, float startAngle, int sides, int numbetweenpoints, Projectile2 projectile)
        {
            List<Vector2> vertices = new List<Vector2>();
            for (int i = 0; i < sides; i++)
            {
                Vector2 start = new Vector2(0, -1);
                start = Vector2.Transform(start, Matrix.CreateRotationZ(MathHelper.ToRadians(startAngle)));
                start = Vector2.Transform(start, Matrix.CreateRotationZ(MathHelper.ToRadians((360 / sides) * i))) * projectile.distance;
                vertices.Add(start);
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 currentPoint = vertices[i];
                Vector2 nextPoint = Vector2.Zero;
                if (i == vertices.Count - 1)
                    nextPoint = vertices[0];
                else nextPoint = vertices[i + 1];

               // world.CreateProjectile(projectile.Copy(startPoint, VectorHelper.FindAngleBetweenTwoPoints(Vector2.Zero, currentPoint)));
                for (float j = 0; j < numbetweenpoints; j++)
                {
                    Vector2 lerped = Vector2.LerpPrecise(currentPoint, nextPoint, j / (float)numbetweenpoints);
                    float dist = lerped.Length();
                    float angle = VectorHelper.FindAngleBetweenTwoPoints(Vector2.Zero, lerped);
                    float speed = dist / projectile.maxTimeLeft;
                    Projectile2 newProj = projectile.Copy(startPoint, angle, dist, speed);
                    world.CreateProjectile(newProj);
                }
            }
        }
    }
}
