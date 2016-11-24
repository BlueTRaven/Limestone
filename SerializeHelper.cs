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
using Limestone.Serialization;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;

namespace Limestone
{
    public static class SerializeHelper
    {
        public class TileConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return typeof(Tile).IsAssignableFrom(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JObject item = JObject.Load(reader);

                if (item["tileType"].Value<int>() == 0)
                {
                    return item.ToObject<TileFloor>();
                }
                else if (item["tileType"].Value<int>() == 1)
                {
                    return item.ToObject<TileWall>();
                }
                else
                {
                    Console.WriteLine("Did not have a correct tile type!\nType: " + item["tileType"].Value<int>() + "\n Falling back to TileEmpty!");
                    return item.ToObject<TileEmpty>();
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
            }
        }

        public static void Save(object obj, string saveName)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Include;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(saveName))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, obj);
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

        public static PlayerSave LoadSave(string savename)
        {
            PlayerSave save = null;
            try
            {
                using (StreamReader sr = new StreamReader(savename))
                {
                    string content = sr.ReadToEnd();
                    save = JsonConvert.DeserializeObject<PlayerSave>(content);

                    Console.WriteLine("Loading save " + savename + "... \nLocation is: " + save.map);
                }
            }
            catch
            {
                Console.WriteLine("There was no player save that could be loaded! Creating new save...");
                save = new PlayerSave();

                if (!Directory.Exists("Saves"))
                    Directory.CreateDirectory("Saves");
                Save(save, savename);
            }
            return save;
        }

        public static SerWorld LoadWorld(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName + ".json"))
            {
                string content = sr.ReadToEnd();
                SerWorld obj = JsonConvert.DeserializeObject<SerWorld>(content, new TileConverter());
                return obj;
            }
            /*BinaryFormatter formatter = new BinaryFormatter();
            formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
            formatter.Binder = new Serialization.CustomizedBinder();

            using (Stream sw = new FileStream(fileName + ".lbmf", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (SerWorld)formatter.Deserialize(sw);
            }*/
        }
    }
}
