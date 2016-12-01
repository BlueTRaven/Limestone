using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using Limestone.Utility;
using Limestone.Tiles;
using Limestone.Items;


namespace Limestone.Entities
{
    public abstract class Collectable : Entity
    {
        public bool collectable = false;
        public bool collectOverride = false;
        private int _collectTicks;
        public int collectTicks { get { return _collectTicks; } set { _collectTicks = value; collectable = false; } }

        public float shadowScale;

        protected int alive;
        protected float activeDistance;
        protected float distance, rotToPlayer;

        protected float height = 0;

        protected Color baseColor;

        protected SoundEffectInstance collectSound;

        public abstract void OnPickup(World world);

        public bool collected;

        public Collectable(Vector2 position) : base(position)
        {
            texture = Assets.GetTexture("notex");
            tType = EntityType.Collectable;

            shadowTextureColor = new Color(0, 0, 0, 127);
            baseColor = Color.White;
        }

        public override void Update(World world)
        {
            collectable = false;
            RunFrameConfiguration();

            hitbox.MoveTo(position);

            speed = 1;
            alive++;

            rotToPlayer = VectorHelper.GetAngleBetweenPoints(center, world.player.center);
            distance = (center - world.player.center).Length();

            if (collectTicks > 0)
                collectTicks--;
            if (collectTicks <= 0 && !collectOverride)
                collectable = true;
        }

        public override void OnTileCollide(World world, Tile tile)
        {   //NOOP 
        }

        public override void Die(World world)
        {
            dead = true;
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            if (frameConfiguration.currentFrame == null)
                frameConfiguration.currentFrame = new Frame(-1, setSize);

            Vector2 offset = Main.camera.up * ((shadowTexture.Height * scale) / 32) + Main.camera.up * height;
            Vector2 flipOffset = Main.camera.right * (frameConfiguration.currentFrame.size.Width - setSize.Width) * scale + Main.camera.down * (frameConfiguration.currentFrame.size.Height - setSize.Height) * scale;

            batch.Draw(texture, Main.camera.up + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.down + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);

            batch.Draw(texture, Main.camera.up + Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.up + Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.down + Main.camera.left + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, Main.camera.down + Main.camera.right + position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, Color.Black, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);
        }

        public override void Draw(SpriteBatch batch)
        {
            if (frameConfiguration.currentFrame == null)
                frameConfiguration.currentFrame = new Frame(-1, setSize);

            batch.Draw(shadowTexture, position, null, shadowTextureColor, -Main.camera.Rotation, ShadowOffset(), shadowScale / 8, 0, 0);

            Vector2 offset = Main.camera.up * ((shadowTexture.Height * scale) / 32) + Main.camera.up * height;
            Vector2 flipOffset = Main.camera.right * (frameConfiguration.currentFrame.size.Width - setSize.Width) * scale + Main.camera.down * (frameConfiguration.currentFrame.size.Height - setSize.Height) * scale;
            batch.Draw(texture, position + offset - (flip ? flipOffset : Vector2.Zero), frameConfiguration.currentFrame.size, color, -Main.camera.Rotation, TextureOffset(), scale, flip ? SpriteEffects.FlipHorizontally : 0, 0);

            if (Main.options.DEBUGDRAWCOLLECTABLEHITBOXES)
                hitbox.DebugDraw(batch);
        }

        protected void Move(Vector2 moveDirec, float speed)
        {
            position += (moveDirec * speed) * this.speed;
            hitbox.MoveTo(position);
        }
    }
}
