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
        public WidgetButtonState forceState;
        public bool stateForce;

        public bool unpressed { get { return state == WidgetButtonState.Unpressed; } set { } }
        public bool hovered { get { return state == WidgetButtonState.Hovered; } set { } }
        public bool pressed { get { return state == WidgetButtonState.Pressed; } set { } }

        private Texture2D texture;
        private float scale;
        public WidgetButton(Rectangle bounds, Texture2D texture, float scale) : base(bounds) { this.texture = texture; this.scale = scale; }

        public WidgetButton SetBackgroundColor(Color unpressedColor, Color hoveredColor, Color pressedColor)
        {
            hasBackgroundColor = true;
            this.unpressedColor = unpressedColor;
            this.hoveredColor = hoveredColor;
            this.pressedColor = pressedColor;
            return this;
        }

        public WidgetButton SetForceState(WidgetButtonState state)
        {
            stateForce = true;
            forceState = state;
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
                {
                    state = WidgetButtonState.Pressed;
                    Assets.GetSoundEffect("buttonclick").Play();
                }
                else
                    state = WidgetButtonState.Hovered;
            }

            if (stateForce)
                state = forceState;
        }

        public override void Draw(SpriteBatch batch)
        {
            if (hasBackgroundColor)
            {
                if (state == WidgetButtonState.Unpressed)
                    DrawGeometry.DrawRectangle(batch, bounds, unpressedColor);
                else if (state == WidgetButtonState.Hovered)
                    DrawGeometry.DrawRectangle(batch, bounds, hoveredColor);
                else if (state == WidgetButtonState.Pressed)
                    DrawGeometry.DrawRectangle(batch, bounds, pressedColor);
            }

            if (texture != null)
                batch.Draw(texture, bounds.Location.ToVector2(), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            if (hasText)
            {
                Vector2 offset = TextHelper.GetAlignmentOffset(font, text, bounds, textAlignment);

                batch.DrawString(font, text, bounds.Location.ToVector2() + offset, textColor);
            }
        }
    }
}
