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
    public class ItemRing : Item
    {
        public Texture2D ringTex;
        public Color ringColor;
        public ItemRing(int type)
        {
            this.type = type;
            this.lType = LimitType.Accessory;
            itemType = 3;

            SetDefaults();
        }
        ~ItemRing()
        {
            texture.Dispose();
        }

        protected override void SetDefaults()
        {
            texture = Assets.GetTexture("ringBase");
            int xloc = 0;
            if (type == 0)
            {
                descriptor.name = "Ring of Majestic Ruby";
                descriptor.description = "A ring with a giant ruby.";
                ringColor = new Color(255, 30, 30);
                rarity = 6;
                xloc = 5;
                health = 300;
                defense = 30000;
                dexterity = 50;
                attack = 50;
            }
            ringTex = Assets.GetTexFromSource(Assets.GetTexture("ringOverlay"), new Rectangle(xloc * 8, 0, 8, 8));
            AddStats();
            descriptor.rarity = rarity.ToString();
            descriptor.SetupPlate();
        }
    }
}
*/