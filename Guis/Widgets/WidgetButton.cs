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
    public enum WidgetButtonState
    {
        Unpressed,
        Hovered,
        Pressed
    }

    public class WidgetButton : Widget
    {
        protected bool hasBackgroundColor;
        protected Color unpressedColor, hoveredColor, pressedColor;

        private bool hasText;
        private SpriteFont font;
        private string text;
        private TextAlignment textAlignment;
        private Color textColor;

        public WidgetButtonState state;

        public bool unpressed { get { return state == WidgetButtonState.Unpressed; } set { } }
        public bool hovered { get { return state == WidgetButtonState.Hovered; } set { } }
        public bool pressed { get { return state == WidgetButtonState.Pressed; } set { } }

        public WidgetButton(Rectangle bounds) : base(bounds) { }

        public WidgetButton SetBackgroundColor(Color unpressedColor, Color hoveredColor, Color pressedColor)
        {
            hasBackgroundColor = true;
            this.unpressedColor = unpressedColor;
            this.hoveredColor = hoveredColor;
            this.pressedColor = pressedColor;
            return this;
        }

        public WidgetButton SetText(SpriteFont font, string text, TextAlignment alignment, Color color)
        {
            hasText = true;
            this.font = font;
            this.text = text;
            this.textAlignment = alignment;
            this.textColor = color;
            return this;
        }

        public override void Update()
        {
            state = WidgetButtonState.Unpressed;
            if (bounds.Contains(Main.mouse.position))
            {
                if (Main.mouse.MouseKeyPress(Inp.MouseButton.Left))
                    state = WidgetButtonState.Pressed;
                else
                    state = WidgetButtonState.Hovered;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (!hasBackgroundColor)
                DrawGeometry.DrawRectangle(batch, bounds, Color.Orange);
            else
            {
                if (state == WidgetButtonState.Unpressed)
                    DrawGeometry.DrawRectangle(batch, bounds, unpressedColor);
                else if (state == WidgetButtonState.Hovered)
                    DrawGeometry.DrawRectangle(batch, bounds, hoveredColor);
                else if (state == WidgetButtonState.Pressed)
                    DrawGeometry.DrawRectangle(batch, bounds, pressedColor);
            }

            if (hasText)
            {
                Vector2 offset = TextHelper.GetAlignmentOffset(font, text, bounds, textAlignment);

                batch.DrawString(font, text, bounds.Location.ToVector2() + offset, textColor);
            }
        }
    }
}
