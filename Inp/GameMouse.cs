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

namespace Limestone.Inp
{
    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    public class GameMouse
    {
        public MouseState currentState;
        public MouseState prevState;

        /// <summary>
        /// Returns the mouse's position in camera space.
        /// </summary>
        public Vector2 position { get { return currentState.Position.ToVector2(); } }
        //public Item heldItem;
        //public ItemSlot hoveredSlot;

        public Vector2 lastClickPos;

        private SoundEffectInstance errorNoise;
        public GameMouse() { }

        public void Update()
        {
            if (errorNoise == null)
            {
                errorNoise = Assets.GetSoundEffect("error").CreateInstance();
                errorNoise.Pitch = -.7f;
                errorNoise.Volume = .5f;
            }
            currentState = Mouse.GetState();

            if (MouseKeyPress(MouseButton.Left))
                lastClickPos = currentState.Position.ToVector2();
            /*if (hoveredSlot != null)
            {
                if (MouseKeyPress(MouseButton.Left))
                {
                    if (heldItem == null)
                    {   //if there is no current held item, set the held item to the current hovered slot's item and set that slot's item to null.
                        if (hoveredSlot.item != null)
                        {
                            heldItem = hoveredSlot.item;
                            hoveredSlot.item = null;
                        }
                    }
                    else if (heldItem != null)
                    {   //if there is a held item,
                        if (hoveredSlot.item == null)
                        {
                            if (hoveredSlot is ItemSlotLimited)
                            {   //if it's a limited type slot
                                ItemSlotLimited l = (ItemSlotLimited)hoveredSlot;
                                if (l.type == heldItem.lType)
                                {   //check if their types are compatable
                                    if (heldItem.equippableBy.Contains(l.lClass) || l.lClass == Class.NONE)
                                    {
                                        hoveredSlot.item = heldItem;
                                        heldItem = null;
                                    }
                                    else errorNoise.Play();
                                }
                                else errorNoise.Play();
                            }
                            else
                            {
                                hoveredSlot.item = heldItem;
                                heldItem = null;
                            }
                        }
                        else
                        {
                            if (hoveredSlot is ItemSlotLimited)
                            {   //if it's a limited type slot
                                ItemSlotLimited l = (ItemSlotLimited)hoveredSlot;
                                if (l.type == heldItem.lType)
                                {   //check if their types are compatable
                                    if (heldItem.equippableBy.Contains(l.lClass) || l.lClass == Class.NONE)
                                    {
                                        var temp = hoveredSlot.item;
                                        hoveredSlot.item = heldItem;
                                        heldItem = temp;
                                    }
                                    else errorNoise.Play();
                                }
                                else errorNoise.Play();
                            }
                            else
                            {
                                var temp = hoveredSlot.item;
                                hoveredSlot.item = heldItem;
                                heldItem = temp;
                            }
                        }
                    }
                }
            }
            hoveredSlot = null;*/
        }

        public void PostUpdate()
        {
            prevState = currentState;
        }

        public bool MouseKeyPress(MouseButton button)
        {
            if (button == MouseButton.Left)
                return currentState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released;
            else return false;
        }

        public bool MouseKeyPressContinuous(MouseButton button)
        {
            if (button == MouseButton.Left)
                return Main.mouse.prevState.LeftButton == ButtonState.Pressed;
            else return false;
        }

        public void Draw(SpriteBatch batch)
        {
            /*if (heldItem != null)
                batch.Draw(heldItem.texture, new Rectangle((int)position.X - 8, (int)position.Y - 8, 16, 16), Color.White);
            else
            {
                if (hoveredSlot != null && hoveredSlot.item != null)
                {
                    hoveredSlot.item.descriptor.DrawPlate(hoveredSlot.bounds.Center.ToVector2(), 
                        batch.GraphicsDevice.Viewport.Width, batch.GraphicsDevice.Viewport.Height, batch);
                }
            }*/
        }
    }
}
