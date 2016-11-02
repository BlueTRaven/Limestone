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
    public class GuiMainMenu : Gui
    {
        public GuiMainMenu()
        {
            widgets.Add(new WidgetTextButton(new Rectangle((Main.camera.center - new Vector2(64, 16)).ToPoint(), new Point(128, 32)), Assets.GetFont("bitfontMunro23BOLD"), "Play", TextAlignment.Center, Color.White)
                .SetBackgroundColor(Color.White, Color.DarkGray, Color.Gray));
            widgets.Add(new WidgetTextButton(new Rectangle((Main.camera.center - new Vector2(64, -16)).ToPoint(), new Point(128, 32)), Assets.GetFont("bitfontMunro23BOLD"), "Options", TextAlignment.Center, Color.White)
                .SetBackgroundColor(Color.White, Color.DarkGray, Color.Gray));
            Main.camera.SetFade(Color.Black, true, 120);
        }

        public override void Update(Main main)
        {
            if (active)
            {
                foreach (Widget widget in widgets)
                    widget.Update();

                if (((WidgetButton)widgets[0]).pressed)
                {
                    Main.hold = false;

                    main.world = new World();
                    Thread thread = main.world.CreateWorld();

                    Main.camera.activeGui = new GuiLoading(thread);

                    active = false;
                }

                if (((WidgetButton)widgets[1]).pressed)
                {
                    Main.camera.activeGui = new GuiOptions(this);
                }
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
