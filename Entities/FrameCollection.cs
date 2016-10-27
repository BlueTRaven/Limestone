using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Tiles;
using Limestone.Items;

namespace Limestone.Entities
{
    public class FrameCollection
    {
        public List<Frame> frames = new List<Frame>();

        public int currentFrame = 0;
        public Frame currentFFrame;
        public bool active = false;
        private bool deactivatesOnCompletion;
        public FrameCollection(bool deactivates, params Frame[] frames)
        {
            this.deactivatesOnCompletion = deactivates;
            this.frames.AddRange(frames);

            currentFFrame = this.frames[currentFrame];
        }

        public void Update()
        {
            frames[currentFrame].duration--;

            if (frames[currentFrame].duration <= 0)
            {
                if (currentFrame == frames.Count - 1)
                {
                    currentFrame = 0;
                    if (deactivatesOnCompletion)
                        active = false;
                }
                else currentFrame++;

                frames[currentFrame].duration = frames[currentFrame].maxDuration;

                currentFFrame = frames[currentFrame];
            }
        }

        public void SetInactive()
        {
            currentFrame = 0;
            frames[currentFrame].duration = frames[currentFrame].maxDuration;

            currentFFrame = frames[currentFrame];
        }
    }
}
