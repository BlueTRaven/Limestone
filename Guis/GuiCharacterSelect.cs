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
    public class GuiCharacterSelect : Gui
    {
        GuiMainMenu menu; //TODO make this save selector instead

        Player p = null;

        public WidgetButtonState[] charselectstates;

        public GuiCharacterSelect(GuiMainMenu menu)
        {
            this.menu = menu;

            widgets.Add(new WidgetTextButton(new Rectangle((new Vector2(Main.camera.center.X, 448) - new Vector2(64, 0)).ToPoint(), new Point(128, 32)), Assets.GetFont("bitfontMunro23BOLD"), "Back", TextAlignment.Center, Color.White)
                .SetBackgroundColor(Color.White, Color.DarkGray, Color.Gray));
            widgets.Add(new WidgetTextButton(new Rectangle((new Vector2(Main.camera.center.X, 448) - new Vector2(224, 32)).ToPoint(), new Point(448, 32)), Assets.GetFont("bitfontMunro23BOLD"), "Create", TextAlignment.Center, Color.White)
                .SetBackgroundColor(Color.White, Color.DarkGray, Color.Gray));

            charselectstates = new WidgetButtonState[3];

            widgets.Add(new WidgetButton(new Rectangle(0, 0, 96, 96), Assets.GetTexFromSource(Assets.GetTexture("guiChars"), new Rectangle(0, 0, 48, 48)), 2)
                .SetText(Assets.GetFont("bitfontMunro8"), "Archer", TextAlignment.Bottom, Color.White)
                .SetBackgroundColor(Color.White, Color.DarkGray, Color.Gray));
            widgets.Add(new WidgetButton(new Rectangle(128, 0, 96, 96), Assets.GetTexFromSource(Assets.GetTexture("guiChars"), new Rectangle(48, 0, 48, 48)), 2)
                .SetText(Assets.GetFont("bitfontMunro8"), "Ice Mage", TextAlignment.Bottom, Color.White)
                .SetBackgroundColor(Color.White, Color.DarkGray, Color.Gray));
            widgets.Add(new WidgetButton(new Rectangle(256, 0, 96, 96), Assets.GetTexFromSource(Assets.GetTexture("guiChars"), new Rectangle(96, 0, 48, 48)), 2)
                .SetText(Assets.GetFont("bitfontMunro8"), "IDK Yet", TextAlignment.Bottom, Color.White)
                .SetBackgroundColor(Color.White, Color.DarkGray, Color.Gray));
        }

        public override void Update(Main main)
        {
            foreach (Widget widget in widgets)
                widget.Update();

            if (((WidgetButton)widgets[0]).pressed)
                Main.camera.activeGui = menu;

            for (int i = 2; i <= 4; i++)
            {
                //for all the character buttons:
                WidgetButton button = (WidgetButton)widgets[i]; //cast to button for easy access

                if (button.pressed)
                {   //if the button is pressed
                    charselectstates[i - 2] = WidgetButtonState.Pressed;    //set its force state to pressed

                    for (int i2 = 2; i2 <= 4; i2++)
                    {   //loop through the rest of the buttons
                        if (i2 != i)    //if it's not the currently looped button
                            charselectstates[i2 - 2] = WidgetButtonState.Unpressed; //unpress all of them, so we only have one button pressed
                    }

                    if (i == 2)
                        p = new Player(Vector2.Zero, Class.Archer);
                    if (i == 3)
                        p = new Player(Vector2.Zero, Class.IceMage);
                    if (i == 4)
                        p = new Player(Vector2.Zero, Class.TimeKeeper);
                }

                if (charselectstates[i - 2] == WidgetButtonState.Pressed)   //set them to pressed.
                    button.state = WidgetButtonState.Pressed;
            }

            if (((WidgetButton)widgets[1]).pressed)
            {
                if (p != null)
                {
                    Main.hold = false;

                    main.world = new World();
                    /*Thread thread = main.world.CreateWorld(p);

                    Main.camera.activeGui = new GuiLoading(thread);*/

                    Thread loadThread = new Thread(() => main.world.LoadWorld(p));
                    loadThread.Start();
                    main.world.mapLoadThread = loadThread;

                    Main.camera.activeGui = new GuiLoading(loadThread);
                }
                else
                {
                    Assets.GetSoundEffect("error").Play();
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
