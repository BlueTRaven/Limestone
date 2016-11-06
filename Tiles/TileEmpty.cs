using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;

using Limestone.Utility;


namespace Limestone.Tiles
{
    public class TileEmpty : Tile
    {
        public TileEmpty(Coordinate position) : base(position) { tileType = TileType.None; }

        public override void Draw(SpriteBatch batch) { /*NOOP*/ }

        public override Tile Copy(Coordinate position) { /*NOOP*/ return null; }
    }
}
