/*using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Items
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Item
    {
        public List<Class> equippableBy = new List<Class>(); //A list of classes that can equip this item. If NONE, then it's not equipment.

        [JsonProperty]
        public int type;
        [JsonProperty]
        public int itemType;
        public int rarity;

        public LimitType lType;
        public Class lClass;

        public ItemDescriptor descriptor;

        public Texture2D texture;
        public int damage, damageMin, damageMax;
        public int defense, speed, attack, wisdom, vitality, health, mana;    //the stats this item will give
        public float dexterity;
        protected void AddStats()
        {
            if (damage != 0)
                descriptor.stats.Append("Damage: " + damage + "\n");
            else
            {
                if (damageMax != 0 && damageMax != 0)
                    descriptor.stats.Append("Damage: " + damageMin + " - " + damageMax + "\n");
            }
            if (attack != 0)
                descriptor.stats.Append("att: " + attack + "\n");
            if (defense != 0)
                descriptor.stats.Append("def: " + defense + "\n");
            if (dexterity != 0)
                descriptor.stats.Append("dex: " + dexterity + "\n");
            if (vitality != 0)
                descriptor.stats.Append("vit: " + vitality + "\n");
            if (wisdom != 0)
                descriptor.stats.Append("wis: " + wisdom + "\n");
            if (speed != 0)
                descriptor.stats.Append("spd: " + speed + "\n");
            if (health != 0)
                descriptor.stats.Append("health: " + health + "\n");
            if (mana != 0)
                descriptor.stats.Append("mana: " + mana + "\n");
        }

        public Item()
        {
            descriptor = new ItemDescriptor();
        }

        protected abstract void SetDefaults();
    }
}
*/