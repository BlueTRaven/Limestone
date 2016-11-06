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
        /// <summary>
        /// This constructor is for use for world loading only. DO NOT USE.
        /// </summary>
        [JsonConstructor]
        public TileFloor(TileType tileType, Vector2 realPosition, TextureInfo textureInfo) : base(realPosition)
        {
            this.tileType = tileType;
            this.realPosition = realPosition;
            this.texInfo = textureInfo;

            this.texture = Assets.GetTexFromSource(textureInfo.name, textureInfo.texX, textureInfo.texY);
        }

        public TileFloor(Coordinate position, TextureInfo info) : base(position)
        {
            tileType = TileType.Floor;
            //bounds = new Rectangle(position.ToPoint(), new Point(Coordinate.coordSize, Coordinate.coordSize));

            this.texInfo = info;
            this.texture = Assets.GetTexFromSource(info.name, info.texX, info.texY);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, bounds.Center.ToVector2(), null, Color.White, MathHelper.ToRadians(rotation), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);

            if (billboarded)
            {
                Texture2D shadowTexture = Assets.GetTexture("shadow");
                batch.Draw(shadowTexture, bounds.Center.ToVector2(), null, new Color(Color.Black, 125), -Main.camera.Rotation, new Vector2(shadowTexture.Width / 2, shadowTexture.Height / 2), 4f / 8f, 0, 0);
                batch.Draw(billboardTexture, bounds.Center.ToVector2() + Main.camera.up * ((shadowTexture.Height * billboardScale) / 32), null, Color.White, -Main.camera.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), billboardScale, SpriteEffects.None, 0);
            }
        }

        #region creation

        /// <summary>
        /// Copies a tile to a position.
        /// </summary>
        /// <param name="position">The position to copy to.</param>
        /// <returns>A new instance of an identical tile.</returns>
        public override Tile Copy(Coordinate position)
        {
            TileFloor copy = new TileFloor(position, texInfo);
            copy.canCreateRandomSpawner = canCreateRandomSpawner;
            copy.billboarded = billboarded;
            copy.billboardScale = billboardScale;
            copy.billboardTexture = billboardTexture;
            return copy;
        }
        #endregion
    }
}
