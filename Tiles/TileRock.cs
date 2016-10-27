﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Tiles
{
    public class TileRock : TileCollidable
    {
        protected Texture2D floorTex;
        public TileRock(Coordinate location, Texture2D rockTex, Texture2D floorTex, Biomes biome)
        {
            position = location;
            bounds = new Rectangle(position.ToPoint(), new Point(Coordinate.coordSize, Coordinate.coordSize));
            texture = rockTex;
            this.floorTex = floorTex;

            this.location = biome;
        }
        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, bounds.Center.ToVector2(), null, Color.White, -Main.camera.Rotation, DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            DrawHelper.DrawOutline(batch, texture, bounds.Location.ToVector2(), DrawHelper.GetTextureOffset(texture), 1, -Main.camera.Rotation, 6);
        }

        public void DrawBeneath(SpriteBatch batch)
        {
            batch.Draw(floorTex, bounds, Color.White);
        }

        public override void OnCollide(Entity entity)
        {
        }

        public static TileRock Create(Coordinate location, Texture2D rockTex, Texture2D floorTex, Biomes biome)
        {
            return new TileRock(location, rockTex, floorTex, biome);
        }

        /// <summary>
        /// Copies a tile to a position.
        /// </summary>
        /// <param name="position">The position to copy to.</param>
        /// <returns>A new instance of an identecal tile.</returns>
        public override Tile Copy(Coordinate position)
        {
            TileRock copy = new TileRock(position, texture, floorTex, location);
            return copy;
        }
    }
}