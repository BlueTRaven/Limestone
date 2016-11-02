using System;
using System.Threading;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using Limestone.Utility;
using Limestone.Items;
using Limestone.Entities;

namespace Limestone.Guis.Widgets
{
    public abstract class Widget
    {
        protected Rectangle bounds;
        public Widget(Rectangle bounds)
        {
            this.bounds = bounds;
        }
        public abstract void Update();
        public abstract void Draw(SpriteBatch batch);
    }
}
