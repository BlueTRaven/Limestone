using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;
using Limestone.Buffs;

namespace Limestone.Items
{
    public enum ArmorType
    {
        Heavy,
        Medium,
        Light
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class ItemArmor : Item
    {
        ArmorType aType;
        public ItemArmor(int type, ArmorType armorType) : base()
        {
            this.type = type;
            this.lType = LimitType.Armor;
            this.aType = armorType;

            itemType = 2;
            SetDefaults();
        }
        ~ItemArmor()
        {
            texture.Dispose();
        }

        protected override void SetDefaults()
        {
            #region Light
            if (aType == ArmorType.Light)
            {
                if (type == 0)
                {
                    descriptor.name = "Test Light Armor";
                    descriptor.description = "A light armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(0, 0, 8, 8));
                    rarity = 1;
                    defense = 1;
                    wisdom = 1;
                    mana = 10;
                }
                else if (type == 1)
                {
                    descriptor.name = "Test Light Armor";
                    descriptor.description = "A light armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(8, 0, 8, 8));
                    rarity = 2;
                    defense = 2;
                    wisdom = 2;
                    mana = 12;
                }
                else if (type == 2)
                {
                    descriptor.name = "Test Light Armor";
                    descriptor.description = "A light armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(16, 0, 8, 8));
                    rarity = 3;
                    defense = 4;
                    wisdom = 3;
                    mana = 14;
                }
                else if (type == 3)
                {
                    descriptor.name = "Test Light Armor";
                    descriptor.description = "A light armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(24, 0, 8, 8));
                    rarity = 4;
                    defense = 5;
                    wisdom = 4;
                    mana = 16;
                }
                else if (type == 4)
                {
                    descriptor.name = "Test Light Armor";
                    descriptor.description = "A light armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(32, 0, 8, 8));
                    rarity = 5;
                    defense = 7;
                    wisdom = 5;
                    mana = 18;
                }
                else if (type == 5)
                {
                    descriptor.name = "Test Light Armor";
                    descriptor.description = "A light armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(40, 0, 8, 8));
                    rarity = 6;
                    defense = 8;
                    wisdom = 6;
                    mana = 20;
                }
                else if (type == 6)
                {
                    descriptor.name = "Test Light Armor";
                    descriptor.description = "A light armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(48, 0, 8, 8));
                    rarity = 7;
                    defense = 10;
                    wisdom = 7;
                    mana = 22;
                }
                else if (type == 7)
                {
                    descriptor.name = "Test Light Armor";
                    descriptor.description = "A light armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(56, 0, 8, 8));
                    rarity = 8;
                    defense = 11;
                    wisdom = 8;
                    mana = 24;
                    attack = 1;
                }
                else if (type == 8)
                {
                    descriptor.name = "Test Light Armor";
                    descriptor.description = "A light armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(64, 0, 8, 8));
                    rarity = 9;
                    defense = 12;
                    wisdom = 9;
                    mana = 26;
                    attack = 2;
                }
                else if (type == 9)
                {
                    descriptor.name = "Test Light Armor";
                    descriptor.description = "A light armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(72, 0, 8, 8));
                    rarity = 10;
                    defense = 14;
                    wisdom = 10;
                    mana = 28;
                    attack = 3;
                }
                else if (type == 10)
                {
                    descriptor.name = "Test Light Armor";
                    descriptor.description = "A light armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(80, 0, 8, 8));
                    rarity = 11;
                    defense = 15;
                    wisdom = 11;
                    mana = 30;
                    attack = 4;
                }
                else if (type == 11)
                {
                    descriptor.name = "Test Light Armor";
                    descriptor.description = "A light armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(88, 0, 8, 8));
                    rarity = 12;
                    defense = 16;
                    wisdom = 12;
                    mana = 32;
                    attack = 5;
                }
                else if (type == 12)
                {
                    descriptor.name = "Robe of Tefnut";
                    descriptor.description = "A robe woven by Tefnut herself out of the clouds.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorLight"), new Rectangle(96, 0, 8, 8));

                    rarity = -1;
                    defense = 14;
                    dexterity = 5;
                    attack = 5;
                    mana = 50;
                }
            }
            #endregion
            #region Medium
            else if (aType == ArmorType.Medium)
            {
                if (type == 0)
                {
                    descriptor.name = "Test Medium Armor";
                    descriptor.description = "A medium armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorMedium"), new Rectangle(0, 0, 8, 8));
                    rarity = -1;
                    defense = 20;
                    dexterity = 75;
                    attack = 75;
                    wisdom = 75;
                }
            }
            #endregion
            #region Heavy
            else if (aType == ArmorType.Heavy)
            {
                if (type == 0)
                {
                    descriptor.name = "Test Heavy Armor";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(0, 0, 8, 8));
                    rarity = 1;
                    defense = 5;
                }
                else if (type == 1)
                {
                    descriptor.name = "Test Heavy Armor";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(8, 0, 8, 8));
                    rarity = 2;
                    defense = 7;
                }
                else if (type == 2)
                {
                    descriptor.name = "Test Heavy Armor";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(16, 0, 8, 8));
                    rarity = 3;
                    defense = 9;
                }
                else if (type == 3)
                {
                    descriptor.name = "Test Heavy Armor";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(24, 0, 8, 8));
                    rarity = 4;
                    defense = 11;
                }
                else if (type == 4)
                {
                    descriptor.name = "Test Heavy Armor";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(32, 0, 8, 8));
                    rarity = 5;
                    defense = 13;
                }
                else if (type == 5)
                {
                    descriptor.name = "Test Heavy Armor";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(48 , 0, 8, 8));
                    rarity = 6;
                    defense = 15;
                }
                else if (type == 6)
                {
                    descriptor.name = "Test Heavy Armor";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(56, 0, 8, 8));
                    rarity = 7;
                    defense = 17;
                }
                else if (type == 7)
                {
                    descriptor.name = "Test Heavy Armor";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(64, 0, 8, 8));
                    rarity = 8;
                    defense = 20;
                }
                else if (type == 8)
                {
                    descriptor.name = "Test Heavy Armor";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(72, 0, 8, 8));
                    rarity = 9;
                    defense = 23;
                }
                else if (type == 9)
                {
                    descriptor.name = "Test Heavy Armor";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(80, 0, 8, 8));
                    rarity = 10;
                    defense = 26;
                }
                else if (type == 10)
                {
                    descriptor.name = "Test Heavy Armor";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(88, 0, 8, 8));
                    rarity = 11;
                    defense = 29;
                }
                else if (type == 11)
                {
                    descriptor.name = "Test Heavy Armor";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(96, 0, 8, 8));
                    rarity = 12;
                    defense = 32;
                }
                else if (type == 12)
                {
                    descriptor.name = "Plate Armor of Shu";
                    descriptor.description = "A heavy armor used for testing.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("armorHeavy"), new Rectangle(104, 0, 8, 8));
                    rarity = -1;
                    defense = 25;
                    dexterity = 3;
                    speed = 5;
                }
            }
            #endregion
            AddStats();
            descriptor.rarity = rarity.ToString();
            descriptor.type = aType.ToString();
            descriptor.SetupPlate();
        }
    }
}
