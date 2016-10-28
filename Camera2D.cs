using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone
{
    public class Camera2D
    {
        private readonly Viewport _viewport;

        public Camera2D(Viewport viewport)
        {
            _viewport = viewport;

            Rotation = 0;
            Zoom = 1;
            Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            Position = Vector2.Zero;
        }

        public float oldCameraRotation;

        public Vector2 Position { get; set; }
        public Vector2 prevPosition { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }
        public Vector2 Origin { get; set; }

        public bool moving { get { return Position != prevPosition; } set { } }

        public Vector2 center { get { return Origin + Position; } set { Position = value - Origin; } }

        public Vector2 down
        {
            get
            {
                Vector2 finalvec = new Vector2(0, 1);
                Vector2 f = Vector2.Transform(finalvec, Matrix.CreateRotationZ(-Rotation));
                f.Normalize();
                return f;
            }
        }
        public Vector2 right
        {
            get
            {
                Vector2 finalvec = new Vector2(1, 0);
                Vector2 f = Vector2.Transform(finalvec, Matrix.CreateRotationZ(-Rotation));
                f.Normalize();
                return f;
            }
        }
        public Vector2 up
        {
            get
            {
                Vector2 finalvec = new Vector2(0, -1);
                Vector2 f = Vector2.Transform(finalvec, Matrix.CreateRotationZ(-Rotation));
                f.Normalize();
                return f;
            }
        }
        public Vector2 left
        {
            get
            {
                Vector2 finalvec = new Vector2(-1, 0);
                Vector2 f = Vector2.Transform(finalvec, Matrix.CreateRotationZ(-Rotation));
                f.Normalize();
                return f;
            }
        }

        public void AddRot(float amt)
        {
            Rotation = MathHelper.ToRadians(BindTo360((MathHelper.ToDegrees(Rotation) + amt)));
        }

        private float BindTo360(float val)
        {
            if (val > 360)
            {
                val -= 360;
                
                return val;
            }
            else if (val <= 0)
            {
                val += 360;
                
                return val;
            }
            return val;
        }

        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        public Matrix GetInverseViewMatrix()
        {
            return Matrix.Invert(GetViewMatrix());
        }

        private bool fading, fadeTo;
        private float fadeTimer, fadeTimerMax;
        private Color currentColor, fadeColor;
        /// <summary>
        /// Fades from transparent to a color or vice versa.
        /// </summary>
        /// <param name="color">The color to fade to or from.</param>
        /// <param name="fadeTo">fade to color (true) or from color (false).</param>
        public void SetFade(Color color, bool fadeTo, int time)
        {
            fadeColor = color;
            this.fadeTo = fadeTo;
            fadeTimer = time;
            fadeTimerMax = time;

            fading = true;
        }

        private bool quaking;
        private float quakeRange, quakeDuration;
        public void SetQuake(float range, int duration)
        {
            quakeRange = range;
            quakeDuration = duration;

            quaking = true;
        }

        public void Update()
        {
            if (quaking)
            {
                if (quakeDuration > 0)
                {
                    quakeDuration--;

                    Position += new Vector2((float)Main.rand.NextDouble(quakeRange, -quakeRange), (float)Main.rand.NextDouble(quakeRange, -quakeRange));
                }
                else quaking = false;
            }
            if (fadeTimer > 0)
            {
                fadeTimer--;

                float fadep = fadeTimer / fadeTimerMax;
                if (fadeTo)
                    currentColor = Color.Lerp(Color.Transparent, fadeColor, fadep);
                else
                    currentColor = Color.Lerp(fadeColor, Color.Transparent, fadep);
            }

            if (fadeTimer <= 0)
                fading = false;
        }

        public void Draw(World world, SpriteBatch batch)
        {
            foreach (Enemy e in world.enemies)
                e.DrawHealthBar(batch);

            DrawHelper.StartDrawCameraSpace(batch);
            world.player.DrawHealthBar(batch);

            if (world.player.drawInventory)
                world.player.DrawItemsContents(batch);
            foreach (Bag b in world.bags)
            {
                if (world.player.drawInventory)
                {
                    if (b.hitbox.Intersects(world.player.hitbox))
                        b.DrawBagContents(batch, world.player.inventoryRect);
                }
            }

            if (fading)
                DrawGeometry.DrawRectangle(batch, _viewport.Bounds, currentColor);

            if (world.player.dead)
            {
                batch.GraphicsDevice.Clear(Color.Black);

                SpriteFont font = Assets.GetFont("bitfontMunro12");
                string text = "YOU HAVE DIED!";
                Vector2 textSize = font.MeasureString(text);
                batch.DrawString(Assets.GetFont("bitfontMunro12"), text, center - textSize / 2, Color.White);
            }

            DrawHelper.StartDrawWorldSpace(batch);
        }
    }
}
