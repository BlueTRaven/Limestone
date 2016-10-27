using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Tiles;
using Limestone.Entities;
namespace Limestone.Generation
{
    public class Door
    {
        public Rectangle bounds;
        public List<Room> adjacentRooms = new List<Room>();
        public Door(Rectangle bounds)
        {
            this.bounds = bounds;
        }
    }
}
