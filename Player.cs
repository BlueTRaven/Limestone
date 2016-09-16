using System;
using System.Text;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Buff;

namespace Limestone
{
    public class Player : EntityLiving
    {
        public RotateableRectangle hitbox;

        private Rectangle sourceRect { get { return Assets.GetSourceRect(textureCoord, (int)textureSize.X); } set { } }
        public override Vector2 center { get { return new Vector2(position.X + ((texture.Width * 6) / 2), position.Y + ((texture.Height * 6) / 2)); } set { } }

        float maxSpeed;

        float healthWidth = 128;
        public int damage = 10;

        List<Buffs> buffs = new List<Buffs>();

        public Player(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;

            speed = 6;
            maxSpeed = 4;
            
            texture = Assets.GetTexture("char1");
            textureSize = new Vector2(8);
            textureCoord = new Vector2(0);

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint() - new Point(24), new Point(24)));

            health = 1000;
            maxHealth = 1000;
        }

        public override void Update(World world)
        {
            if (health > maxHealth)
                health = maxHealth;

            bool keyW = Main.KeyPressContinuous(Keys.W), keyA = Main.KeyPressContinuous(Keys.A), keyS = Main.KeyPressContinuous(Keys.S), keyD = Main.KeyPressContinuous(Keys.D);
            if (keyD)
            {
                position += Main.camera.right * speed;
                textureCoord = Vector2.Zero;
                flip = false;
            }

            if (keyA)
            {
                position += Main.camera.left * speed;
                textureCoord = Vector2.Zero;
                flip = true;
            }

            if (keyW)
            {
                position += Main.camera.up * speed;
                textureCoord = new Vector2(0, 2);
                flip = false;
            }

            if (keyS)
            {
                position += Main.camera.down * speed;
                textureCoord = new Vector2(0, 1f);
                flip = false;
            }
            hitbox.MoveTo(center);

            if (Main.KeyPressContinuous(Keys.Q))
            {
                Main.camera.Rotation += MathHelper.ToRadians(1);
            }

            if (Main.KeyPressContinuous(Keys.E))
            {
                Main.camera.Rotation -= MathHelper.ToRadians(1);
            }

            if (Main.KeyPress(Keys.R))
            {
                Main.camera.Rotation = 0;
            }

            prevPos = position;

            for (int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i].player == null)
                    buffs[i].player = this; //There's gotta be a better way
                buffs[i].RunEffect();

                if (!buffs[i].active)
                    buffs.RemoveAt(i);
            }
        }

        public override void TakeDamage(int amt, Projectile source)//Todo add buffs/debuffs
        {
            if (health > 0)
            {
                health -= amt;
                healthWidth *= (float)health / (float)maxHealth;

                if (source.givebuff != null)
                {
                    buffs.Add(source.givebuff);
                }
            }
            else
            {
                healthWidth = 0;
                Logger.Log("Player has died", false);
                dead = true;
            }
        }

        public override void Die(World world)
        {
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            DrawHelper.DrawOutline(batch, texture, center - texture.Bounds.Center.ToVector2(), Assets.GetSourceRect(textureCoord, (int)textureSize.X), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), textureSize), 1, flip);
        }

        public override void Draw(SpriteBatch batch)
        {
            if (!flip)
                batch.Draw(texture, center, Assets.GetSourceRect(textureCoord, (int)textureSize.X), 
                Color.White, -Main.camera.Rotation, DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), textureSize), 5.5f, 0, 0);
            else
                batch.Draw(texture, center, Assets.GetSourceRect(textureCoord, (int)textureSize.X),
                Color.White, -Main.camera.Rotation, DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), textureSize), 5.5f, SpriteEffects.FlipHorizontally, 0);
            
            //DrawGeometry.DrawRectangle(batch, new Rectangle(VectorHelper.ConvertWorldToScreenCoords(Vector2.Zero).ToPoint(), 
            //    VectorHelper.ConvertWorldToScreenCoords(new Vector2(128, 32)).ToPoint()), Color.Red);
            hitbox.DebugDraw(batch);

            DrawHelper.StartDrawCameraSpace(batch);
            DrawGeometry.DrawRectangle(batch, new Rectangle(Vector2.Zero.ToPoint(), new Vector2(healthWidth, 32).ToPoint()), Color.Red);
            DrawHelper.StartDrawWorldSpace(batch);
        }
    }
}
