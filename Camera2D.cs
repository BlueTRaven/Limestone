using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;

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

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }
        public Vector2 Origin { get; set; }

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

        public void Update()
        {
            
        }
    }
}
