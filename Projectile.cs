using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Buff;

namespace Limestone
{
    public class Projectile : Entity
    {
        int type;

        float scale;

        float endDistance;

        Vector2 startPos;
        Vector2 endPos;
        int timeleft = -1;

        public override Vector2 center { get { return new Vector2(position.X + (texture.Width * (scale / 2)), position.Y + (texture.Height * (scale / 2))); } set { } }
        private Vector2 offset { get { return new Vector2(hitbox.width / 4, hitbox.height / 4); } }

        Vector2 perpVelocity;// { get { return Vector2.Normalize(new Vector2(-velocity.Y, velocity.X)); } }

        float rotationAngle;
        float angleoffset;

        bool spin = false;
        float spinspeed = 1;

        public RotateableRectangle hitbox;
        private bool rotateHitBox = true;

        public bool piercing = false;
        public bool hitPlayer = false;

        public int damage;

        private Projectile parent;

        public Buffs givebuff;

        public Projectile(int type, int damage, Vector2 position, float scale, float angle, float speed, int timeleft, Projectile parent = null)
        {
            this.parent = parent;
            this.type = type;

            this.damage = damage;

            this.position = position;
            this.angle = angle;
            this.speed = speed;
            this.timeleft = timeleft;

            this.parent = parent;

            this.scale = scale;

            SetDefaults();
        }

        public Projectile(int type, int damage, Vector2 position, float scale, float angle, float speed, float distance, Projectile parent = null)
        {
            this.parent = parent;
            this.type = type;

            this.damage = damage;

            this.angle = angle;

            this.position = position;
            startPos = position;
            endDistance = distance;
            this.speed = speed;
            this.scale = scale;

            SetDefaults();
        }

        public void SetDefaults()
        {
            angleoffset = 135;

            if (type == 0)
            {
                texture = Assets.GetTexture("bolt");
                hitbox = new RotateableRectangle(new Rectangle(position.ToPoint() - new Point(16, 32), new Point(16, 32)));
            }
            else if (type == 1)
            {
                texture = Assets.GetTexture("bluebolt");
                hitbox = new RotateableRectangle(new Rectangle(position.ToPoint() - new Point(16, 32), new Point(16, 32)));
                piercing = true;
                givebuff = new Buffs(0, 2);
                givebuff.function = givebuff.EffectWeakness;
            }
            else if (type == 2)
            {
                texture = Assets.GetTexture("shield");
                hitbox = new RotateableRectangle(new Rectangle(position.ToPoint() - new Point(32, 16), new Point(32, 16)));
            }
            else if (type == 3)
            {
                texture = Assets.GetTexture("bolt");
                counter.Add("live", 0);
            }
            else if (type == 4)
            {
                texture = Assets.GetTexture("gear");
                spin = true;
                spinspeed = 8;
                angleoffset = 0;

                timeleft = 64;

                hitbox = new RotateableRectangle(new Rectangle(position.ToPoint() - new Point(16), new Point(32)));
                rotateHitBox = false;
            }
            else if (type == 5)
            {
                texture = Assets.GetTexture("tearshot");
                angleoffset = 180;
            }
            else if (type == 6)
            {
                texture = Assets.GetTexture("tearshot");
                angleoffset = 180;

                color = Color.Red;
            }

            Vector2 startVector = new Vector2(endDistance, 0);
            endPos = Vector2.Transform(startVector, Matrix.CreateRotationZ(MathHelper.ToRadians(angle + 180))) + this.position;

            velocity = endPos - this.position;
            velocity.Normalize();
            perpVelocity = new Vector2(-velocity.Y, velocity.X);
            velocity *= new Vector2(speed);

            if (parent != null)
                color = parent.color;
        }

        public override void Update(World world)
        {
            position += velocity;

            if (!spin)
                rotationAngle = VectorHelper.FindAngleBetweenTwoPoints(Vector2.Zero, velocity);
            else
                rotationAngle += spinspeed;

            if (timeleft == -1) //check to see if we're using timeleft method or length method; checks timeleft first because int check is faster than computing lengths and stuff
            {
                if ((position - startPos).Length() >= (endPos - startPos).Length())
                {
                    Die(world);
                }
            }
            else
            {
                if (timeleft <= 0)
                {
                    Die(world);
                }
                else timeleft--;
            }
            hitbox.MoveTo(center);
            if (rotateHitBox)
                hitbox.RotateTo(center, -rotationAngle);
        }

        public override void Die(World world)
        {//Called when the projectile dies
            if (type == 5)
            {
                if (parent == null)
                {
                    world.CreateProjectile(5, (int)Math.Floor(damage * 1.02f), this.position, this.angle, this.speed, 128, scale / 1.4f, this);
                    world.CreateProjectile(5, (int)Math.Floor(damage * 1.02f), this.position, this.angle - 15, this.speed, 128, scale / 1.4f, this);
                    world.CreateProjectile(5, (int)Math.Floor(damage * 1.02f), this.position, this.angle + 15, this.speed, 128, scale / 1.4f, this);
                }
            }

            dead = true;
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            DrawHelper.DrawOutline(batch, texture, position, DrawHelper.GetTextureOffset(texture), 1, MathHelper.ToRadians(rotationAngle - angleoffset), scale);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, center, null, color, MathHelper.ToRadians(rotationAngle - angleoffset), DrawHelper.GetTextureOffset(texture), scale, 0, 0);

            //DrawGeometry.DrawLine(batch, center, endPos - offset, Color.Blue);
            hitbox.DebugDraw(batch);
        }

        public int DistanceToTimeLeft(float distance)
        {
            return (int)Math.Floor(distance / speed);
        }
    }
}
