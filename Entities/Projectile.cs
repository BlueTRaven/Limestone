/*using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Buffs;

namespace Limestone.Entities
{
    public class Projectile : Entity
    {
        int type;
        public float endDistance;

        Vector2 startPos;
        Vector2 endPos;
        public float timeleft = -1;
        public float fullTimeLeft = -1;

        public bool passesCover = false;

        //public override Vector2 center { get { return new Vector2(position.X + ((texture.Width * scale) / 2), position.Y + ((texture.Height * scale) / 2)) + wave; } set { position = new Vector2(value.X - ((texture.Width * scale) / 2), value.Y - ((texture.Height * scale) / 2)); } }
        public override Vector2 center
        {
            get
            {
                return new Vector2(position.X + ((texture.Width * scale) / 2), position.Y + ((texture.Height * scale) / 2)) + wave;
            }
            set
            {
                position = new Vector2(value.X - ((texture.Width * scale) / 2), value.Y - ((texture.Height * scale) / 2));
            }
        }
        public Vector2 wave;
        Vector2 perpVelocity { get { Vector2 norm = Vector2.Normalize(velocity); return  new Vector2(-norm.Y, norm.X); } }

        float rotationAngle;
        float angleoffset;

        public bool spin = false;
        private float spinspeed = 1;

        private bool rotateHitBox = true;

        public bool friendly = false;
        public bool piercing = false;
        public bool armorPiercing = false;

        private bool boomerang;
        private bool flipped = false;

        public int damage;

        public List<Entity> hitEntities = new List<Entity>();
        public List<Projectile> deathProjectiles = new List<Projectile>();
        public Enemy creator;
        private Projectile parent;

        private Particle trailParticle;
        private int trailTimer, trailTimerMax, trailAmt;
        private Vector2 trailMinMax;
        private float trailBackVel;

        public List<Buff> givebuffs = new List<Buff>();

        private List<string> tags = new List<string>();

        private int elapsedTime = 0;

        private bool wavy = false;
        private float waveAmp = 4f, waveSpeed = 64; // adjust if needed
        private bool waveFlip = false;

        public Texture2D noColorTex = null; //Nocolortex is another texture overlaid on the other one, obviously without color. used for, say, arrows, since you don't want the shaft colored.

        /*public Projectile(Texture2D texture, Color color, Vector2 position, Vector2 widthHeight, bool spin, bool rotateHitBox, float spinspeed, float scale, float angle,
            float speed, float angleOffset, float distance, int damage, bool passesCover = false, bool boomerang = false, bool wavy = false, float waveAmp = 0, float waveSpeed = 0, bool waveFlip = false)
        { }
        #region constructors
        public Projectile(Texture2D texture, Color color, Vector2 position, Vector2 widthHeight, bool rotateHitBox, float scale, float angle,
            float speed, float angleOffset, float distance, int damage, bool passesCover = false)
        {
            this.tType = EntityType.NONE;
            this.texture = texture;
            this.color = color;
            this.hitbox = new RotateableRectangle(new Rectangle(Vector2.Zero.ToPoint(), widthHeight.ToPoint()));
            this.center = position;
            this.spin = spin;
            this.rotateHitBox = rotateHitBox;
            this.spinspeed = spinspeed;
            this.scale = scale;
            this.angle = angle;
            this.speed = speed;
            this.angleoffset = angleOffset;
            this.endDistance = distance;
            this.damage = damage;
            this.passesCover = passesCover;

            timeleft = (float)Math.Floor(endDistance / speed);
            fullTimeLeft = timeleft;

            Vector2 startVector = new Vector2(endDistance, 0);
            endPos = Vector2.Transform(startVector, Matrix.CreateRotationZ(MathHelper.ToRadians(angle + 180)));
            SetStartVel(endPos);

            if(!spin)
                rotationAngle = VectorHelper.FindAngleBetweenTwoPoints(Vector2.Zero, velocity);
            wave = Vector2.Zero;
        }

        public Projectile SetStartVel(Vector2 pos)
        {
            velocity = pos;
            velocity.Normalize();
            velocity *= new Vector2(speed);
            return this;
        }

        public Projectile SetParticleTrail(Particle particle, float backvel, int time, int amt, Vector2 minmax)
        {
            this.trailParticle = particle;
            this.trailBackVel = backvel;
            this.trailTimerMax = time;
            this.trailTimer = trailTimerMax;
            this.trailAmt = amt;
            this.trailMinMax = minmax;
            return this;
        }

        public Projectile SetWavy(float waveAmp, float waveSpeed, bool flip)
        {
            this.wavy = true;
            this.waveAmp = waveAmp;
            this.waveSpeed = waveSpeed;
            this.waveFlip = flip;
            return this;
        }
        public Projectile SetSpin(float spinspeed)
        {
            this.spin = true;
            this.spinspeed = spinspeed;
            return this;
        }
        public Projectile SetBoomerang()
        {
            this.boomerang = true;
            return this;
        }
        #endregion

        public override void Update(World world)
        {
            timeleft--;

            elapsedTime++;
            
            if (!spin)
                rotationAngle = VectorHelper.FindAngleBetweenTwoPoints(Vector2.Zero, velocity);
            else
                rotationAngle += spinspeed;

            if (!boomerang)
            {
                if (timeleft <= 0)
                {
                    Die(world);
                }
            }
            else
            {
                
                if (timeleft <= 0 && !flipped)
                {
                    velocity *= -1;
                    flipped = true;
                }
                else if (flipped)
                {
                    if (timeleft <= -fullTimeLeft)
                        Die(world);
                }
            }

            if (wavy)
            {
                wave = Vector2.Zero;

                float waveAngle = elapsedTime * 3.14f * waveSpeed; // adjust if needed
                
                if (!waveFlip)
                    wave = perpVelocity * (float)Math.Sin(waveAngle) * waveAmp;
                else
                    wave = -perpVelocity * (float)Math.Sin(waveAngle) * waveAmp;
            }
            DoVelocity();
            //center += velocity;

            hitbox.MoveTo(center);
            if (rotateHitBox)
                hitbox.RotateTo(center, -rotationAngle);

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

        private void DoVelocity()
        {
            center = new Vector2(position.X + ((texture.Width * scale) / 2), position.Y + ((texture.Height * scale) / 2)) + velocity;
        }

        public override void Die(World world)
        {//Called when the projectile dies
            
            foreach(Projectile p in deathProjectiles)
            {
                world.CreateProjectile(p.Copy(position, p.angle));
            }

            dead = true;
        }
        Vector2 epos, epos2;
        public Projectile SetPredictive(Player target)
        {
            /*float dist = Vector2.Distance(target.center, center);

            float time = dist / speed;

            float playerDist = target.speed * time;
            Vector2 normPred = Vector2.Normalize(target.predictVec);
            if (!(float.IsNaN(normPred.X) && float.IsNaN(normPred.Y)))
            {
                Vector2 shootpos = normPred * playerDist;
                epos = shootpos;

                SetStartVel(shootpos);
            }
            return this;
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            return; //projectiles have no outlines
            //DrawHelper.DrawOutline(batch, texture, position, DrawHelper.GetTextureOffset(texture), 1, MathHelper.ToRadians(rotationAngle - angleoffset), scale);
        }

        public override void Draw(SpriteBatch batch)
        {
            if (!dead)
            {
                batch.Draw(Assets.GetTexture("shadow"), center, null, new Color(0, 0, 0, 127), -Main.camera.Rotation, 
                    DrawHelper.GetTextureOffset(Vector2.Zero, new Vector2(64, 64)), scale / 16, 0, 0);
                Vector2 offset = Main.camera.up * 8;
                if (noColorTex != null)
                    batch.Draw(noColorTex, center + offset, null, Color.White, MathHelper.ToRadians(rotationAngle - angleoffset), DrawHelper.GetTextureOffset(texture), scale, 0, 0);
                batch.Draw(texture, center + offset, null, color, MathHelper.ToRadians(rotationAngle - angleoffset), DrawHelper.GetTextureOffset(texture), scale, 0, 0);
                
            }
            //hitbox.DebugDraw(batch);
        }

        public int DistanceToTimeLeft(float distance)
        {
            return (int)Math.Floor(distance / speed);
        }

        public static Projectile Create(Texture2D texture, Color color, Vector2 position, Vector2 widthHeight, /*bool spin, bool rotateHitBox, 
            /*float spinspeed, float scale, float angle, float speed, float angleOffset, float distance, int damage, bool passesCover = false)
        {
            Projectile p = new Projectile(texture, color, position, widthHeight, rotateHitBox, scale, angle, speed, angleOffset, distance, damage, passesCover);
            return p;
        }
        /*public static Projectile Create(int type, int damage, Vector2 position, float angle, float speed, float distance, float scale = 1, bool tileCollides = true, bool boomerang = false, Projectile parent = null)
        {
            Projectile p = new Projectile(type, damage, position, scale, angle, speed, distance, boomerang, tileCollides, parent);
            return p;
        }
        public static Projectile Create(int type, int damage, Vector2 position, float angle, float speed, float distance, bool wavy, bool waveFlip = false, float scale = 1, bool tileCollides = true, float waveAmp = 4f, float waveSpeed = 64, Projectile parent = null)
        {
            Projectile p = new Projectile(type, damage, position, scale, angle, speed, distance, waveAmp, waveSpeed, waveFlip, tileCollides, parent);
            p.wavy = true;
            return p;
        }

        public Projectile Copy(Vector2 position, float angle)
        {
            Projectile copy = new Projectile(texture, color, position, new Vector2(hitbox.width, hitbox.height), rotateHitBox, scale, angle,
                speed, angleoffset, endDistance, damage, passesCover);
            if (givebuffs != null)
                copy.givebuffs = givebuffs.ToList();//new Buff(givebuffs.name, givebuffs.maxDuration, givebuffs.function);
            copy.spin = spin;
            copy.spinspeed = spinspeed;

            copy.friendly = friendly;
            copy.noColorTex = noColorTex;
            copy.piercing = piercing;
            copy.armorPiercing = armorPiercing;

            copy.wavy = wavy;
            copy.waveAmp = waveAmp;
            copy.waveSpeed = waveSpeed;
            copy.waveFlip = waveFlip;

            copy.spin = spin;
            copy.spinspeed = spinspeed;

            copy.boomerang = boomerang;
            return copy;
        }

        public Projectile Copy(Vector2 position, float angle, float distance, float speed)
        {
            Projectile copy = new Projectile(texture, color, position, new Vector2(hitbox.width, hitbox.height), rotateHitBox, scale, angle,
                speed, angleoffset, distance, damage, passesCover);
            if (givebuffs != null)
                copy.givebuffs = givebuffs.ToList();
            copy.spin = spin;
            copy.spinspeed = spinspeed;

            copy.friendly = friendly;
            copy.noColorTex = noColorTex;
            copy.piercing = piercing;
            copy.armorPiercing = armorPiercing;

            copy.wavy = wavy;
            copy.waveAmp = waveAmp;
            copy.waveSpeed = waveSpeed;
            copy.waveFlip = waveFlip;

            copy.spin = spin;
            copy.spinspeed = spinspeed;

            copy.boomerang = boomerang;
            return copy;
        }
    }
}*/
