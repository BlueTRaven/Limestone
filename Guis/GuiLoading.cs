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

namespace Limestone.Guis
{
    public class GuiLoading : Gui
    {
        Thread loadingThread;
        private float graphicRot = 0;
        public GuiLoading(Thread thread)
        {
            loadingThread = thread;

            widgets.Add(new WidgetTextButton(new Rectangle((Main.camera.worldCenter - new Vector2(64, 128)).ToPoint(), new Point(128, 32)), Assets.GetFont("bitfontMunro23BOLD"), "Loading", TextAlignment.Center, Color.White)
                .SetBackgroundColor(Color.White, Color.White, Color.White));
        }

        public override void Update(Main main)
        {
            if (!loadingThread.IsAlive)
                Main.camera.activeGui = new GuiNone();
        }
        
        public override void PostUpdate()
        {
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.GraphicsDevice.Clear(Color.Black);

            foreach (Widget widget in widgets)
                widget.Draw(batch);
            graphicRot += .05f;
            batch.Draw(Assets.GetTexture("loading"), Main.camera.worldCenter, null, Color.White, graphicRot, new Vector2(64, 64), 1f, SpriteEffects.None, 0);
            batch.Draw(Assets.GetTexture("loading"), Main.camera.worldCenter, null, Color.White, graphicRot + 90, new Vector2(64, 64), .5f, SpriteEffects.None, 0);
            batch.Draw(Assets.GetTexture("loading"), Main.camera.worldCenter, null, Color.White, graphicRot + 135, new Vector2(64, 64), .25f, SpriteEffects.None, 0);
        }
    }
}
