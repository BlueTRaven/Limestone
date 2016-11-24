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

using Limestone.Interface;

namespace Limestone.Entities
{
    public abstract class Enemy : EntityLiving
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        public string name;

        protected int alive;

        protected bool idleMove = false;
        protected Vector2 idleDirection;
        protected int idleTimer, idleTimerMax;
        protected float idleSpeed;

        protected float distFromPlayer, rotToPlayer;

        public bool quest;

        protected Vector2 healthBarDefault = Vector2.Zero;
        private Vector2 healthBarPos = Vector2.Zero;
        float healthBarWidth = 48;

        public List<Enemy> children = new List<Enemy>();
        public Enemy parent;

        public Enemy(Vector2 position) : base(position)
        {
            texture = Assets.GetTexture("notex");
            tType = EntityType.Enemy;

            healthBarDefault = new Vector2(-24, 20);

            shadowTextureColor = new Color(0, 0, 0, 127);
            baseColor = Color.White;
        }

        public override void Update(World world)
        {
            base.Update(world);

            foreach (Enemy c in children.ToArray())
                if (c.dead) children.Remove(c);
            hitbox.MoveTo(position);

            speed = 1;
            alive++;

            rotToPlayer = VectorHelper.GetAngleBetweenPoints(center, world.player.center);
            distFromPlayer = (center - world.player.center).Length();

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
        }

        protected Enemy CreateChild(Enemy e)
        {
            e.parent = this;
            children.Add(e);
            return this;
        }

        public override void OnTileCollide(World world, Tile tile)
        {   //NOOP
        }

        public override void TakeDamage(int amt, IDamageDealer source, World world)
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

                            world.CreateParticle(new Particle(null, hitbox.center + (Main.camera.down * 4), new Vector2(randVelX, randVelY), c, 4, Main.rand.Next(12, 21)));
                        }
                    }
                }

                int damagePost = amt;

                if (source.GetArmorPiercing())
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
                    if (dieSound != null) dieSound.Play();
                    Die(world);
                }
            }
        }

        public override void Die(World world)
        {
            texts.Clear();

            //world.CreateBag(Bag.Create(center, lootItem.Roll()));
            //world.player.GiveXp(xpGive);
            dead = true;
        }

        protected override void RunFrameConfiguration()
        {
            frameConfiguration.Update();
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
            if (frameConfiguration.currentFrame == null)
                frameConfiguration.currentFrame = new Frame(0, setSize);

            Vector2 offset = Main.camera.up * ((shadowTexture.Height * scale) / 32) + Main.camera.up * height;
            Vector2 flipOffset = Main.camera.right * (frameConfiguration.currentFrame.size.Width - setSize.Width) * scale + Main.camera.down * (frameConfiguration.currentFrame.size.Height - setSize.Height) * scale;

            batch.Draw(texture, Main.camera.up + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.down + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);

            batch.Draw(texture, Main.camera.up + Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.up + Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.down + Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.down + Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
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

            if (Options.DEBUGDRAWENEMYHITBOXES)
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

        protected void GeometricShootPattern(World world, Vector2 startPoint, float startAngle, int sides, int numbetweenpoints, Projectile projectile)
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
                    float angle = VectorHelper.GetAngleBetweenPoints(Vector2.Zero, lerped);
                    float speed = dist / projectile.maxTimeLeft;
                    Projectile newProj = projectile.Copy(startPoint, angle, dist, speed);
                    world.CreateProjectile(newProj);
                }
            }
        }
    }
}
