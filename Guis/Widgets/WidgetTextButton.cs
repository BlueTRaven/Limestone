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

namespace Limestone.Guis.Widgets
{
    public class WidgetTextButton : WidgetButton
    {
        private SpriteFont font;
        public string text;
        private TextAlignment textAlignment;
        private Color textColor;

        public WidgetTextButton(Rectangle bounds, SpriteFont font, string text, TextAlignment alignment, Color color) : base(bounds)
        {
            this.font = font;
            this.text = text;
            this.textAlignment = alignment;
            this.textColor = color;
        }

        public override void Draw(SpriteBatch batch)
        {   
            Vector2 offset = TextHelper.GetAlignmentOffset(font, text, bounds, textAlignment);

            if (!hasBackgroundColor)
                batch.DrawString(font, text, bounds.Location.ToVector2() + offset, textColor);
            else
            {
                if (state == WidgetButtonState.Unpressed)
                    batch.DrawString(font, text, bounds.Location.ToVector2() + offset, unpressedColor);
                else if (state == WidgetButtonState.Hovered)
                    batch.DrawString(font, text, bounds.Location.ToVector2() + offset, hoveredColor);
                else if (state == WidgetButtonState.Pressed)
                    batch.DrawString(font, text, bounds.Location.ToVector2() + offset, pressedColor);
            }
        }
    }
}
