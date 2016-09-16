using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;

namespace Limestone
{
    public class Line
    {
        public Vector2 start, end, normal;

        public Line(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;

            this.normal = VectorHelper.GetNormal(start, end);
        }

        public void Draw(SpriteBatch batch)
        {
            DrawGeometry.DrawLine(batch, start, end, Color.Red);

            Vector2 normalDraw = normal * 16;
            Vector2 midpoint = (end - start) / 2;
            DrawGeometry.DrawLine(batch, start + midpoint, start + midpoint + normalDraw, Color.AliceBlue);
            DrawGeometry.DrawLine(batch, normal * 32, Vector2.Zero, Color.AliceBlue);
        }
    }
}
