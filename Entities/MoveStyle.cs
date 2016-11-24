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

namespace Limestone.Entities
{
    public delegate void Move();

    public class MoveStyle
    {
        private int duration;   //How long the move lasts
        private Move move;

        public bool active, finished;

        public MoveStyle(int duration, Move move)
        {
            this.duration = duration;
            this.move = move;
        }

        public void Update()
        {
            active = true;

            if (!finished && active)
                move?.Invoke();

            duration--;

            if (duration <= 0)
            {
                finished = true;
                active = false;
            }
        }
    }
}
