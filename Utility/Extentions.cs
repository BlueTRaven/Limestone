using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Buffs;

namespace Limestone.Utility
{
    public static class Extentions
    {
        public static double NextDouble(this Random rand, double minimum, double maximum)
        {
            return rand.NextDouble() * (maximum - minimum) + minimum;
        }
        
        public static Rectangle CreateRectangle(Vector2 location, Vector2 size)
        {
            return new Rectangle(location.ToPoint(), size.ToPoint());
        }

        public static Rectangle CreateRectangle(Vector2 location, int sizeX, int sizeY)
        {
            return new Rectangle((int)location.X, (int)location.Y, sizeX, sizeY);
        }

        public static Color GetPixel(this Color[] colors, int x, int y, int width)
        {
            return colors[x + (y * width)];
        }
        public static Color[] GetPixels(this Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colors1D);
            return colors1D;
        }
    }
}
