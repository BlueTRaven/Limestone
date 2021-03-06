﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;
using Limestone.Guis;

namespace Limestone
{
    /// <summary>
    /// The way the camera moves to the target position.
    /// Smooth: Smoothly ramps up speed, moving towards the target position until it's near enough, at which point it snaps to the target location.
    /// Fast: Same as smooth, but faster speed and no ramp up.  //NOTE BUGGY
    /// Snap: Simply snaps the camera to the target position.
    /// </summary>
    public enum CameraMoveMode
    {
        Smooth,
        Fast,
        Snap
    }

    public class GameCamera
    {
        public Gui activeGui;

        private readonly Viewport _viewport;

        private readonly CameraMoveMode defaultMoveMode;
        public GameCamera(Viewport viewport)
        {
            _viewport = viewport;

            Rotation = 0;
            Zoom = 1;
            Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            Position = Vector2.Zero;

            defaultMoveMode = CameraMoveMode.Smooth;
        }

        public CameraMoveMode moveMode;
        public CameraMoveMode tempMoveMode;
        public int moveModeDuration;

        public Vector2 target;
        private Vector2 prevTarget;
        public Vector2 velocity;

        public float oldCameraRotation;

        public Vector2 Position { get; set; }
        public Vector2 prevPosition { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }
        public Vector2 Origin { get; set; }

        public Rectangle clamp;

        public bool moving { get { return Position != prevPosition; } set { } }

        public Vector2 center { get { return new Vector2(_viewport.Width / 2, _viewport.Height / 2); } set { } }
        public Vector2 worldCenter { get { return Origin + Position; } set { Position = value - Origin; } }

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

        public void SetMoveMode(CameraMoveMode mode, int duration)
        {
            tempMoveMode = mode;
            moveModeDuration = duration;
        }

        public void Update()
        {
            if (moveModeDuration > 0)
                moveMode = tempMoveMode;

            if (moveMode == CameraMoveMode.Snap)
                worldCenter = target;
            else if (moveMode == CameraMoveMode.Fast)
            {
                Vector2 dist = target - worldCenter;

                if (dist != Vector2.Zero)
                    dist = Vector2.Normalize(dist);
                else dist = Vector2.Zero;

                //velocity += dist;   //if distance is not zero, return normalized dist, otherwie return vector2.zero
                worldCenter += dist * 4;
            }
            else if (moveMode == CameraMoveMode.Smooth)
                worldCenter = Vector2.Lerp(worldCenter, target, .10f);

            if (fadeTimer > 0)
            {
                fadeTimer--;

                float step = fadeTimer / fadeTimerMax;
                if (fadeTo)
                    currentColor = Color.Lerp(Color.Transparent, fadeColor, step);
                else
                    currentColor = Color.Lerp(fadeColor, Color.Transparent, step);
            }

            if (fadeTimer <= 0)
                fading = false;
        }

        public void PostUpdate(Main main)
        {
            if (activeGui == null)
                activeGui = new GuiMainMenu();

            if (quaking)
            {
                if (quakeDuration > 0)
                {
                    quakeDuration--;

                    Position += new Vector2((float)Main.rand.NextDouble(quakeRange, -quakeRange), (float)Main.rand.NextDouble(quakeRange, -quakeRange));
                }
                else quaking = false;
            }

            if (clamp != Rectangle.Empty)
            {
                Position = Vector2.Clamp(Position, Vector2.Zero, new Vector2(clamp.Width - _viewport.Width, clamp.Height - _viewport.Height));
            }

            activeGui.Update(main);
            prevPosition = Position;
            prevTarget = target;

            moveMode = defaultMoveMode;
        }

        public void Draw(Main main, SpriteBatch batch)
        {
            if (main.world != null)
                foreach (Enemy e in main.world.enemies)
                    e.DrawHealthBar(batch);

            if (Main.options.DEBUGDRAWCAMERACLAMP)
                DrawGeometry.DrawHollowRectangle(batch, clamp, 4, Color.Red);

            DrawHelper.StartDrawCameraSpace(batch);
            if (!Main.hold && main.world != null)
            {
                if (!main.world.mapLoadThread.IsAlive)
                {
                    if (main.world.player != null)
                    {
                        if (!activeGui.stopsWorldDraw)
                        {
                            main.world.player.DrawHealthBar(batch);

                            /*if (main.world.player.drawInventory)
                                main.world.player.DrawItemsContents(batch);
                            foreach (Bag b in main.world.bags)
                            {
                                if (main.world.player.drawInventory)
                                {
                                    if (b.hitbox.Intersects(main.world.player.hitbox))
                                        b.DrawBagContents(batch, main.world.player.inventoryRect);
                                }
                            }*/

                            if (main.world.player.dead)
                            {
                                batch.GraphicsDevice.Clear(Color.Black);

                                SpriteFont font = Assets.GetFont("bitfontMunro12");
                                string text = "YOU HAVE DIED!";
                                Vector2 textSize = font.MeasureString(text);
                                batch.DrawString(Assets.GetFont("bitfontMunro12"), text, worldCenter - textSize / 2, Color.White);
                            }
                        }
                    }
                }
            }

            activeGui.Draw(batch);

            if (fading)
                DrawGeometry.DrawRectangle(batch, _viewport.Bounds, currentColor);

            DrawHelper.StartDrawWorldSpace(batch);
        }
    }
}
