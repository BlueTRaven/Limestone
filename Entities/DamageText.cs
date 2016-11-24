using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;


namespace Limestone.Entities
{
    public class DamageText
    {
        string number = "null";
        SpriteFont font;
        private Vector2 textSize { get { return font.MeasureString(number); }set { } }
        private Vector2 position;
        public Vector2 center { get { return new Vector2(position.X + (textSize.X / 2), position.Y + (textSize.X / 2)); } set { position = new Vector2(value.X - (textSize.X / 2), value.Y - (textSize.Y / 2)); } }
        private Vector2 distFromCenter;
        private float height = 64;

        public bool dead = false;
        private int timeLeft = 90;

        private Color color;

        public DamageText(string number, Vector2 position, Color color)
        {
            this.number = number;
            this.font = Assets.GetFont("munro12");
            this.center = position;
            this.color = color;
        }

        public void Update()
        {
            height += 1;
            timeLeft--;
            if (timeLeft <= 0)
                dead = true;
        }

        public void Draw(SpriteBatch batch)
        {
            distFromCenter = Vector2.Transform(new Vector2(textSize.X, height), Matrix.CreateRotationZ(-Main.camera.Rotation));
            DrawHelper.DrawOutline(batch, font, number, center - distFromCenter, Vector2.Zero, 1, -Main.camera.Rotation, 2.5f);
            batch.DrawString(font, number, center - distFromCenter, color, -Main.camera.Rotation, Vector2.Zero, 2.5f, SpriteEffects.None, 0);
        }
    }
}
