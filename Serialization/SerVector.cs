using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Limestone.Serialization
{
    [Serializable]
    public class SerVector : ISerializable
    {
        float x, y;

        protected SerVector(SerializationInfo info, StreamingContext context)
        {
            x = info.GetInt32("x");
            y = info.GetInt32("y");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("x", x);
            info.AddValue("y", y);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }
    }
}
