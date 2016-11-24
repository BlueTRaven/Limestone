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
        WidgetDialogueBox dialogue;

        public bool showingDialogue;
        public GuiNone()
        {
            stopsWorldInput = false;
            stopsWorldDraw = false;

            dialogue = new WidgetDialogueBox(new Rectangle(0, Main.HEIGHT - 96, Main.WIDTH, 96), Assets.GetFont("munro12"));
        }

        public override void Update(Main main)
        {
            dialogue.Update();
            showingDialogue = dialogue.draw;
            if (Main.keyboard.KeyPressed(Keys.Escape))
                Main.camera.activeGui = new GuiPauseMenu();
        }

        public void DisplayDialogue(string text, int speed, int timeout)
        {
            dialogue.AddText(text, speed, timeout);
        }

        public override void PostUpdate()
        {
        }

        public override void Draw(SpriteBatch batch)
        {
            dialogue.Draw(batch);
            batch.DrawString(Assets.GetFont("bitfontMunro12"), Main.fps, new Vector2(1, 96 - 16), Color.White);
        }
    }
}
