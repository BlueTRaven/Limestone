/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Limestone.Entities;
using Limestone.Utility;

namespace Limestone.Buffs
{
    public delegate void BuffFunction(Buff buff, EntityLiving entity, World world);
    public class Buff
    {
        public int duration;
        public int maxDuration;
        float[] storage;

        public bool firstTick = true, lastTick = false, active = true;

        public BuffFunction function;

        public string name = "NONE";

        public EntityLiving entity;
        public Buff(string displayText, int duration, BuffFunction function)
        {
            this.duration = duration;
            maxDuration = duration;
            this.function = function;
            this.name = displayText;
        }

        public void RunEffect(World world)
        {
            if (active)
            {
                duration--;

                if (duration <= 0)
                    lastTick = true;
                if (entity != null && duration >= 0)
                    function(this, entity, world);
                if (duration < 0)
                    active = false;
            }
        }

        public static void EffectArmorBreak(Buff buff, EntityLiving entity, World world)
        {
            buff.name = "Armor Break";
            if (entity.tType == EntityType.Player)
            {
                Player player = (Player)entity;
                player.defense = 0;
            }
        }
        public static void EffectWeakness(Buff buff, EntityLiving entity, World world)
        {//player only
            buff.name = "Weak";
            if (entity is Player)
            {
                Player player = (Player)entity;
                player.damageMult = .5f;
                buff.firstTick = false;
            }
            else
            {
                Logger.Log("EffectWeakness was used on an entity other than the player.", true);
            }
        }

        public static void EffectBleeding(Buff buff, EntityLiving entity, World world)
        {
            buff.name = "Bleeding";
            if (buff.firstTick)
            {
                buff.storage = new float[] { 0 };
                buff.firstTick = false;
            }
            buff.storage[0]--;

            if (buff.storage[0] <= 0)
            {
                entity.TakeDamage(10, null, world);
                buff.storage[0] = 30;
            }
        }

        public static void EffectSlowed(Buff buff, EntityLiving entity, World world)
        {
            if (entity is Player)
            {
                ((Player)entity).speedMult = 1f / 3f;
            }
            else
            {
                entity.speed = 2f / 3f;
            }
        }

        public static void EffectParalyzed(Buff buff, EntityLiving entity, World world)
        {
            if (entity is Player)
            {
                ((Player)entity).speedMult = 0;
            }
            else
                entity.speed = 0;
        }

        public static void EffectStunned(Buff buff, EntityLiving entity, World world)
        {
            entity.stunned = true;
            if (buff.lastTick)
            {
                entity.stunned = false;
            }
        }

        public static void EffectFrenzy(Buff buff, EntityLiving entity, World world)
        {   //buff: player only
            if (entity is Player)
            {
                entity.speed = ((Player)entity).maxSpeed * 1.5f;
            }
        }

        public Buff Copy()
        {
            return new Buff(name, duration, function);
        }
    }
}
*/