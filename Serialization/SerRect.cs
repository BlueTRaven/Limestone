using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Limestone.Serialization
{
    [Serializable]
    public class SerRect : ISerializable
    {
        float x, y, width, height;

        public SerRect(SerializationInfo info, StreamingContext context)
        {
            x = info.GetInt32("x");
            y = info.GetInt32("y");
            width = info.GetInt32("width");
            height = info.GetInt32("height");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("x", x);
            info.AddValue("y", y);
            info.AddValue("width", width);
            info.AddValue("height", height);
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }
    }
}
