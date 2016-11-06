using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Tiles
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TileWall : TileCollidable
    {
        private TextureInfo wallInfo;
        private Texture2D wallTexture;

        /// <summary>
        /// This constructor is for use for world loading only. DO NOT USE.
        /// </summary>
        [JsonConstructor]
        public TileWall(TextureInfo wallTexInfo, TileType tileType, Vector2 realPosition, TextureInfo textureInfo) : base(realPosition)
        {
            this.wallInfo = wallTexInfo;
            this.tileType = tileType;
            this.realPosition = realPosition;
            this.texInfo = textureInfo;

            this.texture = Assets.GetTexFromSource(textureInfo.name, textureInfo.texX, textureInfo.texY);
            this.wallTexture = Assets.GetTexFromSource(wallTexInfo.name, wallTexInfo.texX, wallTexInfo.texY);
        }

        public TileWall(Coordinate position, TextureInfo topInfo, TextureInfo wallInfo, bool drawOutline = true) : base(position)
        {
            tileType = TileType.Wall;
            //bounds = new Rectangle(position.ToPoint(), new Point(Coordinate.coordSize, Coordinate.coordSize));

            this.texInfo = topInfo;
            this.texture = Assets.GetTexFromSource(topInfo.name, topInfo.texX, topInfo.texY);

            this.wallInfo = wallInfo;
            this.wallTexture = Assets.GetTexFromSource(wallInfo.name, wallInfo.texX, wallInfo.texY);

            this.drawOutline = drawOutline;
        }

        public override void OnCollide(World world, Entity entity)
        {
            if (adjacentTiles == null)
                SetCardinalTiles(world);
            if (entity.tType == EntityType.Projectile)
            {
                Projectile2 p = (Projectile2)entity;

                if (p.tileCollides)
                    p.Die(world);
            }
            else if (entity.tType == EntityType.Player || entity.tType == EntityType.Enemy)
            {
                EntityLiving el = (EntityLiving)entity;
                Rectangle hitbox = el.hitbox.ToRectangle();
                Rectangle intersection = Rectangle.Intersect(bounds, hitbox);

                Vector2 centerDistance = bounds.Center.ToVector2() - el.center;

                if (intersection.Width > intersection.Height)
                {
                    if (centerDistance.Y > 0)
                    {
                        if (adjacentTileUp != null &&!adjacentTileUp.collidable)
                            el.Move(new Vector2(0, -intersection.Height - 1));
                    }
                    if (centerDistance.Y < 0)
                    {
                        if (adjacentTileDown != null && !adjacentTileDown.collidable)
                            el.Move(new Vector2(0, intersection.Height + 1));
                    }
                }
                else
                {
                    if (centerDistance.X > 0)
                    {
                        if (adjacentTileLeft != null && !adjacentTileLeft.collidable)
                            el.Move(new Vector2(-intersection.Width, 0));
                    }
                    if (centerDistance.X < 0)
                    {
                        if (adjacentTileRight != null && !adjacentTileRight.collidable)
                            el.Move(new Vector2(intersection.Width, 0));
                    }
                }
            }
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            if (setCardinals)
            {
                for (int i = 0; i < 32; i++)
                {
                    if (adjacentTiles[3] != null && !(adjacentTiles[3] is TileWall))
                        batch.Draw(wallTexture, bounds.Center.ToVector2() + Main.camera.up * i, new Rectangle(0, i / 4, 8, 1), Color.White, MathHelper.ToRadians(0), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
                    if (adjacentTiles[2] != null && !(adjacentTiles[2] is TileWall))
                        batch.Draw(wallTexture, bounds.Center.ToVector2() + Main.camera.up * i, new Rectangle(0, i / 4, 8, 1), Color.White, MathHelper.ToRadians(90), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
                    if (adjacentTiles[1] != null && !(adjacentTiles[1] is TileWall))
                        batch.Draw(wallTexture, bounds.Center.ToVector2() + Main.camera.up * i, new Rectangle(0, i / 4, 8, 1), Color.White, MathHelper.ToRadians(180), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
                    if (adjacentTiles[0] != null && !(adjacentTiles[0] is TileWall))
                        batch.Draw(wallTexture, bounds.Center.ToVector2() + Main.camera.up * i, new Rectangle(0, i / 4, 8, 1), Color.White, MathHelper.ToRadians(270), DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
                }
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, bounds.Center.ToVector2() + Main.camera.up * 32, null, Color.White, 0, DrawHelper.GetTextureOffset(texture.Bounds.Location.ToVector2(), texture.Bounds.Size.ToVector2()), 6, 0, 0);
        }

        #region creation

        /// <summary>
        /// Copies a tile to a position.
        /// </summary>
        /// <param name="position">The position to copy to.</param>
        /// <returns>A new instance of an identecal tile.</returns>
        public override Tile Copy(Coordinate position)
        {
            TileWall copy = new TileWall(position, texInfo, wallInfo, drawOutline);
            copy.canCreateRandomSpawner = canCreateRandomSpawner;
            //copy.position = position;
            //copy.bounds = new Rectangle(new Point(position.x * Coordinate.coordSize, position.y * Coordinate.coordSize), new Point(Coordinate.coordSize, Coordinate.coordSize));
            return copy;
        }
        #endregion
    }
}
