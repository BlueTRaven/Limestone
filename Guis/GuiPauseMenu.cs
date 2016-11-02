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
    public class GuiPauseMenu : Gui
    {
        public GuiPauseMenu()
        {
            widgets.Add(new WidgetTextButton(new Rectangle((Main.camera.center - new Vector2(64, 16)).ToPoint(), new Point(128, 32)), Assets.GetFont("bitfontMunro23BOLD"), "Resume", TextAlignment.Center, Color.White)
                .SetBackgroundColor(Color.White, Color.DarkGray, Color.Gray));
            widgets.Add(new WidgetTextButton(new Rectangle((Main.camera.center - new Vector2(64, -16)).ToPoint(), new Point(128, 32)), Assets.GetFont("bitfontMunro23BOLD"), "Options", TextAlignment.Center, Color.White)
                .SetBackgroundColor(Color.White, Color.DarkGray, Color.Gray));
            widgets.Add(new WidgetTextButton(new Rectangle((Main.camera.center - new Vector2(64, -48)).ToPoint(), new Point(128, 32)), Assets.GetFont("bitfontMunro23BOLD"), "Exit", TextAlignment.Center, Color.White)
                .SetBackgroundColor(Color.White, Color.DarkGray, Color.Gray));
        }

        public override void Update(Main main)
        {
            foreach (Widget widget in widgets)
                widget.Update();

            if (((WidgetButton)widgets[0]).pressed)
            {
                Main.camera.activeGui = new GuiNone();
            }

            if (((WidgetButton)widgets[1]).pressed)
            {
                Main.camera.activeGui = new GuiOptions(this);
            }

            if (((WidgetButton)widgets[2]).pressed)
            {
                main.Exit();
            }
        }

        public override void PostUpdate()
        {
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (Widget widget in widgets)
                widget.Draw(batch);
        }
    }
}
