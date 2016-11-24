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
            widgets.Add(new WidgetTextButton(new Rectangle((Main.camera.center - new Vector2(64, 16)).ToPoint(), new Point(128, 32)), Assets.GetFont("bitfontMunro23BOLD"), "Start", TextAlignment.Center, Color.White)
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

            if (((WidgetButton)widgets[0]).pressed || Main.keyboard.KeyPressed(Keys.Enter))
            {
                Main.hold = false;

                main.world = new World();

                Thread loadThread = new Thread(() => main.world.LoadWorld(new Player2(Vector2.Zero)));
                loadThread.Start();
                main.world.mapLoadThread = loadThread;

                Main.camera.activeGui = new GuiLoading(loadThread);

                active = false;
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
