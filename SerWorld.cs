using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Limestone.Tiles;
using Limestone.Entities;

namespace Limestone
{
    public class SerWorld
    {
        public Rectangle bounds;

        public List<Tile> serTiles = new List<Tile>();

        public List<EnemySpawner> spawners = new List<EnemySpawner>();
    }
}
