using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Tiles
{
    public abstract class TileCollidable : Tile
    {
        [JsonConstructor]
        public TileCollidable(Vector2 realPosition) : base(realPosition) { collidable = true; }
        public TileCollidable(Coordinate position) : base(position) { collidable = true; }
        protected bool drawOutline = true;
        public abstract void OnEntityCollide(World world, Entity entity);
        public abstract void DrawOutline(SpriteBatch batch);
    }
}
