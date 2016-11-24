using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using Limestone.Utility;
using Limestone.Items;
using Limestone.Entities;

using Limestone.Interface;
using Limestone.Tiles;

namespace Limestone.Entities
{
    public class Bomb : Entity, IDamageDealer
    {
        private int elapsedTime;

        public readonly float distance;
        public float timeleft, maxTimeLeft;

        public override Vector2 center { get { return hitbox.center; } set { } }

        private Vector2 endPosition;

        private Vector2 size, offset;
        private float angleoffset;

        public bool spin;
        private float spinSpeed;

        private Particle trailParticle;
        private int trailTimer, trailTimerMax, trailAmt;
        private Vector2 trailMinMax;
        private float trailBackVel;

        public float radius;
        public int explosionTimeLeft;
        public int damageTime, damageTimeMax;

        private readonly float moveSpeed;

        public float damage;
        public bool armorPiercing;

        private float height;

        private float velZ, gravity = -.009f;

        private bool draw = true;
        /// <summary>
        /// Creates a new Bomb instance.
        /// </summary>
        /// <param name="texture">The texture to use. If null, uses whitePixel.</param>
        /// <param name="color">The color to draw. If whitepixel, draws as that color; otherwise is additive over the texture.</param>
        /// <param name="scale">how big.</param>
        /// <param name="position">the creation position.</param>
        /// <param name="offset">the offset of the texture from the origin (top left) of the hitbox.</param>
        /// <param name="radius">how large the 'explosion' should be.</param>
        /// <param name="explosionTime">how long the explosion lasts.</param>
        /// <param name="damageTime">how long inbetween 'waves' of damage.</param>
        /// <param name="angle">the angle to lob the bomb at.</param>
        /// <param name="visualAngleOffset">the visual angle offset of the bomb; mainly for textures, as they are not billboarded.</param>
        /// <param name="speed">how fast the bomb should travel.</param>
        /// <param name="distance">how far away the bomb should travel. Converted to timeleft; = distance / speed.</param>
        /// <param name="damage">How much damage each 'wave' of damage does.</param>
        public Bomb(Texture2D texture, Color color, float scale, Vector2 position, Vector2 offset, float radius, int explosionTime, int damageTime, float angle, float visualAngleOffset, float speed, float distance, float damage) : base(position)
        {
            this.tType = EntityType.Bomb;

            if (texture != null)
                this.texture = texture;
            else
                this.texture = Assets.GetTexture("whitePixel");

            this.color = color;
            this.scale = scale;
            this.offset = offset;

            this.radius = radius;
            this.explosionTimeLeft = explosionTime;
            this.damageTime = damageTime;
            this.damageTimeMax = damageTime;

            this.moveSpeed = speed;

            this.angle = angle;
            this.angleoffset = visualAngleOffset;

            this.damage = damage;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(16)), position, angle);
            hitbox.MoveTo(position - offset);
            hitbox.RotateTo(position, angle);

            this.distance = distance;

            velocity = Vector2.Normalize(Vector2.Transform(new Vector2(-1, 0), Matrix.CreateRotationZ(MathHelper.ToRadians(-angle)))) * speed;

            maxTimeLeft = distance / speed;
            timeleft = maxTimeLeft;

            endPosition = position + velocity * timeleft;

            velZ = (-gravity / 2) * timeleft;
        }

        public override void Update(World world)
        {
            timeleft--;
            elapsedTime++;

            velZ += gravity;
            height += velZ;

            position += velocity;
            hitbox.MoveTo(position - offset);
            hitbox.RotateTo(position, angle);

            if (timeleft <= 0)
            {
                draw = false;
                Die(world);
            }

            if (height < 0)
                height = 0;
        }

        public override void Die(World world)
        {
            float distanceFromPlayer = (center - world.player.center).Length();
            
            explosionTimeLeft--;
            damageTime--;

            if (damageTime <= 0)
            {
                if (distanceFromPlayer < radius)
                {
                    world.player.TakeDamage((int)damage, this, world);
                }
                damageTime = damageTimeMax;

                float offset = Main.rand.Next(360);
                for (float i = 0; i < 360f; i += 360f / 16)
                {
                    float t = radius / 32;
                    Particle p = new Particle(null, center - new Vector2(1), Vector2.Transform(new Vector2(32, 0), Matrix.CreateRotationZ(MathHelper.ToRadians(i + offset))), color, 2, (int)t);
                    world.CreateParticle(p);
                }

                offset = Main.rand.Next(360);
                for (float i = 0; i < 360f; i += 360f / 16)
                {
                    float t = radius / 16;
                    Particle p = new Particle(null, center - new Vector2(2), Vector2.Transform(new Vector2(16, 0), Matrix.CreateRotationZ(MathHelper.ToRadians(i + offset))), color, 4, (int)t);
                    world.CreateParticle(p);
                }

                offset = Main.rand.Next(360);
                for (float i = 0; i < 360f; i += 360f / 16)
                {
                    float t = radius / 8;
                    Particle p = new Particle(null, center - new Vector2(3), Vector2.Transform(new Vector2(8, 0), Matrix.CreateRotationZ(MathHelper.ToRadians(i + offset))), color, 6, (int)t);
                    world.CreateParticle(p);
                }
            }

            if (explosionTimeLeft <= 0)
                dead = true;
        }

        public override void OnTileCollide(World world, Tile tile)
        {
            if (tileCollides)
                Die(world);
        }


        float innerRadius;
        public override void Draw(SpriteBatch batch)
        {
            DrawGeometry.DrawCircle(batch, endPosition, radius, color, 2, (int)(radius / 1.5f));

            float lerpRadius = (timeleft / maxTimeLeft) * radius;

            DrawGeometry.DrawCircle(batch, endPosition, lerpRadius, color, 1, (int)(radius / 1.5f));

            if (!dead && draw)
            {
                batch.Draw(Assets.GetTexture("shadow"), position, null, new Color(0, 0, 0, 127), -Main.camera.Rotation,
                    DrawHelper.GetTextureOffset(Vector2.Zero, new Vector2(64, 64)), scale / 16, 0, 0);

                Vector2 offset = (Main.camera.up * 8) * height;
                batch.Draw(texture, position + offset, null, color, MathHelper.ToRadians(-angle + angleoffset), texture.Bounds.Size.ToVector2() / 2, scale, 0, 0);

                if (Options.DEBUGDRAWPROJECTILEHITBOXES)
                    hitbox.DebugDraw(batch);
            }
        }

        protected override void RunFrameConfiguration()
        {
            frameConfiguration.Update();
        }

        public override void DrawOutline(SpriteBatch batch)
        {   //NOOP
        }

        public float GetDamage()
        {
            return damage;
        }

        public bool GetArmorPiercing()
        {
            return armorPiercing;
        }

        public override Entity Copy()
        {
            return new Bomb(texture, color, scale, position, offset, radius, explosionTimeLeft, damageTime, angle, 0, speed, distance, damage);
        }
    }
}
