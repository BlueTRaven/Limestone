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
        internal static bool DEBUGDRAWLOADINFO = false;
        internal volatile static string DEBUGLOADINGINFO = "none";
        Thread loadingThread;
        private float outerRot, midRot, inRot;
        public GuiLoading(Thread thread)
        {
            loadingThread = thread;

            widgets.Add(new WidgetTextButton(new Rectangle((Main.camera.center - new Vector2(64, 128)).ToPoint(), new Point(128, 32)), Assets.GetFont("bitfontMunro23BOLD"), "Loading", TextAlignment.Center, Color.White)
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

            batch.DrawString(Assets.GetFont("bitfontMunro12"), DEBUGLOADINGINFO, new Vector2(Main.camera.center.X, 0), Color.White);
            outerRot += .05f;
            midRot -= .025f;
            inRot += 0.0125f;
            batch.Draw(Assets.GetTexture("loading"), Main.camera.center, null, Color.White, outerRot, new Vector2(64, 64), 1f, SpriteEffects.None, 0);
            batch.Draw(Assets.GetTexture("loading"), Main.camera.center, null, Color.White, midRot, new Vector2(64, 64), .5f, SpriteEffects.None, 0);
            batch.Draw(Assets.GetTexture("loading"), Main.camera.center, null, Color.White, inRot, new Vector2(64, 64), .25f, SpriteEffects.None, 0);
        }
    }
}
