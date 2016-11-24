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

            private string fullText, currentText;
            private int timeBetween, timeoutTime;
            private readonly int timeBetweenFull;

            private char[] fullTextChars;
            private int currentChar;

            public StringBuilder fullTextClipped;

            public Dialogue(string text, int betweenTime, int timeoutTime)
            {
                this.fullText = text;
                this.timeBetween = betweenTime;
                this.timeBetweenFull = timeBetween;
                this.timeoutTime = timeoutTime;

                fullTextChars = fullText.ToCharArray();

                fullTextClipped = new StringBuilder();
            }

            public void Update()
            {
                timeBetween--;
                timeoutTime--;

                if (timeBetween <= 0)
                {
                    timeBetween = timeBetweenFull;

                    if (currentChar < fullTextChars.Count())
                    {
                        currentChar++;

                        fullTextClipped.Append(fullTextChars[currentChar]);
                    }
                }

                if (timeoutTime <= 0)
                    done = true;
            }
        }

        public bool draw = false;

        Queue<Dialogue> queuedDialogues = new Queue<Dialogue>();
        Dialogue currentDialogue;

        List<Tuple<string, int>> queuedText = new List<Tuple<string, int>>();
        string _currentTextFull;
        string currentTextFull { get { return _currentTextFull; } set { _currentTextFull = value; currentTextChars = value.ToCharArray(); currentTextClipped = new StringBuilder(); currentChar = 0; } }
        char[] currentTextChars;

        StringBuilder currentTextClipped;
        int currentChar, timeToNext, timeMax;

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
                //timeoutTime--;

                if (currentChar < currentTextChars.Length)
                {
                    timeToNext--;

                    if (timeToNext <= 0)
                    {
                        timeToNext = timeMax;

                        currentTextClipped.Append(currentTextChars[currentChar++]);

                        if (currentTextChars[currentChar - 1] != ' ' && currentTextChars[currentChar - 1] != '\n' && currentTextChars[currentChar - 1] != '\r')   //if it's not a space, newline, or character break character
                        Assets.GetSoundEffect("buttonclick").Play();
                    }
                }

                if (pressed)// || timeoutTime <= 0)
                {
                    if (queuedText.Count > 0)
                    {
                        if (queuedText.Count > 1)
                        {
                            currentTextFull = queuedText[1].Item1;
                            timeMax = queuedText[1].Item2;
                        }
                        queuedText.RemoveAt(0);  //remove from the start
                    }
                }
            }
            if (queuedText.Count == 0)
                draw = false;
        }

        public void AddText(string text, int time, int timeout)
        {
            if (queuedDialogues.Count <= 0)
            {
                currentDialogue = new Dialogue(text, time, timeout);
                queuedDialogues.Enqueue(currentDialogue);
            }

            /*if (queuedText.Count == 0)
            {
                currentTextFull = text;
                timeMax = time;
            }
            queuedText.Add(new Tuple<string, int>(text, time));*/

            //this.timeoutTime = timeout;
            draw = true;
        }

        public override void Draw(SpriteBatch batch)
        {
            if (draw)
            {
                DrawGeometry.DrawRectangle(batch, bounds, Color.Gray);
                DrawGeometry.DrawRectangle(batch, new Rectangle(bounds.X + 3, bounds.Y + 3, bounds.Width - 6, bounds.Height - 6), Color.White);

                string textf = TextHelper.WrapText(font, currentTextClipped.ToString(), bounds.Width);
                batch.DrawString(font, textf, bounds.Location.ToVector2() + new Vector2(8), Color.Black);
            }
        }


    }
}
