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

            if (!interacting && !spokento && Main.mouse.MouseKeyPress(Inp.MouseButton.Left))
            {
                none.DisplayDialogue("'?'\n. . .", 8, 0);
                none.DisplayDialogue("'?'\nIt has been a long time since there has been a visitor here.", 4, 0);
                none.DisplayDialogue("'?'\n. . .", 6, 0);
                none.DisplayDialogue("'?'\nI would ask you to leave. In fact, I would destroy you, as I have done to most others who have come here. But something keeps me from doing so. Something.", 4, 0);
                none.DisplayDialogue("'?'\nI feel strangely obligated to trust you, stranger.", 4, 0);
                none.DisplayDialogue("'?'\nSo, I would tell you of this place.", 4, 0);
                none.DisplayDialogue("'?'\nThis is a prison. One made long ago to keep the worst of the old world dead.", 4, 0);
                none.DisplayDialogue("'?'\nBut it is falling into ruin. It may soon be that their doors will open, and their horrors wraught upon the world.", 4, 0);
                none.DisplayDialogue("'?'\nI fear I must ask for your help, adventurer.", 4, 0);
                none.DisplayDialogue("'?'\nI see strength within you... as well as something I do not understand. \nBut something tells me you will help me.", 4, 0);
                none.DisplayDialogue("'?'\nI am Echo, caretaker of this dead prison.", 6, 0);
                none.DisplayDialogue("'Echo'\nPlease. I need your help dearly.", 4, 0);

                none.DisplayDialogue("'You'\n...\n\nWhat would you have me do?", 6, 0);
                facingAngle += 90;
                none.DisplayDialogue("'Echo'\nI require you to delve deep inside the vaults, to find a way to seal the cells closed fully once again.", 4, 0);

                spokento = true;
            }

            float waveAngle = elapsedTime * 3.14f * .008f;

            height = 128 + (float)Math.Sin(waveAngle) * 32;
        }

        public override void OnInteract()
        {
            if (!interacting)
            {
                
            }
            base.OnInteract();
        }

        private void MoveDialogue(string dialogue, int speed)
        {
            GuiNone none = (GuiNone)Main.camera.activeGui;
            none.DisplayDialogue(dialogue, speed, 0);
        }

        public override Entity Copy()
        {
            return new NPCEcho(position);
        }
    }
}
