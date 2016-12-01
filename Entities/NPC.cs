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

namespace Limestone.Entities
{
    public abstract class NPC : EntityLiving
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        protected Color hoveredColor;

        protected bool hovered = false;
        protected float interactiveDistance = 128;
        protected bool interacting;

        protected int facingAngle;

        public NPC(Vector2 position) : base(position)
        {
            texture = Assets.GetTexture("notex");
            tType = EntityType.NPC;

            shadowScale = 4;
            scale = 4;
            shadowTextureColor = new Color(0, 0, 0, 127);
            baseColor = Color.White;
        }

        public override void Update(World world)
        {
            base.Update(world);

            hovered = (hitbox.ToRectangle().Contains(VectorHelper.ConvertScreenToWorldCoords(Main.mouse.position)) && (world.player.center - center).Length() < interactiveDistance);

            if (hovered)
            {
                OnHover();

                if (Main.mouse.MouseKeyPress(Inp.MouseButton.Left))
                    OnInteract();
            }
        }

        public virtual void OnHover()
        {
        }

        public virtual void OnInteract()
        {
            interacting = true; //NOTE manually set interacting to false.
        }

        public override void Die(World world)
        {
            dead = true;
        }

        public override void OnTileCollide(World world, Tile tile)
        {   //NOOP
        }

        public override void TakeDamage(int amt, IDamageDealer source, World world)
        {   //NOOP NPCS DO NOT TAKE DAMAGE
        }

        protected override void RunFrameConfiguration()
        {
            frameConfiguration.Update();
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            if (frameConfiguration.currentFrame == null)
                frameConfiguration.currentFrame = new Frame(0, setSize);

            Vector2 offset = Main.camera.up * ((shadowTexture.Height * scale) / 32) + Main.camera.up * height;
            Vector2 flipOffset = Main.camera.right * (frameConfiguration.currentFrame.size.Width - setSize.Width) * scale + Main.camera.down * (frameConfiguration.currentFrame.size.Height - setSize.Height) * scale;
            
            if (hovered)
            {
                Texture2D finalTex = Assets.GetSolidFilledTexture(texture, hoveredColor);
                batch.Draw(finalTex, Main.camera.up + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.White, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
                batch.Draw(finalTex, Main.camera.down + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.White, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
                batch.Draw(finalTex, Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.White, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
                batch.Draw(finalTex, Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.White, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);

                batch.Draw(finalTex, Main.camera.up + Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.White, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
                batch.Draw(finalTex, Main.camera.up + Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.White, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
                batch.Draw(finalTex, Main.camera.down + Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.White, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
                batch.Draw(finalTex, Main.camera.down + Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.White, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            }
            else
            {

                batch.Draw(texture, Main.camera.up + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
                batch.Draw(texture, Main.camera.down + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
                batch.Draw(texture, Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
                batch.Draw(texture, Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);

                batch.Draw(texture, Main.camera.up + Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
                batch.Draw(texture, Main.camera.up + Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
                batch.Draw(texture, Main.camera.down + Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
                batch.Draw(texture, Main.camera.down + Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (frameConfiguration.currentFrame == null)
                frameConfiguration.currentFrame = new Frame(-1, new Rectangle(0, 0, 8, 8));

            batch.Draw(shadowTexture, position, null, shadowTextureColor, -Main.camera.Rotation, ShadowOffset(), shadowScale / 8, 0, 0);

            Vector2 offset = Main.camera.up * ((shadowTexture.Height * scale) / 32) + Main.camera.up * height;
            Vector2 flipOffset = Main.camera.right * (frameConfiguration.currentFrame.size.Width - setSize.Width) * scale + Main.camera.down * (frameConfiguration.currentFrame.size.Height - setSize.Height) * scale;
            batch.Draw(texture, position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, color, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);

            foreach (DamageText dt in texts)
                dt.Draw(batch);

            if (Main.options.DEBUGDRAWNPCHITBOXES)
                hitbox.DebugDraw(batch);

            if (Main.options.DEBUGDRAWNPCINTERACTIONRADIUS)
                DrawGeometry.DrawCircle(batch, center, interactiveDistance, new Color(Color.Blue, 63));
        }
    }
}
