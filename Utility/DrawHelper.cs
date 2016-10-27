using System;
using System.Text;
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

            batch.Draw(texture, currentOffset + Main.camera.left * thickness, null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.right * thickness, null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.up * thickness, null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.down * thickness, null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);

            batch.Draw(texture, currentOffset + Main.camera.left + Main.camera.up * thickness, null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.right + Main.camera.up * thickness, null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.left + Main.camera.down * thickness, null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.right + Main.camera.down * thickness, null, Color.Black, -Main.camera.Rotation, offset, scale, 0, 0);
        }

        public static void DrawOutline(SpriteBatch batch, Texture2D texture, Vector2 position, Vector2 offset, int thickness, float rotation, float scale)
        {
            Vector2 currentOffset = position + texture.Bounds.Center.ToVector2() * scale;

            batch.Draw(texture, currentOffset + Main.camera.left * thickness, null, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.right * thickness, null, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.up * thickness, null, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.down * thickness, null, Color.Black, rotation, offset, scale, 0, 0);

            batch.Draw(texture, currentOffset + Main.camera.left + Main.camera.up * thickness, null, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.right + Main.camera.up * thickness, null, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.left + Main.camera.down * thickness, null, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.right + Main.camera.down * thickness, null, Color.Black, rotation, offset, scale, 0, 0);
        }

        public static void DrawOutline(SpriteBatch batch, Texture2D texture, Vector2 position, Rectangle sourceRect, Vector2 offset, int thickness, float rotation, float scale)
        {
            Vector2 currentOffset = position;

            batch.Draw(texture, currentOffset + Main.camera.left * thickness, sourceRect, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.right * thickness, sourceRect, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.up * thickness, sourceRect, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.down * thickness, sourceRect, Color.Black, rotation, offset, scale, 0, 0);

            batch.Draw(texture, currentOffset + Main.camera.left + Main.camera.up * thickness, sourceRect, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.right + Main.camera.up * thickness, sourceRect, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.left + Main.camera.down * thickness, sourceRect, Color.Black, rotation, offset, scale, 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.right + Main.camera.down * thickness, sourceRect, Color.Black, rotation, offset, scale, 0, 0);
        }

        public static void DrawOutline(SpriteBatch batch, SpriteFont font, string text, Vector2 position, Vector2 offset, int thickness, float rotation, float scale)
        {
            Vector2 currentOffset = position;

            batch.DrawString(font, text, currentOffset + Main.camera.left * thickness, Color.Black, rotation, offset, scale, 0, 0);
            batch.DrawString(font, text, currentOffset + Main.camera.right * thickness, Color.Black, rotation, offset, scale, 0, 0);
            batch.DrawString(font, text, currentOffset + Main.camera.up * thickness, Color.Black, rotation, offset, scale, 0, 0);
            batch.DrawString(font, text, currentOffset + Main.camera.down * thickness, Color.Black, rotation, offset, scale, 0, 0);

            batch.DrawString(font, text, currentOffset + Main.camera.left + Main.camera.up * thickness, Color.Black, rotation, offset, scale, 0, 0);
            batch.DrawString(font, text, currentOffset + Main.camera.right + Main.camera.up * thickness, Color.Black, rotation, offset, scale, 0, 0);
            batch.DrawString(font, text, currentOffset + Main.camera.left + Main.camera.down * thickness, Color.Black, rotation, offset, scale, 0, 0);
            batch.DrawString(font, text, currentOffset + Main.camera.right + Main.camera.down * thickness, Color.Black, rotation, offset, scale, 0, 0);

        }

        public static void DrawOutline(SpriteBatch batch, Texture2D texture, Vector2 position, Rectangle sourceRect, Vector2 offset, int thickness, bool flipped = false, float scale = 4)
        {
            Vector2 currentOffset = position + texture.Bounds.Center.ToVector2();

            batch.Draw(texture, currentOffset + Main.camera.left * thickness, sourceRect, Color.Black, -Main.camera.Rotation, offset, scale, flipped ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.right * thickness, sourceRect, Color.Black, -Main.camera.Rotation, offset, scale, flipped ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.up * thickness, sourceRect, Color.Black, -Main.camera.Rotation, offset, scale, flipped ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.down * thickness, sourceRect, Color.Black, -Main.camera.Rotation, offset, scale, flipped ? SpriteEffects.FlipHorizontally : 0, 0);

            batch.Draw(texture, currentOffset + Main.camera.left + Main.camera.up * thickness, sourceRect, Color.Black, -Main.camera.Rotation, offset, scale, flipped ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.right + Main.camera.up * thickness, sourceRect, Color.Black, -Main.camera.Rotation, offset, scale, flipped ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.left + Main.camera.down * thickness, sourceRect, Color.Black, -Main.camera.Rotation, offset, scale, flipped ? SpriteEffects.FlipHorizontally : 0, 0);
            batch.Draw(texture, currentOffset + Main.camera.right + Main.camera.down * thickness, sourceRect, Color.Black, -Main.camera.Rotation, offset, scale, flipped ? SpriteEffects.FlipHorizontally : 0, 0);
        }

        public static void StartDrawCameraSpace(SpriteBatch batch, bool endoverride = false, Effect effect = null)
        {
            if (!endoverride)
                batch.End();
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, effect);
        }

        public static void StartDrawWorldSpace(SpriteBatch batch, bool endoverride = false, Effect effect = null)
        {
            if (!endoverride)
                batch.End();
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, effect, Main.camera.GetViewMatrix());
        }

        /*public static string WrapText(SpriteFont font, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    if (size.X > maxLineWidth)
                    {
                        if (sb.ToString() == "")
                        {
                            sb.Append(WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth));
                        }
                        else
                        {
                            sb.Append("\n" + WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth));
                        }
                    }
                    else
                    {
                        sb.Append("\n" + word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }
            }

            return sb.ToString();
        }*/
        public static float StringWidth(SpriteFont font, string text)
        {
            return font.MeasureString(text).X;
        }
        public static string WrapText(SpriteFont font, string text, float lineWidth)
        {
            const string space = " ";
            string[] words = text.Split(new string[] { space }, StringSplitOptions.None);
            float spaceWidth = StringWidth(font, space),
                spaceLeft = lineWidth,
                wordWidth;
            StringBuilder result = new StringBuilder();

            foreach (string word in words)
            {
                wordWidth = StringWidth(font, word);
                if (word.Contains("\n"))
                    spaceLeft = lineWidth;
                else if (wordWidth + spaceWidth > spaceLeft)
                {
                    result.AppendLine();
                    spaceLeft = lineWidth - wordWidth;
                }
                else
                {
                    spaceLeft -= (wordWidth + spaceWidth);
                }
                result.Append(word + space);
            }

            return result.ToString();
        }
    }
}
