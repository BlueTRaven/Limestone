using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Tiles;
using Limestone.Entities;
using Limestone.Entities.Enemies;
using Limestone.Items;
using Limestone.Generation;

namespace Limestone
{
    public static class SaveHelper
    {
        public class ItemConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return typeof(Item).IsAssignableFrom(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JObject item = JObject.Load(reader);

                if (item["itemType"].Value<int>() == 0)
                {
                    return item.ToObject<ItemWeapon>();
                }
                else if (item["itemType"].Value<int>() == 1)
                {
                    return item.ToObject<ItemAbility>();
                }
                else
                {
                    return item.ToObject<ItemWeapon>();
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
            }
        }

        public static void Save(object obj, string saveName)
        {
            using (StreamWriter file = File.CreateText(@"" + saveName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.ReferenceLoopHandling = ReferenceLoopHandling.Error;
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, obj);
            }
        }

        public static object Load(string loadName, JsonConverter converter)
        {
            using (StreamReader sr = new StreamReader(@"" + loadName + ".json"))
            {
                string content = sr.ReadToEnd();
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(content, converter);

                return obj;
            }
        }

        public static Player LoadPlayer(string fileName)
        {
            using (StreamReader sr = new StreamReader(@"" + fileName + ".json"))
            {
                string content = sr.ReadToEnd();
                Player obj = JsonConvert.DeserializeObject<Player>(content,  new ItemConverter());
                return obj;
            }
        }
    }
}
