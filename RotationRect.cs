using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;

namespace Limestone
{
    public class RotationRect
    {
        public List<Line> lines = new List<Line>(); //Starting at top, goes clockwise

        public RotationRect(Vector2 pos1, Vector2 pos2, float angle)
        {
            //lines.Add(new Line(pos1, new Vector2(pos2.X, pos1.Y)));
            lines.Add(new Line(new Vector2(pos2.X, pos1.Y), pos2));
            //lines.Add(new Line(pos2, new Vector2(pos1.X, pos2.Y)));
            //lines.Add(new Line(new Vector2(pos1.X, pos2.Y), pos1));
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (Line l in lines)
                l.Draw(batch);
        }
    }
}
