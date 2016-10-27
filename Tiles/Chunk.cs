using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;

namespace Limestone.Tiles
{
    public class Chunk
    {
        public static readonly int size = 256;

        public Tile[,] tiles = new Tile[8, 8];

        public Coordinate position;
        public Rectangle bounds { get { return new Rectangle(position.ToPoint(), new Point(size)); } set { } }

        public Chunk(Coordinate position)
        {
            this.position = position;
        }

        public void Update()
        {
            
        }

        public void Draw(SpriteBatch batch)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[x, y] != null && tiles[x, y].texture != null)
                        tiles[x, y].Draw(batch);
                }
            }

            //DrawGeometry.DrawHollowRectangle(batch, bounds, 8, Color.Red);
        }
    }
}
