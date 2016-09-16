using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Limestone.Utility
{
    public static class Assets
    {
        public static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static Dictionary<string, Effect> shaders = new Dictionary<string, Effect>();

        public static Texture2D GetTexture(string name)
        {
            try
            {
                return textures[name];
            }
            catch (KeyNotFoundException e)
            {
                Logger.Log("Couldn't find and/or load texture file '" + name + "'!\n" + e.ToString(), true);
                return null;
            }
        }

        public static Effect GetEffect(string name)
        {
            try
            {
                return shaders[name];
            }
            catch (KeyNotFoundException e)
            {
                Logger.Log("Couldn't find and/or load shader file '" + name + "'!\n" + e.ToString(), true);
                return null;
            }
        }

        public static void Load(GraphicsDevice device, ContentManager content)
        {
            Texture2D whitePixel = new Texture2D(device, 1, 1);
            whitePixel.SetData<Color>(new Color[] { Color.White });
            textures.Add("whitePixel", whitePixel);

            textures.Add("char1", content.Load<Texture2D>("Textures/char1"));
            textures.Add("boss1", content.Load<Texture2D>("Textures/boss1"));

            textures.Add("bolt", content.Load<Texture2D>("Textures/bolt"));
            textures.Add("bluebolt", content.Load<Texture2D>("Textures/bluebolt"));
            textures.Add("shield", content.Load<Texture2D>("Textures/shield"));
            textures.Add("gear", content.Load<Texture2D>("Textures/gear"));
            textures.Add("tearshot", content.Load<Texture2D>("Textures/tearshot"));

            shaders.Add("test", content.Load<Effect>("Effects/sharpen"));
        }

        internal static void Unload()
        {
            foreach (KeyValuePair<string, Texture2D> kvp in textures)
                textures[kvp.Key].Dispose();

            textures.Clear();
        }

        public static Rectangle GetSourceRect(Vector2 location, int texturesize)
        {
            return new Rectangle((location * texturesize).ToPoint(), new Point(texturesize, texturesize));
        }
    }
}
