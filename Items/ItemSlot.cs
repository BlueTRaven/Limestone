/*using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Items
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ItemSlot
    {
        [JsonProperty]
        public Item item;
        public Rectangle bounds;
        public Rectangle drawRect { get { return new Rectangle(bounds.X + 8, bounds.Y + 8, bounds.Width - 16, bounds.Height - 16); } set { } }

        public ItemSlot(Item item, Rectangle rect)
        {
            this.item = item;
            bounds = rect;
        }
    }

    public enum LimitType
    {
        Weapon,
        Ability,
        Armor,
        Accessory
    }
    public class ItemSlotLimited : ItemSlot
    {
        public LimitType type;
        public Class lClass;
        public ItemSlotLimited(Item item, Rectangle rect, LimitType itemType, Class lClass) : base(item, rect)
        {
            this.item = item;
            bounds = rect;
            type = itemType;
            this.lClass = lClass;
            drawRect = new Rectangle(rect.X + 8, rect.Y + 8, rect.Width - 16, rect.Height - 16);
        }
    }
}
*/