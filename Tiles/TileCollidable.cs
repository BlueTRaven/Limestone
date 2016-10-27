using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Tiles
{
    public abstract class TileCollidable : Tile
    {
        protected bool billboarded = false;
        protected bool drawOutline = true;
        public abstract void OnCollide(Entity entity);
        public abstract void DrawOutline(SpriteBatch batch);
    }
}
