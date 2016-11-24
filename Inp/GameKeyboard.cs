using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Limestone.Inp
{
    public class GameKeyboard
    {
        public KeyboardState currentState, previousState;

        private int noInputDuration;
        private bool noInput;

        public void Update()
        {
            currentState = Keyboard.GetState();
        }

        public void PostUpdate()
        {
            previousState = currentState;
            noInputDuration--;
        }

        public void SetNoInput(int duration)
        {
            this.noInput = true;
            this.noInputDuration = duration;
        }

        public bool KeyPressed(Keys key)
        {
            return (currentState.IsKeyDown(key) && previousState.IsKeyUp(key) && !noInput);
        }

        public bool KeyPressedContinuous(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        public bool KeyPressModifier(Keys key, Keys modifier)
        {
            return (currentState.IsKeyDown(modifier) && KeyPressed(key)) || (KeyPressed(modifier) && currentState.IsKeyDown(key) && !noInput);
        }

        public bool KeysPressedTogether(Keys[] keys)
        {
            if (!noInput)
            {
                int num = 0;
                foreach (Keys k in keys)
                {
                    if (currentState.IsKeyDown(k))
                        num++;
                }

                if (num >= 2)
                    return true;
                else return false;
            }
            else return false;
        }
    }
}
