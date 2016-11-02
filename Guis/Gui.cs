using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using Limestone.Utility;
using Limestone.Items;
using Limestone.Entities;
using Limestone.Guis.Widgets;

namespace Limestone.Guis
{
    public abstract class Gui
    {
        public bool active = true;
        public bool dead;
        public bool stopsWorldDraw = true, stopsWorldInput = true;

        public List<Widget> widgets = new List<Widget>();
        public abstract void Update(Main main);
        public abstract void PostUpdate();

        public abstract void Draw(SpriteBatch batch);
    }
}
