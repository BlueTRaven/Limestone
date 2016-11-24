using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Limestone.Tiles;
using Limestone.Entities;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Limestone.Serialization
{
    [Serializable]
    public class SerWorld
    {
        public Rectangle bounds;

        public List<Tile> serTiles = new List<Tile>();

        public List<EntitySpawner> spawners = new List<EntitySpawner>();

        public int worldIndex;

        /*protected SerWorld(SerializationInfo info, StreamingContext context)
        {
            bounds = ((SerRect)info.GetValue("bounds", typeof(SerRect))).ToRectangle();
            spawners = ((EnemySpawner[])info.GetValue("spawners", typeof(EnemySpawner[]))).ToList();
            serTiles = ((Tile[])info.GetValue("tiles", typeof(Tile[]))).ToList();
        }*/

        public void GetObjectData(SerializationInfo info, StreamingContext context){ /*NOOP Used for serialization only*/ }
    }
}
