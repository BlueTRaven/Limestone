using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Items
{
    public enum WeaponType
    {
        Sword,
        Bow,
        EleOrb
    }

    public delegate void ShootFunction(World world, Player player);

    [JsonObject(MemberSerialization.OptIn)]
    public class ItemWeapon : Item
    {
        public List<Projectile2> projectiles = new List<Projectile2>();
        [JsonProperty]
        private WeaponType wepType;

        public ShootFunction function;
        public ItemWeapon(int type, WeaponType wepType) : base()
        {
            this.type = type;
            this.lType = LimitType.Weapon;
            this.wepType = wepType;

            itemType = 0;
            SetDefaults();
        }

        #region Defaults
        protected override void SetDefaults()
        {
            #region Elemental Orbs
            if (wepType == WeaponType.EleOrb)
            {
                equippableBy.Add(Class.Archer);

                Color projOuterColor, projInnerColor;
                projOuterColor = projInnerColor = Color.DarkGray;
                if (type == 0)
                {
                    descriptor.name = "Marble orb";
                    descriptor.description = "An orb simply cut from a chunk of marble and infused with magical properties.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("eleOrbs"), new Rectangle(0, 0, 8, 8));
                    rarity = 1;

                    damageMin = 5;
                    damageMax = 55;

                    projOuterColor = Color.White;
                }
                else if (type == 1)
                {
                    descriptor.name = "Garnet Orb";
                    descriptor.description = "An orb cut from an incredibly large but cheap garnet. It's fairly crudely made.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("eleOrbs"), new Rectangle(8, 0, 8, 8));
                    rarity = 2;

                    damageMin = 20;
                    damageMax = 75;

                    projOuterColor = Color.IndianRed;
                }
                else if (type == 2)
                {
                    descriptor.name = "Jade Orb";
                    descriptor.description = "An orb made from many crystals of jade fused together.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("eleOrbs"), new Rectangle(16, 0, 8, 8));
                    rarity = 3;

                    damageMin = 35;
                    damageMax = 95;

                    projOuterColor = Color.GreenYellow;
                }
                else if (type == 3)
                {
                    descriptor.name = "Peridot Orb";
                    descriptor.description = "An orb made from many crystals of peridot fused together.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("eleOrbs"), new Rectangle(24, 0, 8, 8));
                    rarity = 4;

                    damageMin = 50;
                    damageMax = 115;

                    projOuterColor = Color.DarkOliveGreen;
                }
                else if (type == 4)
                {
                    descriptor.name = "Lapis Lazuli Orb";
                    descriptor.description = "An orb made from a giant chunk of lapis.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("eleOrbs"), new Rectangle(32, 0, 8, 8));
                    rarity = 5;

                    damageMin = 65;
                    damageMax = 135;

                    projOuterColor = Color.Blue;
                }
                else if (type == 5)
                {
                    descriptor.name = "Gold Orb";
                    descriptor.description = "An orb made from pure gold.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("eleOrbs"), new Rectangle(40, 0, 8, 8));
                    rarity = 6;

                    damageMin = 80;
                    damageMax = 155;

                    projOuterColor = Color.Gold;
                }
                else if (type == 6)
                {
                    descriptor.name = "Pearl Orb";
                    descriptor.description = "An orb made from a giant pearl grown inside a magically enhanced mollusk for the express purpose of making an orb.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("eleOrbs"), new Rectangle(48, 0, 8, 8));
                    rarity = 7;

                    damageMin = 95;
                    damageMax = 175;

                    projOuterColor = Color.Pink;
                }
                else if (type == 7)
                {
                    descriptor.name = "Bloodstone Orb";
                    descriptor.description = "An orb cut with magic from bloodstone. It's incredibly hard to use as the bloodstone's magical properties can only be controlled easily by the most powerful of magus.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("eleOrbs"), new Rectangle(56, 0, 8, 8));
                    rarity = 8;

                    damageMin = 110;
                    damageMax = 195;

                    projOuterColor = Color.Red;
                }
                else if (type == 8)
                {
                    descriptor.name = "Blackstone Emerald Orb";
                    descriptor.description = "An orb made from blackstone, an incredibly rare, naturally occuring sphere of stone. Noone knows how these got to be there, but they seem to hold magic incredibly well. This orb is encrusted with Emeralds.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("eleOrbs"), new Rectangle(64, 0, 8, 8));
                    rarity = 9;

                    damageMin = 125;
                    damageMax = 215;

                    projOuterColor = new Color(50, 50, 50);
                    projInnerColor = Color.Green;
                }
                else if (type == 9)
                {
                    descriptor.name = "Blackstone Ruby Orb";
                    descriptor.description = "An orb made from blackstone, an incredibly rare, naturally occuring sphere of stone. Noone knows how these got to be there, but they seem to hold magic incredibly well. This orb is encrusted with Rubies.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("eleOrbs"), new Rectangle(72, 0, 8, 8));
                    rarity = 10;

                    damageMin = 140;
                    damageMax = 235;

                    projOuterColor = new Color(50, 50, 50);
                    projInnerColor = Color.Red;
                }
                else if (type == 10)
                {
                    descriptor.name = "Orb of Sapphirine Beauty";
                    descriptor.description = "An orb of incredible beauty. It was carefully cut from a huge diamond, then infused with sapphires.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("eleOrbs"), new Rectangle(80, 0, 8, 8));
                    rarity = 11;

                    damageMin = 155;
                    damageMax = 255;

                    projOuterColor = Color.DarkGray;
                    projInnerColor = Color.Teal;
                }
                else if (type == 11)
                {
                    descriptor.name = "Orb of Crystalline Beauty";
                    descriptor.description = "An orb of incredible beauty. It was carefully cut from a huge diamond, then infused with gems made of pure magic.";

                    texture = Assets.GetTexFromSource(Assets.GetTexture("eleOrbs"), new Rectangle(88, 0, 8, 8));
                    rarity = 12;

                    damageMin = 170;
                    damageMax = 275;

                    projOuterColor = Color.DarkGray;
                    projInnerColor = Color.Blue;
                }
                Projectile2 p1 = new Projectile2(Assets.GetTexFromSource("projectilesFull", 2, 0), projOuterColor, 3, Vector2.Zero, Vector2.Zero, new Vector2(8, 24), 0, 222.5f, 8, 512, 20).SetWavy(48, 0.08f, false);
                    //new Projectile(Assets.GetTexFromSource("projectilesFull", 2, 0), projOuterColor, Vector2.Zero, new Vector2(8, 8), true, 3, 0, 8, 140, 512, 20).SetWavy(48, 0.03f, false);//, false, false, true, 7, 300, false);
                Projectile2 p2 = new Projectile2(Assets.GetTexFromSource("projectilesFull", 2, 0), projOuterColor, 3, Vector2.Zero, Vector2.Zero, new Vector2(8, 24), 0, 237.5f, 8, 512, 20).SetWavy(48, 0.08f, true);
                Projectile2 p3 = new Projectile2(Assets.GetTexFromSource("projectilesFull", 2, 0), projInnerColor, 3, Vector2.Zero, Vector2.Zero, new Vector2(8, 24), 0, 225, 8, 512, 20);
                projectiles.Add(p1);
                projectiles.Add(p2);
                projectiles.Add(p3);

                function = ShootEleOrb;
            }
            #endregion
            #region Bows
            if (wepType == WeaponType.Bow)
            {
                Color color = Color.White;
                equippableBy.Add(Class.Archer);
                if (type == 0)
                {
                    descriptor.name = "Wooden Bow";
                    descriptor.description = "A simple bow made out of hard, but flexible wood.";

                    texture = Assets.GetTexture("bowWood");
                    rarity = 1;

                    damageMin = 30;
                    damageMax = 50;
                    color = Color.Gray;
                }
                if (type == 1)
                {
                    descriptor.name = "Pinksteel Bow";
                    descriptor.description = "Crafted from pinksteel, this bow is surpsingly flexible for being metal.";

                    texture = Assets.GetTexture("bowPinksteel");
                    rarity = 2;

                    damageMin = 35;
                    damageMax = 60;
                    color = Color.Pink;
                }
                if (type == 2)
                {
                    descriptor.name = "Blackwood bow";
                    descriptor.description = "A bow made from extremely flexible, but fragile blackwood. Because it's so easy to shatter, only experts can handle it.";

                    texture = Assets.GetTexture("bowBlackwood");
                    rarity = 3;

                    damageMin = 40;
                    damageMax = 70;
                    color = Color.Black;
                }
                if (type == 3)
                {
                    descriptor.name = "Bone Bow";
                    descriptor.description = "A bow made from many of the deceased, taken from their tombs unwillingly. The souls held in the bones hate the owner with a passion and will try to kill him.";
                        //redo this description it's terrible
                    texture = Assets.GetTexture("bowBone");
                    rarity = 4;

                    damageMin = 50;
                    damageMax = 80;
                    color = Color.White;
                }
                if (type == 4)
                {
                    descriptor.name = "Stone Bow";
                    descriptor.description = "A bow made from magically enhanced stone. It's still barely bendable and extremely heavy.";

                    texture = Assets.GetTexture("bowStone");
                    rarity = 5;

                    damageMin = 60;
                    damageMax = 90;
                    color = Color.DimGray;
                }
                if (type == 5)
                {
                    descriptor.name = "Reinforced Wooden Bow";
                    descriptor.description = "A wooden bow magically reinforced and thickened. It takes an incredible amount of strength to bend the thing.";

                    texture = Assets.GetTexture("bowThick");
                    rarity = 6;

                    damageMin = 70;
                    damageMax = 100;
                    color = Color.Gray;
                }
                if (type == 6)
                {
                    descriptor.name = "Firestone Bow";
                    descriptor.description = "This bow was made by taking heated rock and freezing it in time. It will stay blazing hot until its magic runs out.";

                    texture = Assets.GetTexture("bowFirestone");
                    rarity = 7;

                    damageMin = 75;
                    damageMax = 105;
                    color = Color.Red;
                }
                if (type == 7)
                {
                    descriptor.name = "Copper Bow";
                    descriptor.description = "A bow made from copper.";

                    texture = Assets.GetTexture("bowCopper");
                    rarity = 8;

                    damageMin = 80;
                    damageMax = 110;
                    color = Color.DarkOliveGreen;
                }
                if (type == 8)
                {
                    descriptor.name = "Brass bow";
                    descriptor.description = "A bow made from brass, with some ornimental copper leaf attached.";

                    texture = Assets.GetTexture("bowBrass");
                    rarity = 9;

                    damageMin = 85;
                    damageMax = 115;
                    color = Color.Yellow;
                }
                if (type == 9)
                {
                    descriptor.name = "Sapphire Bow";
                    descriptor.description = "A bow made of an unknown metal and encrusted with many sapphire jewels.";

                    texture = Assets.GetTexture("bowSapphire");
                    rarity = 10;

                    damageMin = 90;
                    damageMax = 120;
                    color = Color.Blue;
                }
                if (type == 10)
                {
                    descriptor.name = "Emerald Bow";
                    descriptor.description = "A bow made of an unkown metal and encrusted with emerald jewels. They almost seem to glow in the dark...";

                    texture = Assets.GetTexture("bowEmerald");
                    rarity = 11;

                    damageMin = 100;
                    damageMax = 135;
                    color = Color.LimeGreen;
                }
                if (type == 11)
                {
                    descriptor.name = "Bow of the Jewelcrafter";
                    descriptor.description = "A stunning bow encrusted with giant white diamonds. Its maker is unknown but is clearly unmatched in artistic skill.";

                    texture = Assets.GetTexture("bowJewelcrafter");
                    rarity = 12;

                    damageMin = 110;
                    damageMax = 150;
                    color = Color.White;
                }
                Projectile2 p1 = new Projectile2(Assets.GetTexture("arrowtail"), color, 3.7f, Vector2.Zero, Vector2.Zero, new Vector2(12, 24), 0, 225, 8, 384, 20);
                //Projectile.Create(Assets.GetTexture("arrowtail"), color, Vector2.Zero, new Vector2(16, 32), true, 3.7f, 0, 8f, 135, 384, 20);
                p1.noColorTex = Assets.GetTexture("arrow");
                p1.piercing = true;
                projectiles.Add(p1);

                function = ShootBow;
            }
            #endregion
            #region Swords
            if (wepType == WeaponType.Sword)
            {
                Color color = Color.Wheat;
                if (type == 0)
                {
                    equippableBy.Add(Class.Archer);
                    descriptor.name = "Iron Sword";
                    descriptor.description = "A sword made out of iron. Simple, but sturdy.";

                    texture = Assets.GetTexture("swordIron");
                    rarity = 1;

                    damageMin = 160;
                    damageMax = 225;
                    color = Color.Gray;
                }
                if (type == 1)
                {
                    equippableBy.Add(Class.Archer);
                    descriptor.name = "Bluesteel Sword";
                    descriptor.description = "A sword made out of a bluish steel. It's very hard and sharp.";

                    texture = Assets.GetTexture("swordBluesteel");
                    rarity = 2;

                    damageMin = 170;
                    damageMax = 230;

                    color = Color.DarkSlateBlue;
                }
                if (type == 2)
                {
                    equippableBy.Add(Class.Archer);
                    descriptor.name = "Elvenwood Sword";
                    descriptor.description = "Made by the elves to the north, this sword made of simple wood and infused with elvish magics, is incredibly sharp.";

                    texture = Assets.GetTexture("swordElvenwood");
                    rarity = 3;

                    damageMin = 180;
                    damageMax = 240;

                    color = Color.MediumPurple;
                }
                if (type == 3)
                {
                    equippableBy.Add(Class.Archer);
                    descriptor.name = "Bloodwood Sword";
                    descriptor.description = "Legend tells of trees that bleed when cut down. A sword made from one of these trees could make its enemies weep for eternity.";

                    texture = Assets.GetTexture("swordBloodwood");
                    rarity = 4;

                    damageMin = 190;
                    damageMax = 245;

                    color = Color.DarkRed;
                }
                if (type == 4)
                {
                    equippableBy.Add(Class.Archer);
                    descriptor.name = "Gold Sword";
                    descriptor.description = "A sword made of glorious gold.";

                    texture = Assets.GetTexture("swordGold");
                    rarity = 5;

                    damageMin = 200;
                    damageMax = 250;

                    color = Color.Gold;
                }
                if (type == 5)
                {
                    equippableBy.Add(Class.Archer);
                    descriptor.name = "Ruby Sword";
                    descriptor.description = "A sword made of majestic rubies.\nThey glow with a dim light.";

                    texture = Assets.GetTexture("swordRuby");
                    rarity = 6;

                    damageMin = 210;
                    damageMax = 255;

                    color = Color.Red;
                }
                if (type == 6)
                {
                    equippableBy.Add(Class.Archer);
                    descriptor.name = "Sapphire Sword";
                    descriptor.description = "A sword made of sapphires.\nThe blood spilled with this contrasts greatly with its beauty.";

                    damageMin = 220;
                    damageMax = 265;
                    texture = Assets.GetTexture("swordSapphire");
                    rarity = 7;

                    color = Color.Blue;
                }
                if (type == 7)
                {
                    equippableBy.Add(Class.Archer);
                    descriptor.name = "Diamond sword";
                    descriptor.description = "A sword made of a huge chunk of diamond. It's doubtless the most expensive thing you've ever seen.";

                    damageMin = 230;
                    damageMax = 270;
                    texture = Assets.GetTexture("swordDiamond");
                    rarity = 8;

                    color = Color.CornflowerBlue;
                }
                if (type == 8)
                {
                    equippableBy.Add(Class.Archer);
                    descriptor.name = "Crystal Sword";
                    descriptor.description = "An almost completely see-through sword made of tiny crystals melded together to produce a seamless rock.";

                    damageMin = 240;
                    damageMax = 275;
                    texture = Assets.GetTexture("swordCrystal");
                    rarity = 9;

                    color = Color.Blue;
                }
                if (type == 9)
                {
                    equippableBy.Add(Class.Archer);
                    descriptor.name = "Sword of the Jealous Arbiter";
                    descriptor.description = "This sword was once weielded by the self-proclaimed arbiter of the capitol. His misplaced arbitry caused the downfall of the land.";

                    damageMin = 250;
                    damageMax = 280;
                    texture = Assets.GetTexture("swordArbiter");
                    rarity = 10;

                    color = Color.GreenYellow;
                }
                if (type == 10)
                {
                    equippableBy.Add(Class.Archer);
                    descriptor.name = "Sword of the Ice Binder";
                    descriptor.description = "A sword made of solid opaque ice. The magics infused in it make any cut freeze over instantly, for better or worse.";

                    damageMin = 260;
                    damageMax = 285;
                    texture = Assets.GetTexture("swordIcebinder");
                    rarity = 11;

                    color = Color.White;    
                }
                if (type == 11)
                {
                    equippableBy.Add(Class.Archer);
                    descriptor.name = "Sword of Demon's Bane";
                    descriptor.description = "A sword that carries thousands of souls of slain demons. The warrior who once weilded this was said to have gone mad. You can almost hear the demons whispering to you...";

                    damageMin = 270;
                    damageMax = 290;
                    texture = Assets.GetTexture("swordDemonbane");
                    rarity = 12;

                    color = Color.Red;
                }
                Projectile2 p1 = new Projectile2(Assets.GetTexFromSource("projectilesFull", 0, 1), color, 3.5f, Vector2.Zero, Vector2.Zero, new Vector2(12, 24), 0, 180, 4.4f, 112, 20);
                    //Projectile.Create(Assets.GetTexFromSource("projectilesFull", 0, 1), color, Vector2.Zero, new Vector2(16, 32), true, 3.5f, 0, 4.4f, -45, 128, 20);
                projectiles.Add(p1);
                function = ShootSword;
            }
            #endregion
            AddStats();
            if (equippableBy.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Class equipBy in equippableBy)
                    sb.Append(equipBy.ToString() + ", ");
                sb.Remove(sb.Length - 2, 2);
                descriptor.classuse = sb.ToString();
            }

            descriptor.rarity = rarity.ToString();
            descriptor.type = lType.ToString();

            
            descriptor.SetupPlate();
        }
        #endregion

        public void ShootEleOrb(World world, Player player)
        {
            float angle = VectorHelper.FindAngleBetweenTwoPoints(player.center, VectorHelper.ConvertScreenToWorldCoords(Main.mouse.currentState.Position.ToVector2()));
            int randDamg = Main.rand.Next(damageMin, damageMax);

            for (int i = 0; i < projectiles.Count; i++)
            {
                Projectile2 p1 = null;
                if (i == 0)
                {
                    p1 = world.CreateProjectile(projectiles[i].Copy(player.center, angle + 2.5f));
                }
                else if (i == 1)
                {
                    p1 = world.CreateProjectile(projectiles[i].Copy(player.center, angle  - 2.5f));
                }
                else p1 = world.CreateProjectile(projectiles[i].Copy(player.center, angle));
                p1.damage = (int)((randDamg + (int)(((randDamg * 1.2) / 75) * attack)) * player.damageMult);
                p1.friendly = true;
            }
        }

        public void ShootBow(World world, Player player)
        {
            float angle = VectorHelper.FindAngleBetweenTwoPoints(player.center, VectorHelper.ConvertScreenToWorldCoords(Main.mouse.currentState.Position.ToVector2()));
            int randDamg = Main.rand.Next(damageMin, damageMax);
            if (rarity < 10)
            {
                Projectile2 p = world.CreateProjectile(projectiles[0].Copy(player.center, angle));
                p.damage = (int)((randDamg + (int)(((randDamg * 1.2) / 75) * attack)) * player.damageMult);
                p.friendly = true;
            }
            else
            {
                //for (float i = 0; i <= 360; i += 360 / 15)
                //world.testproj.Add(new Projectile2(Assets.GetTexFromSource("projectilesFull", 3, 0), player.center, Vector2.Zero, new Vector2(8, 32), i, 12, 512).SetBoomerang());
                for (float i = -5; i <= 5; i += 10)
                {
                    Projectile2 p = world.CreateProjectile(projectiles[0].Copy(player.center, angle + i));
                    p.damage = (int)((randDamg + (int)(((randDamg * 1.2) / 75) * attack)) * player.damageMult);
                    p.friendly = true;
                }
            }
        }

        public void ShootSword(World world, Player player)
        {
            float angle = VectorHelper.FindAngleBetweenTwoPoints(player.center, VectorHelper.ConvertScreenToWorldCoords(Main.mouse.currentState.Position.ToVector2()));
            int randDamg = Main.rand.Next(damageMin, damageMax);
            Projectile2 p = world.CreateProjectile(projectiles[0].Copy(player.center, angle));
            p.damage = (int)((randDamg + (int)(((randDamg * 1.2) / 75) * attack)) * player.damageMult);
            p.friendly = true;
        }
    }
}
