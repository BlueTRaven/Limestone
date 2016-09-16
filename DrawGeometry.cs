using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Limestone.Utility;
namespace Limestone
{
    public class DrawGeometry
    {
        public static void DrawRectangle(SpriteBatch batch, Rectangle area, Color color)
        {
            Texture2D whitePixel = Assets.GetTexture("whitePixel");

            batch.Draw(whitePixel, area, color);
        }

        public static void DrawHollowRectangle(SpriteBatch batch, Rectangle area, int width, Color color)
        {
            Texture2D whitePixel = Assets.GetTexture("whitePixel");

            batch.Draw(whitePixel, new Rectangle(area.X, area.Y, area.Width, width), color);
            batch.Draw(whitePixel, new Rectangle(area.X, area.Y, width, area.Height), color);
            batch.Draw(whitePixel, new Rectangle(area.X + area.Width - width, area.Y, width, area.Height), color);
            batch.Draw(whitePixel, new Rectangle(area.X, area.Y + area.Height - width, area.Width, width), color);
        }

        public static void DrawLine(SpriteBatch batch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Texture2D whitePixel = Assets.GetTexture("whitePixel");

            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            batch.Draw(whitePixel, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
