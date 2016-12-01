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
using Limestone.Entities;
using Limestone.Interface;
using Limestone.Guis;

namespace Limestone.Entities.NPCs
{
    public class NPCEcho : NPC
    {
        private bool spokento;

        public NPCEcho(Vector2 position) : base(position)
        {
            height = 128;

            hoveredColor = Color.CornflowerBlue;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(32)));
            setSize = new Rectangle(0, 0, 8, 8);
            scale = 4;
        }

        public override void Update(World world)
        {
            base.Update(world);

            if (moveQueue.Count > 0)
            {
                if (currentMove == null || currentMove.finished)
                {
                    previousMove = currentMove;
                    currentMove = moveQueue.Dequeue();   //Set current style to the next style in the queue.
                }

                currentMove.Update();
            }

            float waveAngle = elapsedTime * 3.14f * .008f;

            height = 128 + (float)Math.Sin(waveAngle) * 32;
        }

        public override void OnInteract()
        {
            if (!interacting)
            {
                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue(". . .", "?", 3, 360))));
                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue("It has been a long time since there has been a visitor here.", "?", 3, 360))));
                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue(". . .", "?", 3, 360))));
                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue("I would ask you to leave. In fact, I would destroy you, as I have done to most others who have come here. But something keeps me from doing so. Something.", "?", 3, 360))));
                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue("I feel strangely obligated to trust you, stranger.", "?", 3, 360))));
                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue("So, I would tell you of this place.", "?", 3, 360))));
                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue("This is a prison. One made long ago to keep the worst of the old world dead.", "?", 3, 360))));
                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue("But it is falling into ruin. It may soon be that their doors will open, and their horrors wraught upon the world.", "?", 3, 360))));
                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue("I fear I must ask for your help, adventurer.", "?", 3, 360))));
                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue("I see strength within you... as well as something I do not understand. \nBut something tells me you will help me.", "?", 3, 360))));
                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue("I am Echo, caretaker of this dead prison.", "?", 3, 360))));

                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue("Please. I need your help dearly.", "Echo", 3, 360))));
                moveQueue.Enqueue(new MoveStyle(0, new Move(() => MoveDialogue("", "Echo", 3, 360))));
            }
            base.OnInteract();
        }

        private void MoveDialogue(string dialogue, string name, int speed, int timeout)
        {
            GuiNone none = (GuiNone)Main.camera.activeGui;
            none.DisplayDialogue(dialogue, name, speed, timeout);
        }

        public override Entity Copy()
        {
            return new NPCEcho(position);
        }
    }
}
