using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limestone.Buff
{
    public class Buffs
    {
        int type, duration;
        int[] storage;

        public bool firstTick = true, lastTick = false, active = true;

        public BuffFunction function;

        public  Player player;
        public Buffs(int type, int duration)
        {
            this.type = type;
            this.duration = duration;
        }

        public void RunEffect()
        {
            if (active)
            {
                duration--;

                if (duration == 0)
                    lastTick = true;
                function(player);
            }
        }

        public void EffectWeakness(Player player)
        {
            if (firstTick)
            {
                storage = new int[] { player.damage };
                player.damage /= 2;
                firstTick = false;
            }

            if (lastTick)
            {
                player.damage = storage[0];
                active = false;
            }
        }

        public void EffectBleeding(Player player)
        {
            player.TakeDamage(2, null);

            if (lastTick)
            {
                active = false;
            }
        }
    }
}
