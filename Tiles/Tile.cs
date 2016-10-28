using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Tiles
{
    #region Coordinate
    public static class CoordinateHelper
    {
        public static Vector2 ToVector2Unscaled(this Coordinate coordinate)
        {
            return new Vector2(coordinate.x, coordinate.y);
        }
        public static Rectangle ToCoordinateScale(this Rectangle rectangle)
        {
            return new Rectangle(rectangle.X * Coordinate.coordSize, rectangle.Y * Coordinate.coordSize, rectangle.Width * Coordinate.coordSize, rectangle.Height * Coordinate.coordSize);
        }
        public static Coordinate ToCoordinate(this Vector2 vector)
        {
            return new Coordinate((int)Math.Floor(vector.X / Coordinate.coordSize), (int)Math.Floor(vector.Y / Coordinate.coordSize));
        }

        public static Coordinate ToCoordinate(this Point point)
        {
            return new Coordinate((int)Math.Floor((double)point.X / Coordinate.coordSize), (int)Math.Floor((double)point.Y / Coordinate.coordSize));
        }
    }
    public class Coordinate
    {
        public int x, y;

        public static readonly int coordSize = 48;
        public static readonly Coordinate Zero = new Coordinate(0, 0);

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Coordinate(Coordinate coord)
        {
            this.x = coord.x;
            this.y = coord.y;
        }

        public Coordinate(int size)
        {
            x = size;
            y = size;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x * coordSize, y * coordSize);
        }

        public Point ToPoint()
        {
            return new Point(x * coordSize, y * coordSize);
        }

        public static Coordinate VectorToCoord(Vector2 position)
        {
            return new Coordinate((int)Math.Floor(position.X / coordSize), (int)Math.Floor(position.Y / coordSize));
        }

        public static Vector2 ToVector2(Coordinate coord)
        {
            return new Vector2(coord.x * coordSize, coord.y * coordSize);
        }

        

        public static Coordinate operator +(Coordinate A, Coordinate B)
        {
            return new Coordinate(A.x + B.x, A.y + B.y);
        }

        public static Coordinate operator /(Coordinate A, int B)
        {
            return new Coordinate(A.x / B, A.y / B);
        }
    }
    #endregion

    public enum TileType
    {
        Wall,
        Floor
    }
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Tile
    {
        [JsonProperty]
        private Coordinate _position;
        public Coordinate position { get { return _position; } set { _position = value; bounds = new Rectangle(value.ToPoint(), new Point(Coordinate.coordSize)); } }
        public Rectangle bounds;
        public int rotation;

        [JsonProperty]
        public Biomes location;

        [JsonProperty]
        protected string textureName;
        protected string wallTextureName;
        public Texture2D texture;
        public Color color;

        public bool border;

        [JsonProperty]
        public bool revealed = false;
        protected bool canCreateRandomSpawner = true;

        [JsonProperty]
        public TileType tileType;
        public void OnReveal(World world)
        {
            if (canCreateRandomSpawner)
            {
                if (Main.rand.Next(200) == 0)
                {
                    if (!typeof(TileCollidable).IsSubclassOf(GetType()))
                    {
                        //world.CreateSpawner(new EnemySpawner(position.ToVector2(), Biome.GetSpawners(location)));
                    }
                }
            }
        }
        public abstract void Draw(SpriteBatch batch);
        public abstract Tile Copy(Coordinate position);
        public Color MinimapColor()
        {
            //if (!revealed)
                //return Color.Black;
            switch (location)
            {
                case Biomes.Beach:
                    return Color.SandyBrown;
                case Biomes.LowLands:
                    return Color.GreenYellow;
                case Biomes.MidLands:
                    return Color.Green;
                case Biomes.HighLands:
                    return Color.DarkGreen;
                case Biomes.AncientLands:
                    return Color.DarkSlateBlue;
                case Biomes.Sea:
                    return Color.CornflowerBlue;
                default:
                    return Color.Pink;
            }
            /*Color[] pixelcolor = new Color[1];
            Rectangle middle = new Rectangle(24, 24, 1, 1);
            texture.GetData<Color>(0, middle, pixelcolor, 0, 1);

            return pixelcolor[0];*/
        }

        protected void RandomRot()
        {
            Random r = new Random();
            int num = (int)(r.NextDouble() * 4);

            rotation = 90 * num;
        }
    }
}
