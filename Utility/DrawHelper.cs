using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;

namespace Limestone.Utility
{
    public static class DrawHelper
    {
        public static Vector2 GetTextureOffset(Texture2D texture)
        {
            return texture.Bounds.Center.ToVector2() - texture.Bounds.Location.ToVector2();
        }

        public static Vector2 GetTextureOffset(Vector2 position, Vector2 size)
        {
            Vector2 sizeMid = size / 2;
            return sizeMid - position;
        }

        public static void DrawOutline(SpriteBatch batch, Texture2D texture, Vector2 position, Vector2 offset, float scale, int thickness)
        {
            Vector2 currentOffset = position + texture.Bounds.Center.ToVector2() * scale;

            batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y), null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y), null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X, currentOffset.Y - thickness), null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X, currentOffset.Y + thickness), null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);

            batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y - thickness), null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y + thickness), null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y + thickness), null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y - thickness), null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
        }

        public static void DrawOutline(SpriteBatch batch, Texture2D texture, Vector2 position, Vector2 offset, int thickness, float rotation, float scale)
        {
            Vector2 currentOffset = position + texture.Bounds.Center.ToVector2() * scale;

            batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y), null, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y), null, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X, currentOffset.Y - thickness), null, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X, currentOffset.Y + thickness), null, Color.Black, rotation, offset, scale, 0, 0);

            batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y - thickness), null, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y + thickness), null, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y + thickness), null, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y - thickness), null, Color.Black, rotation, offset, scale, 0, 0);
        }

        public static void DrawOutline(SpriteBatch batch, Texture2D texture, Vector2 position, Rectangle sourceRect, Vector2 offset, int thickness, float rotation, float scale)
        {
            Vector2 currentOffset = position;

            batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y), sourceRect, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y), sourceRect, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X, currentOffset.Y - thickness), sourceRect, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X, currentOffset.Y + thickness), sourceRect, Color.Black, rotation, offset, scale, 0, 0);

            batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y - thickness), sourceRect, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y + thickness), sourceRect, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y + thickness), sourceRect, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y - thickness), sourceRect, Color.Black, rotation, offset, scale, 0, 0);
        }

        public static void DrawOutline(SpriteBatch batch, Texture2D texture, Vector2 position, Rectangle sourceRect, Vector2 offset, int thickness, bool flipped = false)
        {
            Vector2 currentOffset = position + texture.Bounds.Center.ToVector2();

            if (!flipped)
            {
                batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, 0, 0);
                batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, 0, 0);
                batch.Draw(texture, new Vector2(currentOffset.X, currentOffset.Y - thickness), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, 0, 0);
                batch.Draw(texture, new Vector2(currentOffset.X, currentOffset.Y + thickness), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, 0, 0);

                batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y - thickness), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, 0, 0);
                batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y + thickness), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, 0, 0);
                batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y + thickness), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, 0, 0);
                batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y - thickness), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, 0, 0);
            }
            else
            {
                batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, SpriteEffects.FlipHorizontally, 0);
                batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, SpriteEffects.FlipHorizontally, 0);
                batch.Draw(texture, new Vector2(currentOffset.X, currentOffset.Y - thickness), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, SpriteEffects.FlipHorizontally, 0);
                batch.Draw(texture, new Vector2(currentOffset.X, currentOffset.Y + thickness), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, SpriteEffects.FlipHorizontally, 0);

                batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y - thickness), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, SpriteEffects.FlipHorizontally, 0);
                batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y + thickness), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, SpriteEffects.FlipHorizontally, 0);
                batch.Draw(texture, new Vector2(currentOffset.X - thickness, currentOffset.Y + thickness), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, SpriteEffects.FlipHorizontally, 0);
                batch.Draw(texture, new Vector2(currentOffset.X + thickness, currentOffset.Y - thickness), sourceRect, Color.Black, -Main.camera.Rotation, offset, 5.5f, SpriteEffects.FlipHorizontally, 0);
            }
        }

        public static void StartDrawCameraSpace(SpriteBatch batch, Effect effect = null)
        {
            batch.End();
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, effect);
        }

        public static void StartDrawWorldSpace(SpriteBatch batch, Effect effect = null)
        {
            batch.End();
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, effect, Main.camera.GetViewMatrix());
        }
    }
}
