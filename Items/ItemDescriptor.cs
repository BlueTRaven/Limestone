using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Items
{
    public class ItemDescriptor
    {
        public Rectangle bounds;

        public string name, description, rarity, type, classuse, abilitydescription, damage;
        private Vector2 namePos, descriptionPos, tierPos, typePos, classusePos, abilitydescriptionPos, statsPos;
        private SpriteFont font;
        private SpriteFont font2;

        public StringBuilder stats = new StringBuilder();
        public ItemDescriptor()
        {
            name = description = rarity = type = classuse = abilitydescription = "@NULL@";
            font = Assets.GetFont("bitfontMunro8");
            font2 = Assets.GetFont("bitfontMunro12");
        }

        public void SetupPlate()
        {
            abilitydescription = "Ability Description: " + abilitydescription;
            description = "Description: " + description;

            namePos = new Vector2(8, 8);
            tierPos = new Vector2(192 - 24, 24);
            typePos = new Vector2(8, 24);
            classusePos = new Vector2(8, 36);
            Vector2 size = font.MeasureString(DrawHelper.WrapText(font, abilitydescription, 192 - 8));
            abilitydescriptionPos = new Vector2(8, 48);
            Vector2 size2 = font.MeasureString(DrawHelper.WrapText(font, description, 192 - 8));
            descriptionPos = new Vector2(8, 48 + size.Y);
            statsPos = new Vector2(8, 48 + size2.Y + size.Y);

            bounds = new Rectangle(0, 0, 192, 192);
        }

        public void DrawPlate(Vector2 position, int maxWidth, int maxHeight,  SpriteBatch batch)
        {
            float posX = MathHelper.Clamp(position.X, 0, maxWidth - bounds.Width);
            float posY = MathHelper.Clamp(position.Y, 0, maxWidth - 512);

            bounds = new Rectangle((int)posX, (int)posY, 192, 192);
            Rectangle smallBounds = new Rectangle(bounds.X + 4, bounds.Y + 4, bounds.Width - 8, bounds.Height - 8);
            batch.Draw(Assets.GetTexture("guiItemslot"), bounds, Color.White);
            //DrawGeometry.DrawRectangle(batch, bounds, Color.DimGray);
            //DrawGeometry.DrawRectangle(batch, smallBounds, Color.Gray);

            batch.DrawString(font2, name, bounds.Location.ToVector2() + namePos, Color.White);
            if (Int32.Parse(rarity) != -1)
                batch.DrawString(font2, rarity, bounds.Location.ToVector2() + tierPos, Color.White);
            else
                batch.DrawString(font2, "UT", bounds.Location.ToVector2() + tierPos, Color.Purple);
            batch.DrawString(font, type, bounds.Location.ToVector2() + typePos, Color.White);
            batch.DrawString(font, classuse, bounds.Location.ToVector2() + classusePos, Color.White);
            if (!abilitydescription.Contains("@NULL@"))
            {
                batch.DrawString(font, DrawHelper.WrapText(font, abilitydescription, smallBounds.Width - 4), bounds.Location.ToVector2() + abilitydescriptionPos, Color.White);
                batch.DrawString(font, DrawHelper.WrapText(font, description, smallBounds.Width - 4), bounds.Location.ToVector2() + descriptionPos, Color.White);
            }
            else
            {
                batch.DrawString(font, DrawHelper.WrapText(font, description, smallBounds.Width - 4), bounds.Location.ToVector2() + abilitydescriptionPos, Color.White);
            }
            batch.DrawString(font, stats.ToString(), bounds.Location.ToVector2() + statsPos, Color.White);
        }
    }
}
