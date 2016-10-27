using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Tiles;
using Limestone.Items;

namespace Limestone.Entities
{
    public class Frame
    {
        bool first, last;

        public int duration, maxDuration;
        public Rectangle size;

        public Frame(int duration, Rectangle size)
        {
            this.duration = duration;
            this.maxDuration = duration;
            this.size = size;
        }
    }
}
