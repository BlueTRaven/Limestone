using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using Limestone.Utility;
using Limestone.Items;
using Limestone.Entities;
using Limestone.Guis.Widgets;

namespace Limestone.Guis.Widgets
{
    public class WidgetDialogueBox : WidgetButton
    {
        private class Dialogue
        {
            public bool done;

            private string fullText;
            public string name;

            private int timeBetween, timeoutTime;
            private readonly int timeBetweenFull;

            private char[] fullTextChars;
            private int currentChar;

            public StringBuilder fullTextClipped;

            public Dialogue(string text, string name, int betweenTime, int timeoutTime)
            {
                this.fullText = text;
                this.name = name;

                this.timeBetween = betweenTime;
                this.timeBetweenFull = timeBetween;
                this.timeoutTime = timeoutTime;

                fullTextChars = fullText.ToCharArray();

                fullTextClipped = new StringBuilder();
            }

            public void Update()
            {
                timeBetween--;

                if (timeBetween <= 0)
                {
                    if (currentChar < fullTextChars.Count())
                    {
                        timeBetween = timeBetweenFull;

                        fullTextClipped.Append(fullTextChars[currentChar++]);
                    }
                    else timeoutTime--;
                }

                if (timeoutTime <= 0)
                    done = true;
            }
        }

        public bool draw = false;

        Queue<Dialogue> queuedDialogues = new Queue<Dialogue>();
        Dialogue currentDialogue;

        SpriteFont font;

        //private int timeoutTime;

        public WidgetDialogueBox(Rectangle bounds, SpriteFont font) : base(bounds, null, 1)
        {
            this.font = font;
        }

        public override void Update()
        {
            base.Update();

            if (draw)
            {
                currentDialogue.Update();

                if (pressed || currentDialogue.done)
                {
                    if (queuedDialogues.Count <= 0)
                        draw = false;
                    else
                        currentDialogue = queuedDialogues.Dequeue();
                }
            }
        }

        /// <summary>
        /// Adds text to the dialogue widget.
        /// </summary>
        /// <param name="text">Text to be displayed.</param>
        /// <param name="name">The text's name, where it comes from eg. "Joe" or "Info" or "Tutorial"</param>
        /// <param name="timeBetween">The time it takes for each letter to appear.</param>
        /// <param name="timeout">the time it takes for the dialogue box to disappear *note* AFTER all letters have been revealed.</param>
        public void AddText(string text, string name, int timeBetween, int timeout)
        {
            Dialogue newDialog = new Dialogue(text, name, timeBetween, timeout);
            if (queuedDialogues.Count <= 0)
            {
                currentDialogue = newDialog;
            }
            queuedDialogues.Enqueue(newDialog);
            draw = true;
        }

        public override void Draw(SpriteBatch batch)
        {
            if (draw)
            {
                DrawGeometry.DrawRectangle(batch, bounds, Color.Gray);
                DrawGeometry.DrawRectangle(batch, new Rectangle(bounds.X + 3, bounds.Y + 3, bounds.Width - 6, bounds.Height - 6), Color.White);

                Rectangle nameplaterect = new Rectangle(bounds.X + 24, bounds.Y - 16, 96, 16);
                DrawGeometry.DrawRectangle(batch, nameplaterect, Color.Gray);
                DrawGeometry.DrawRectangle(batch, new Rectangle(bounds.X + 27, bounds.Y - 13, 90, 19), Color.White);

                Vector2 nameSize = font.MeasureString(currentDialogue.name);
                batch.DrawString(font, currentDialogue.name, nameplaterect.Center.ToVector2() - new Vector2((nameSize.X / 2), 4), Color.Black);

                string textf = TextHelper.WrapText(font, currentDialogue.fullTextClipped.ToString(), bounds.Width - 16);
                batch.DrawString(font, textf, bounds.Location.ToVector2() + new Vector2(8, 4), Color.Black);
            }
        }


    }
}
