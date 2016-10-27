using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Items;

namespace Limestone.Entities
{
    public enum BagType
    {
        Untiered,
        Potion,
        Rare,
        Uncommon,
        Common
    }
    public class Bag : Entity
    {
        public BagType type;
        public override Vector2 center { get { return new Vector2(position.X + ((texture.Width * scale) / 2), position.Y + ((texture.Height * scale) / 2)); } set { position = new Vector2(value.X - ((texture.Width * scale) / 2), value.Y - ((texture.Height * scale) / 2)); } }

        public ItemSlot[] itemSlots = new ItemSlot[8];    //Can hold up to 8 items
        public Bag(Vector2 position, List<Item> items)
        {
            this.tType = EntityType.Bag;
            if (items.Count <= 0)
                dead = true;
            for (int i = 0; i < 8; i++)
            {
                if (i >= items.Count)
                    itemSlots[i] = new ItemSlot(null, new Rectangle(64 * i, 424, 48, 48));
                else
                    itemSlots[i] = new ItemSlot(items[i], new Rectangle(64 * i, 424, 48, 48));
            }
            this.scale = 3.5f;

            this.texture = FindBagTexture();
            this.center = position;
            this.hitbox = new RotateableRectangle(new Rectangle(this.position.ToPoint(), new Point(64)));
        }

        public override void Die(World world)
        {
            dead = true;
        }

        public override void Update(World world)
        {
            bool foundItem = false;
            foreach(ItemSlot slot in itemSlots)
            {
                if (slot.item != null)
                    foundItem = true;
            }

            if (!foundItem) Die(world);
        }

        public Texture2D FindBagTexture()
        {
            int greatestRarity = 0;
            for (int i = 0; i < 8; i++)
            {
                if (itemSlots[i] != null && itemSlots[i].item != null)
                {
                    if (itemSlots[i].item.rarity > greatestRarity || itemSlots[i].item.rarity == -1)
                        greatestRarity = itemSlots[i].item.rarity;
                }
            }
            return BagRarity(greatestRarity);
        }

        public Texture2D BagRarity(int rarity)
        {
            if (rarity == -1)  
                return Assets.GetTexture("bagUntiered");
            
            if (rarity > 0 && rarity <= 5)
                return Assets.GetTexture("bagCommon");
            else if (rarity > 5 && rarity <= 9)
                return Assets.GetTexture("bagUncommon");
            else if (rarity > 9 && rarity <= 12)
                return Assets.GetTexture("bagRare");
            else return Assets.GetTexture("bagCommon");
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            DrawHelper.DrawOutline(batch, texture, position, DrawHelper.GetTextureOffset(texture), scale, 1);
        }

        public override void Draw(SpriteBatch batch)
        {
            if (!dead && texture != null)
                batch.Draw(texture, position + texture.Bounds.Center.ToVector2() * scale, null, color, -Main.camera.Rotation, DrawHelper.GetTextureOffset(texture), scale, 0, 0);
        }

        /// <summary>
        /// Draws the bag contents.
        /// NOTE: Do DrawHelper.StartDrawCameraSpace before, and DrawHelper.StartDrawWorldSpace after!
        /// </summary>
        /// <param name="batch"></param>
        public void DrawBagContents(SpriteBatch batch, Rectangle inventoryRect)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i < 4)
                    itemSlots[i].bounds = new Rectangle(64 * i, 232, 48, 48);
                else
                    itemSlots[i].bounds = new Rectangle(64 * (i - 4), 300, 48, 48);
            }
            Vector2 mousepos = Main.mouse.position;
            ItemSlot hovered = null;
            for (int i = 0; i < 8; i++)
            {
                if (itemSlots[i] != null)
                {
                    Rectangle bounds = itemSlots[i].bounds;
                    itemSlots[i].bounds = new Rectangle(bounds.X + inventoryRect.X, bounds.Y + inventoryRect.Y, 48, 48);

                    batch.Draw(Assets.GetTexture("guiItemslot"), itemSlots[i].bounds, Color.White);
                    //DrawGeometry.DrawRectangle(batch, itemSlots[i].bounds, Color.Gray);
                    if (itemSlots[i].item != null)
                    {
                        DrawHelper.DrawOutline(batch, itemSlots[i].item.texture, itemSlots[i].drawRect.Location.ToVector2() - new Vector2(16), Vector2.Zero, 1, 0, 4);

                        if (itemSlots[i].item.itemType == 3)
                        {
                            ItemRing ring = (ItemRing)itemSlots[i].item;
                            DrawHelper.DrawOutline(batch, ring.ringTex, itemSlots[i].drawRect.Location.ToVector2() - new Vector2(16), Vector2.Zero, 1, 0, 4);

                            batch.Draw(ring.texture, itemSlots[i].drawRect, Color.White);
                            batch.Draw(ring.ringTex, itemSlots[i].drawRect, ring.ringColor);
                        }
                        else
                            batch.Draw(itemSlots[i].item.texture, itemSlots[i].drawRect, Color.White);
                    }

                    if (itemSlots[i].bounds.Contains(mousepos))
                    {
                        hovered = itemSlots[i];
                        Main.mouse.hoveredSlot = itemSlots[i];
                    }
                }
            }
        }

        public static Bag Create(Vector2 position, List<Item> items)
        {
            return new Bag(position, items);
        }
    }
}
