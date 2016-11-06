using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Tiles
{
    public class TileBreakable : TileCollidable
    {
        public Tile tileMadeWhenBroken;

        public int health;
        public TileBreakable(Coordinate position, TileBreakable copy) : base(position)
        {
            this.position = position;

            this.texture = copy.texture;

            this.tileMadeWhenBroken = copy.tileMadeWhenBroken;
            this.health = copy.health;
        }

        public TileBreakable(Coordinate position, Texture2D texture, Tile tileMadeWhenBroken, int hp) : base(position)
        {
            this.position = position;
            bounds = new Rectangle(new Point(position.x * Coordinate.coordSize, position.y * Coordinate.coordSize), new Point(Coordinate.coordSize, Coordinate.coordSize));

            this.texture = texture;

            this.tileMadeWhenBroken = tileMadeWhenBroken;
            this.health = hp;
        }

        public void OnBroken(World world)
        {

        }

        public override void OnCollide(World world, Entity entity)
        {
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            DrawHelper.DrawOutline(batch, texture, bounds.Center.ToVector2() - (texture.Bounds.Center.ToVector2() * 8), Vector2.Zero, 2, 0, 4);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, bounds, Color.White);
            DrawGeometry.DrawHollowRectangle(batch, bounds, 1, Color.Red);
        }

        #region creation

        public override Tile Copy(Coordinate position)
        {
            TileBreakable created = new TileBreakable(position, texture, tileMadeWhenBroken, health);
            return created;
        }
        #endregion
    }
}
