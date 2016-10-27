using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone
{
    public enum Biomes
    {
        Sea,
        Beach,
        LowLands,
        MidLands,
        HighLands,
        AncientLands,
        Rock
    }

    public static class Biome
    {
        public static int GetSpawners(Biomes biome)
        {
            switch(biome)
            {
                case Biomes.Sea:
                    return 0;
                default:
                    return 0;
            }
        }
        public static Texture2D GetBiomeGrassTexture(Biomes biome)
        {
            switch(biome)
            {
                case Biomes.Sea:
                    return Assets.GetTexture("water1");
                case Biomes.Beach:
                    return Assets.GetTexture("beach1");
                case Biomes.LowLands:
                    return Assets.GetTexture("grassLowlands1");
                case Biomes.MidLands:
                    return Assets.GetTexture("grassMidlands1");
                case Biomes.HighLands:
                    return Assets.GetTexture("grassHighlands1");
                case Biomes.AncientLands:
                    return Assets.GetTexture("grassAncientlands1");
                default:
                    return Assets.GetTexture("water1");
            }
        }

        public static Texture2D GetBiomeRockTexture(Biomes biome)
        {
            int r = Main.rand.Next(0, 100);

            switch (biome)
            {
                case Biomes.Sea:
                    return null;
                case Biomes.Beach:
                    return null;
                case Biomes.LowLands:
                    return Assets.GetTexture("rocksLowlands1");
                case Biomes.MidLands:
                    if (r < 50)
                        return Assets.GetTexture("rocksMidlands1");
                    else
                        return Assets.GetTexture("rocksMidlands2");
                case Biomes.HighLands:
                    return Assets.GetTexture("rocksHighlands1");
                case Biomes.AncientLands:
                    if (r < 80)
                        return Assets.GetTexture("rocksAncientlands1");
                    else
                        return Assets.GetTexture("rocksAncientlands3");
                default:
                    return null;
            }
        }

        public static int GetSpawnChance(Biomes biome, int enemyindex)
        {
            if (biome == Biomes.Sea)
            {
                return 1;
            }
            else if (biome == Biomes.Beach)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
