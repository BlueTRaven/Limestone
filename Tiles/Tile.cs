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
        Floor,
        None = -1
    }

    public abstract class Tile
    {
        private Coordinate _position;
        public Coordinate position { get { return _position; } set { _position = value; bounds = new Rectangle(value.ToPoint(), new Point(Coordinate.coordSize)); } }
        public Vector2 realPosition;

        public Rectangle bounds;
        public int rotation;
        
        public Texture2D texture;
        public Color color;

        public bool border;

        protected Color miniMapColor;

        [JsonProperty]
        public bool revealed = false;
        protected bool canCreateRandomSpawner = true;

        [JsonProperty]
        public TileType tileType;
        public bool collidable = false;    //readonly - is only set in the TileCollidable constructor.

        public Tile[] adjacentTiles;
        public bool setCardinals = false;

        public Tile adjacentTileLeft { get { return adjacentTiles[0]; } set { } }
        public Tile adjacentTileDown { get { return adjacentTiles[1]; } set { } }
        public Tile adjacentTileRight { get { return adjacentTiles[2]; } set { } }
        public Tile adjacentTileUp { get { return adjacentTiles[3]; } set { } }

        [JsonConstructor]
        public Tile(Vector2 realPosition)
        {
            this.position = realPosition.ToCoordinate();
        }

        public Tile(Coordinate position)
        {
            this.position = position;
        }

        protected TextureInfo texInfo;

        protected bool billboarded;
        protected Texture2D billboardTexture;
        protected float billboardScale;
        public Tile SetBillboarded(Texture2D billboardTexture, float scale)
        {
            billboarded = true;
            this.billboardTexture = billboardTexture;
            this.billboardScale = scale;
            return this;
        }

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

        public void SetCardinalTiles(World world)
        {
            adjacentTiles = new Tile[4];
            Coordinate c1 = new Coordinate(-1, 0);
            Coordinate c2 = new Coordinate(0, 1);
            Coordinate c3 = new Coordinate(1, 0);
            Coordinate c4 = new Coordinate(0, -1);
            if (world.GetTile(position + c1) != null)   //left
                adjacentTiles[0] = world.GetTile(position + c1);
            if (world.GetTile(position + c2) != null)   //down
                adjacentTiles[1] = world.GetTile(position + c2);
            if (world.GetTile(position + c3) != null)   //right
                adjacentTiles[2] = world.GetTile(position + c3);
            if (world.GetTile(position + c4) != null)   //up
                adjacentTiles[3] = world.GetTile(position + c4);
            setCardinals = true;
        }

        public Color MinimapColor()
        {
            return miniMapColor;
        }

        protected void RandomRot()
        {
            Random r = new Random();
            int num = (int)(r.NextDouble() * 4);

            rotation = 90 * num;
        }
    }
}
