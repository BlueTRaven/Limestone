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
    public class GuiOptions : Gui
    {
        private Gui parent;
        public GuiOptions(Gui parent)
        {
            this.parent = parent;

            widgets.Add(new WidgetTextButton(new Rectangle((new Vector2(Main.camera.center.X, 448) - new Vector2(64, 0)).ToPoint(), new Point(128, 32)), Assets.GetFont("bitfontMunro23BOLD"), "Back", TextAlignment.Center, Color.White)
                .SetBackgroundColor(Color.White, Color.DarkGray, Color.Gray));
            widgets.Add(new WidgetTextButton(new Rectangle((new Vector2(Main.camera.center.X, 448) - new Vector2(224, 32)).ToPoint(), new Point(448, 32)), Assets.GetFont("bitfontMunro23BOLD"), "Draw Projectile Hitboxes: " + Options.DEBUGDRAWPROJECTILEHITBOXES, TextAlignment.Center, Color.White)
                .SetBackgroundColor(Color.White, Color.DarkGray, Color.Gray));
        }

        public override void Update(Main main)
        {
            foreach (Widget widget in widgets)
                widget.Update();

            if (((WidgetButton)widgets[0]).pressed)
                Main.camera.activeGui = parent;
            if (((WidgetButton)widgets[1]).pressed)
                ((WidgetTextButton)widgets[1]).text = "Draw Projectile Hitboxes: " + (Options.DEBUGDRAWPROJECTILEHITBOXES = !Options.DEBUGDRAWPROJECTILEHITBOXES).ToString();
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
