using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;

using Limestone.Utility;

namespace Limestone.Tiles
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TileFloor : Tile
    {
        public TileFloor(Coordinate position, string textureName, Biomes biome, bool canCreateRandomSpawner = true)
        {
            tileType = TileType.Floor;
            this.position = position;
            bounds = new Rectangle(position.ToPoint(), new Point(Coordinate.coordSize, Coordinate.coordSize));

            this.textureName = textureName;
            this.texture = Assets.GetTexture(textureName);

            this.location = biome;
            this.canCreateRandomSpawner = canCreateRandomSpawner;
            //RandomRot();
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, bounds.Center.ToVector2(), null, Color.White, MathHelper.ToRadians(rotation), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
        }

        #region creation
        public static TileFloor Create(Coordinate position, string texture, Biomes biome)
        {
            TileFloor tF = new TileFloor(position, texture, biome);
            return tF;
        }
        public static Tile Create(Coordinate position)
        {
            TileFloor tf = new TileFloor(position, "water1", Biomes.AncientLands);
            return tf;
        }

        /// <summary>
        /// Copies a tile to a position.
        /// </summary>
        /// <param name="position">The position to copy to.</param>
        /// <returns>A new instance of an identical tile.</returns>
        public override Tile Copy(Coordinate position)
        {
            TileFloor copy = new TileFloor(position, textureName, location);
            copy.canCreateRandomSpawner = canCreateRandomSpawner;
            return copy;
        }
        #endregion
    }
}
