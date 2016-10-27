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
    public enum AbilityType
    {
        Quiver,
        IcecSpell,
        Pendant
    }

    public delegate void AbilityFunction(World world, Player player);

    [JsonObject(MemberSerialization.OptIn)]
    public class ItemAbility : Item
    {
        public int cost;

        public Projectile2 projectile;

        public AbilityFunction function;

        [JsonProperty]
        private AbilityType abilityType;
        public ItemAbility(int type, AbilityType abilityType) : base()
        {
            this.type = type;
            this.lType = LimitType.Ability;
            this.abilityType = abilityType;
            this.lClass = Class.Archer;

            itemType = 1;
            SetDefaults();
        }

        protected override void SetDefaults()
        {
            if (abilityType == AbilityType.Pendant)
            {
                projectile = new Projectile2(Assets.GetTexFromSource("projectilesFull", 8, 0), Color.White, 6, Vector2.Zero, new Vector2(0, 16), new Vector2(32, 8), 0, -135, 10, 128, 20);
                if (type == 0)
                {
                    descriptor.name = "Ruby Pendant";
                    descriptor.description = "A pendant with a small ruby crystal at the center.";
                    texture = Assets.GetTexFromSource(Assets.GetTexture("pendants"), new Rectangle(0, 0, 8, 8));
                    rarity = 6;

                    cost = 100;
                    damage = 20;
                }

                descriptor.abilitydescription = "Shoots paralyzing projectiles at the mouse's cursor.";
                descriptor.rarity = rarity.ToString();

                function = AbilityTimePendant;
            }
            else if (abilityType == AbilityType.IcecSpell)
            {
                equippableBy.Add(Class.IceMage);

                if (type == 0)
                {
                    descriptor.name = "Ice spell";
                    descriptor.description = "A beginner ice mage's ability.";

                    texture = Assets.GetTexture("quiverSilkbound");
                    rarity = 6;

                    cost = 30;
                    damage = 700;
                    dexterity = 20;
                    wisdom = 75;
                }
                projectile = new Projectile2(Assets.GetTexFromSource("projectilesFull", 4, 0), Color.White, 3.5f, Vector2.Zero, Vector2.Zero, new Vector2(10), 0, 0, 1.2f, 512, 20);
                    //new Projectile(Assets.GetTexFromSource("projectilesFull", 4, 0), Color.White, Vector2.Zero, new Vector2(10, 10), true, 3.5f, 0, 1.2f, 135, 512, 20);
                projectile.armorPiercing = true;

                descriptor.abilitydescription = "Shoots A large, slow moving ice ball that does heavy armor piercing damage, and explodes into many icicles that slow.";
                descriptor.rarity = rarity.ToString();

                function = AbilityIcicle;
            }
            else if (abilityType == AbilityType.Quiver)
            {
                projectile = new Projectile2(Assets.GetTexFromSource("projectilesFull", 1, 2), Color.White, 3.5f, Vector2.Zero, Vector2.Zero, new Vector2(16, 32), 0, 225, 8, 512, 20);
                    //new Projectile(Assets.GetTexFromSource("projectilesFull", 1, 2), Color.White, Vector2.Zero, new Vector2(16, 32), true, 3.5f, 0, 8f, 135, 512, 20);
                equippableBy.Add(Class.Archer);
                descriptor.abilitydescription = "Shoots three projectiles that slow and deal heavy damage.";

                if (type == 0)
                {
                    descriptor.name = "Silkbound Quiver";
                    descriptor.description = "A nicely woven quiver, wrapped with ties of silk.";

                    texture = Assets.GetTexture("quiverSilkbound");
                    rarity = 1;
                    descriptor.rarity = rarity.ToString();

                    cost = 60;
                    damage = 100;
                }
                if (type == 1)
                {
                    descriptor.name = "Bloodstained Quiver";
                    descriptor.description = "A quiver stained with blood from an unknown source. Just holding it makes you feel odd.";

                    texture = Assets.GetTexture("quiverBloodstained");
                    rarity = 2;

                    cost = 68;
                    damage = 175;
                    projectile.color = Color.Red;
                }
                if (type == 2)
                {
                    descriptor.name = "Redwood Quiver";
                    descriptor.description = "A quiver made from a dark brownish red wood. While sturdy, it's very stiff and uncomfortable.";

                    texture = Assets.GetTexture("quiverRedwood");
                    rarity = 3;
                    descriptor.rarity = rarity.ToString();

                    cost = 76;
                    damage = 250;
                    projectile.color = Color.Red;
                }
                if (type == 3)
                {
                    descriptor.name = "Pinksteel Quiver";
                    descriptor.description = "A quiver made of a pinkish, malliable metal.";

                    texture = Assets.GetTexture("quiverPinksteel");
                    rarity = 4;
                    descriptor.rarity = rarity.ToString();

                    cost = 84;
                    damage = 325;
                    projectile.color = Color.HotPink;
                }
                if (type == 4)
                {
                    descriptor.name = "Ebonwood Quiver";
                    descriptor.description = "A quiver made of dark wood. It's so light you can't feel it on your back.";

                    texture = Assets.GetTexture("quiverEbonwood");
                    rarity = 5;
                    descriptor.rarity = rarity.ToString();

                    cost = 92;
                    damage = 400;
                    projectile.color = Color.MediumPurple;
                }
                if (type == 5)
                {
                    descriptor.name = "Bloodstone Quiver";
                    descriptor.description = "An incredibly heavy quiver made of a strange, bright red stone. You can almost hear whispers of something talking to you while wearing it...";

                    texture = Assets.GetTexture("quiverBloodstone");
                    rarity = 6;
                    descriptor.rarity = rarity.ToString();

                    attack = 4;
                    dexterity = 5;
                    mana = 30;

                    cost = 100;
                    damage = 475;
                    projectile.color = Color.DarkRed;
                }
                projectile.noColorTex = Assets.GetTexFromSource("projectilesFull", 0, 2);
                projectile.GiveBuff(new Buff("Slowed", rarity * 20, Buff.EffectSlowed));

                descriptor.classuse = "Archer";

                function = AbilityQuiver;
            }
            AddStats();
            descriptor.rarity = rarity.ToString();
            descriptor.type = lType.ToString();
            descriptor.SetupPlate();
            rarity *= 2;

        }

        private void AbilityIcicle(World world, Player player)
        {
            float angle = VectorHelper.FindAngleBetweenTwoPoints(player.center, VectorHelper.ConvertScreenToWorldCoords(Main.mouse.state.Position.ToVector2()));
            if (player.mana - cost >= 0)
            {
                int r = Main.rand.Next(0, 2);
                int s = r == 0 ? -7 : 7;
                Projectile2 p = world.CreateProjectile(projectile.Copy(player.center, angle).SetSpin(s));
                for (float i = 1; i <= 15; i++)
                {
                    Projectile2 dp = projectile.Copy(player.position, (360 / 15) * i);
                    dp.friendly = true;
                    dp.damage = damage / 4;
                    dp.armorPiercing = true;
                    dp.texture = Assets.GetTexFromSource("projectilesFull", 5, 0);
                    dp.spin = false;
                    dp.GiveBuff(new Buff("Slowed", 10 * (rarity + 1), Buff.EffectSlowed));
                    p.deathProjectiles.Add(dp);
                }
                p.damage = damage;
                p.friendly = true;

                player.mana -= cost;
            }
        }

        private void AbilityQuiver(World world, Player player)
        {
            float angle = VectorHelper.FindAngleBetweenTwoPoints(player.center, VectorHelper.ConvertScreenToWorldCoords(Main.mouse.state.Position.ToVector2()));
            if (player.mana - cost >= 0)
            {
                for (int i = -5; i <= 5; i += 5)
                {
                    Projectile2 p = world.CreateProjectile(projectile.Copy(player.center, angle + i))
                        .SetParticleTrail(new Particle(Vector2.Zero, Vector2.Zero, projectile.color, 4, 10), 1.5f, 3, 1, new Vector2(-.5f, .5f));
                    p.damage = damage;
                    p.friendly = true;
                }
                player.mana -= cost;
            }
        }

        private void AbilityTimePendant(World world, Player player)
        {
            Vector2 mousepos = VectorHelper.ConvertScreenToWorldCoords(Main.mouse.state.Position.ToVector2());

            if (player.mana - cost >= 0)
            {
                for (float i = 0; i < 360; i += 360 / 20)
                {
                    Projectile2 p = world.CreateProjectile(projectile.Copy(mousepos, i));//new Projectile(Assets.GetTexture("shield"), Color.White, mousepos, new Vector2(16, 8), true, 6, i, 8, 135, 128, 20, true);

                    p.armorPiercing = true;
                    p.damage = damage;
                    p.friendly = true;

                    p.GiveBuff(new Buff("Paralyzed", (rarity / 2) * 30, Buff.EffectParalyzed), 
                        new Buff("Stunned", (rarity / 2) * 30, Buff.EffectStunned));
                }
                player.mana -= cost;
            }
        }

        private void AbilityHelm(World world, Player player)
        {
            if (player.mana - cost >= 0)
            {
                Buff givebuff = new Buff("Frenzied", 300, Buff.EffectFrenzy);
                player.buffs.Add(givebuff);
                givebuff.entity = player;
                player.mana -= cost;
            }
        }
    }
}
