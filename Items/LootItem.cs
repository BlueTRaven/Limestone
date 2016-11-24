/*using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Items
{
    public class LootItem
    {
        public List<LootRule> lootRules = new List<LootRule>();
        private int type;

        public LootItem(int type)
        {
            this.type = type;

            SetDefaults();
        }

        private void SetDefaults()
        {
            if (type == 0)
            {
            }
            else if (type == 1)
            {
                lootRules.Add(new LootRule(new ItemArmor(12, ArmorType.Light), 100));
            }
            else if (type == 2)
            {
                lootRules.Add(new LootRule(new ItemArmor(12, ArmorType.Heavy), 100));
            }
        }

        public List<Item> Roll()
        {
            List<Item> items = new List<Item>();
            foreach(LootRule rule in lootRules)
            {
                int random = Main.rand.Next(0, 100);
                //Console.WriteLine("does tier " + (rule.item.type + 1) + " drop: " + random + " <= " + rule.chance + " = " + (random <= rule.chance));
                if (random <= rule.chance)
                {
                    //Console.WriteLine("Dropping " + (rule.item.type + 1));
                    items.Add(rule.item);
                }
            }
            return items;
        }
    }
}
*/