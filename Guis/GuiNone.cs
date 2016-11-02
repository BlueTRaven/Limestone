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
using Limestone.Guis.Widgets;

namespace Limestone.Guis
{
    public class GuiNone : Gui
    {
        public GuiNone()
        {
            stopsWorldInput = false;
            stopsWorldDraw = false;
        }

        public override void Update(Main main)
        {
            if (Main.keyboard.KeyPressed(Keys.Escape))
                Main.camera.activeGui = new GuiPauseMenu();
        }

        public override void PostUpdate()
        {
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.DrawString(Assets.GetFont("bitfontMunro12"), Main.fps, new Vector2(1, 96 - 16), Color.White);
        }
    }
}
