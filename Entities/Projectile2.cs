using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Buffs;

namespace Limestone.Entities
{
    public class Projectile2 : Entity
    {
        private int elapsedTime;

        public Texture2D noColorTex;
        public List<Projectile2> deathProjectiles = new List<Projectile2>();

        public readonly float distance;
        public float timeleft, maxTimeLeft;

        public override Vector2 center { get { return Vector2.Zero; } set { } }
        private Vector2 perpVelocity { get { Vector2 norm = Vector2.Normalize(velocity); return new Vector2(-norm.Y, norm.X); } }

        private Vector2 size, offset;
        private float angleoffset;

        public bool spin;
        private float spinSpeed;

        private bool boomerang;
        private bool flipped = true;

        private Vector2 wave;
        private float waveAmp, waveSpeed;
        private bool wavy, waveFlip;

        private Particle trailParticle;
        private int trailTimer, trailTimerMax, trailAmt;
        private Vector2 trailMinMax;
        private float trailBackVel;

        public List<Buff> giveBuffs = new List<Buff>();

        public bool piercing; 

        public bool armorPiercing;
        public float damage;

        public bool friendly;

        public List<Entity> hitEntities = new List<Entity>(); 
        public Projectile2(Texture2D texture, Color color, float scale, Vector2 position, Vector2 offset, Vector2 size, float angle, float angleoffset, float speed, float distance, float damage)
        {
            this.tType = EntityType.Projectile;

            this.speed = speed;
            this.texture = texture;
            this.color = color;
            this.scale = scale;
            this.position = position;
            this.size = size;
            angle = -angle;
            this.angle = angle;
            this.angleoffset = angleoffset;
            this.offset = offset;
            this.damage = damage;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), size.ToPoint()), position, angle);
            hitbox.MoveTo(position - offset);
            hitbox.RotateTo(position, angle);

            this.distance = distance;

            velocity = Vector2.Normalize(Vector2.Transform(new Vector2(-1, 0), Matrix.CreateRotationZ(MathHelper.ToRadians(-angle)))) * speed;

            maxTimeLeft = distance / speed;
            timeleft = maxTimeLeft;
        }

        public override void Update(World world)
        {
            timeleft--;
            elapsedTime++;

            if (boomerang)
            {
                if (timeleft <= 0 && !flipped)
                {
                    flipped = true;
                    timeleft = maxTimeLeft;
                    velocity = -velocity;
                }
            }

            if (timeleft <= 0 && flipped)
                Die(world);

            if (spin)
                angle += spinSpeed;

            if (wavy)
            {
                wave = Vector2.Zero;

                float waveAngle = elapsedTime * 3.14f * waveSpeed; // adjust if needed

                if (!waveFlip)
                    wave = perpVelocity * (float)Math.Sin(waveAngle) * waveAmp;
                else
                    wave = -perpVelocity * (float)Math.Sin(waveAngle) * waveAmp;
            }

            position += velocity;
            hitbox.MoveTo((position - offset) + wave);
            hitbox.RotateTo(position + wave, angle);

            if (trailParticle != null)
            {
                trailTimer--;

                if (trailTimer <= 0)
                {
                    for (int i = 0; i < trailAmt; i++)
                        world.CreateParticle(trailParticle.Copy(center, (-Vector2.Normalize(-velocity) * trailBackVel) + (VectorHelper.GetPerp(velocity) * (float)Main.rand.NextDouble(trailMinMax.X, trailMinMax.Y)), trailParticle.color));
                    trailTimer = trailTimerMax;
                }
            }
        }

        public Projectile2 SetPredictive(Player target)
        {
            float dist = Vector2.Distance(target.center, center);

            float time = dist / speed;

            float playerDist = target.speed * time;
            Vector2 normPred = Vector2.Normalize(target.predictVec);
            if (!(float.IsNaN(normPred.X) && float.IsNaN(normPred.Y)))
            {   //check if it's a number, otherwise it just disappears
                Vector2 shootpos = normPred * playerDist;
                //epos = shootpos;

                velocity = Vector2.Normalize(shootpos) * speed;

                angle = -VectorHelper.FindAngleBetweenTwoPoints(center, velocity);
            }
            return this;
        }

        public Projectile2 SetWavy(float waveAmp, float waveSpeed, bool flip)
        {
            this.wavy = true;
            this.waveAmp = waveAmp;
            this.waveSpeed = waveSpeed;
            this.waveFlip = flip;
            return this;
        }

        public Projectile2 SetSpin(float spinSpeed)
        {
            this.spin = true;
            this.spinSpeed = spinSpeed;
            return this;
        }
        public Projectile2 SetBoomerang()
        {
            this.boomerang = true;
            this.flipped = false;
            return this;
        }

        public override void Die(World world)
        {
            dead = true;

            foreach (Projectile2 p in deathProjectiles)
                world.testproj.Add(p.Copy(center, p.angle));
        }

        public Projectile2 SetParticleTrail(Particle particle, float backvel, int time, int amt, Vector2 minmax)
        {
            this.trailParticle = particle;
            this.trailBackVel = backvel;
            this.trailTimerMax = time;
            this.trailTimer = trailTimerMax;
            this.trailAmt = amt;
            this.trailMinMax = minmax;
            return this;
        }

        public static bool DEBUGDRAWPROJECTILEHITBOXES = false;
        public override void Draw(SpriteBatch batch)
        {
            if (!dead)
            {
                batch.Draw(Assets.GetTexture("shadow"), position + wave, null, new Color(0, 0, 0, 127), -Main.camera.Rotation,
                    DrawHelper.GetTextureOffset(Vector2.Zero, new Vector2(64, 64)), scale / 16, 0, 0);

                Vector2 offset = Main.camera.up * 8;
                if (noColorTex != null)
                    batch.Draw(noColorTex, position + offset + wave, null, color, MathHelper.ToRadians(-angle + angleoffset), texture.Bounds.Size.ToVector2() / 2, scale, 0, 0);
                batch.Draw(texture, position + offset + wave, null, color, MathHelper.ToRadians(-angle + angleoffset), texture.Bounds.Size.ToVector2() / 2, scale, 0, 0);

                if (DEBUGDRAWPROJECTILEHITBOXES)
                hitbox.DebugDraw(batch);
            }
        }

        public override void DrawOutline(SpriteBatch batch)
        {   //NOOP
        }

        public Projectile2 GiveBuff(params Buff[] buffs)
        {
            giveBuffs.AddRange(buffs);
            return this;
        }

        public Projectile2 Copy(Vector2 position, float angle)
        {
            Projectile2 copy = new Projectile2(texture, color, scale, position, offset, size, angle, angleoffset, speed, distance, damage);
            if (giveBuffs != null)
                copy.giveBuffs = giveBuffs.ToList();
            copy.spin = spin;
            copy.spinSpeed = spinSpeed;

            copy.friendly = friendly;
            copy.noColorTex = noColorTex;
            copy.piercing = piercing;
            copy.armorPiercing = armorPiercing;

            copy.wavy = wavy;
            copy.waveAmp = waveAmp;
            copy.waveSpeed = waveSpeed;
            copy.waveFlip = waveFlip;

            copy.boomerang = boomerang;
            return copy;
        }

        public Projectile2 Copy(Vector2 position, float angle, float distance, float speed)
        {
            Projectile2 copy = new Projectile2(texture, color, scale, position, offset, size, angle, angleoffset, speed, distance, damage);
            if (giveBuffs != null)
                copy.giveBuffs = giveBuffs.ToList();
            copy.spin = spin;
            copy.spinSpeed = spinSpeed;

            copy.friendly = friendly;
            copy.noColorTex = noColorTex;
            copy.piercing = piercing;
            copy.armorPiercing = armorPiercing;

            copy.wavy = wavy;
            copy.waveAmp = waveAmp;
            copy.waveSpeed = waveSpeed;
            copy.waveFlip = waveFlip;

            copy.boomerang = boomerang;
            return copy;
        }
    }
}
