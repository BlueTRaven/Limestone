using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone
{
    public class Chatbox
    {
        List<string> totalTexts = new List<string>();
        List<string> texts = new List<string>();
        public string text = "";
        private string wrapped = "";
        private int timeout = 300;
        public bool updated = false;
        public Chatbox()
        {
        }

        public string AddShout(string enemyName, string text)
        {
            texts.Insert(0, "<" + enemyName + ">: " + text + "\n");
            totalTexts.Insert(0, "<" + enemyName + ">: " + text + "\n");
            
            updated = true;
            return this.text;
        }

        public StringBuilder Add(string text)
        {
            return null;
        }

        public string ShowText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string t in texts)
                sb.Append(t);
            if (updated)
            {
                SpriteFont font = Assets.GetFont("munro12");
                StringBuilder wrapped = new StringBuilder();
                wrapped.Append(DrawHelper.WrapText(font, sb.ToString(), 256));

                this.wrapped = wrapped.ToString();
                return this.wrapped.ToString();
            }
            else return this.wrapped;
        }

        public void Update(Main main)
        {
            if (texts.Count > 0)
                timeout--;
            if (timeout <= 0)
            {
                texts.RemoveAt(texts.Count - 1);
                timeout = 300;
            }
        }

        public void InterpretString(Main main)
        {
        }
    }
}
