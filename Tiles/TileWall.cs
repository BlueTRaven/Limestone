using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Tiles
{
    public class TileWall : TileCollidable
    {
        Tile[] cardinalTiles;
        public bool setCardinals = false;

        Texture2D wallTexture;

        public TileWall(Coordinate position, Texture2D texture, Texture2D wallTexture, Biomes biome, bool drawOutline = true, bool canCreateRandomSpawner = true)
        {
            this.position = position;
            bounds = new Rectangle(position.ToPoint(), new Point(Coordinate.coordSize, Coordinate.coordSize));

            this.texture = texture;
            this.wallTexture = wallTexture;

            this.location = biome;
            this.drawOutline = drawOutline;
            this.canCreateRandomSpawner = canCreateRandomSpawner;
        }

        public void SetCardinalTiles(World world)
        {
            cardinalTiles = new Tile[4];
            Coordinate c1 = new Coordinate(-1, 0);
            Coordinate c2 = new Coordinate(0, 1);
            Coordinate c3 = new Coordinate(1, 0);
            Coordinate c4 = new Coordinate(0, -1);
            if (world.GetTile(position + c1) != null)
                cardinalTiles[0] = world.GetTile(position + c1);
            if (world.GetTile(position + c2) != null)
                cardinalTiles[1] = world.GetTile(position + c2);
            if (world.GetTile(position + c3) != null)
                cardinalTiles[2] = world.GetTile(position + c3);
            if (world.GetTile(position + c4) != null)
                cardinalTiles[3] = world.GetTile(position + c4);
            setCardinals = true;
        }

        public override void OnCollide(Entity entity)
        {
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            if (setCardinals)
            {
                for (int i = 0; i < 32; i++)
                {
                    if (cardinalTiles[3] != null && !(cardinalTiles[3] is TileWall))
                        batch.Draw(wallTexture, bounds.Center.ToVector2() + Main.camera.up * i, new Rectangle(0, i / 4, 8, 1), Color.White, MathHelper.ToRadians(0), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
                    if (cardinalTiles[2] != null && !(cardinalTiles[2] is TileWall))
                        batch.Draw(wallTexture, bounds.Center.ToVector2() + Main.camera.up * i, new Rectangle(0, i / 4, 8, 1), Color.White, MathHelper.ToRadians(90), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
                    if (cardinalTiles[1] != null && !(cardinalTiles[1] is TileWall))
                        batch.Draw(wallTexture, bounds.Center.ToVector2() + Main.camera.up * i, new Rectangle(0, i / 4, 8, 1), Color.White, MathHelper.ToRadians(180), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
                    if (cardinalTiles[0] != null && !(cardinalTiles[0] is TileWall))
                        batch.Draw(wallTexture, bounds.Center.ToVector2() + Main.camera.up * i, new Rectangle(0, i / 4, 8, 1), Color.White, MathHelper.ToRadians(270), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
                }
            }
                //batch.Draw(Assets.GetTexture("test"), bounds.Center.ToVector2() + Main.camera.up * (i - 1), new Rectangle(i * 8, 0, 8, 8), Color.White, 0, DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
        }

        public override void Draw(SpriteBatch batch)
        {
            
            batch.Draw(texture, bounds.Center.ToVector2() + Main.camera.up * 32, null, Color.White, 0, DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
        }

        #region creation
        public static TileWall Create(Coordinate position, Texture2D texture, Texture2D wallTexture, Biomes biome, bool billboarded = false, bool drawoutline = true, bool beneathtexture = false)
        {
            TileWall tW = new TileWall(position, texture, wallTexture, biome, drawoutline);
            //tW.location = biome;
            return tW;
        }

        /// <summary>
        /// Copies a tile to a position.
        /// </summary>
        /// <param name="position">The position to copy to.</param>
        /// <returns>A new instance of an identecal tile.</returns>
        public override Tile Copy(Coordinate position)
        {
            TileWall copy = new TileWall(position, texture, wallTexture, location, drawOutline);
            copy.canCreateRandomSpawner = canCreateRandomSpawner;
            //copy.position = position;
            //copy.bounds = new Rectangle(new Point(position.x * Coordinate.coordSize, position.y * Coordinate.coordSize), new Point(Coordinate.coordSize, Coordinate.coordSize));
            return copy;
        }
        #endregion
    }
}
