using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using Limestone.Entities;

namespace Limestone.Utility
{
    public static class Assets
    {
        public static  GraphicsDevice device;
        public static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();

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

        public static SpriteFont GetFont(string name)
        {
            try
            {
                return fonts[name];
            }
            catch (KeyNotFoundException e)
            {
                Logger.Log("Couldn't find and/or load font file '" + name + "'!\n" + e.ToString(), true);
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

        public static SoundEffect GetSoundEffect(string name)
        {
            try
            {
                return soundEffects[name];
            }
            catch (KeyNotFoundException e)
            {
                Logger.Log("Couldn't find and/or load sound file '" + name + "'!\n" + e.ToString(), true);
                return null;
            }
        }

        public static void Load(GraphicsDevice device, ContentManager content)
        {
            Assets.device = device;
            Texture2D whitePixel = new Texture2D(device, 1, 1);
            whitePixel.SetData<Color>(new Color[] { Color.White });
            textures.Add("whitePixel", whitePixel);

            textures.Add("shadow", content.Load<Texture2D>("Textures/shadow"));
            textures.Add("notex", content.Load<Texture2D>("Textures/notex"));
            //Gui
            textures.Add("guiItemslot", content.Load<Texture2D>("Textures/GuiItemslot"));
            textures.Add("loading", content.Load<Texture2D>("Textures/loading"));
            textures.Add("guiChars", content.Load<Texture2D>("Textures/Guis/guiChars"));
            //Enemies
            textures.Add("char1", content.Load<Texture2D>("Textures/char1"));
            textures.Add("char2", content.Load<Texture2D>("Textures/char2"));
            textures.Add("char3", content.Load<Texture2D>("Textures/char3"));
            textures.Add("bagUntiered", content.Load<Texture2D>("Textures/bagUntiered"));
            textures.Add("bagCommon", content.Load<Texture2D>("Textures/bagCommon"));
            textures.Add("bagUncommon", content.Load<Texture2D>("Textures/bagUncommon"));
            textures.Add("bagRare", content.Load<Texture2D>("Textures/bagRare"));
            textures.Add("boss1", content.Load<Texture2D>("Textures/Enemies/boss1"));
            textures.Add("antispectator", content.Load<Texture2D>("Textures/Enemies/antispectator"));
            textures.Add("skullBoss1", content.Load<Texture2D>("Textures/Enemies/skullBoss1"));
            textures.Add("testdude1", content.Load<Texture2D>("Textures/Enemies/testdude1"));
            textures.Add("pet", content.Load<Texture2D>("Textures/Enemies/pet"));
            textures.Add("evil", content.Load<Texture2D>("Textures/Enemies/evil"));
            textures.Add("mummy1", content.Load<Texture2D>("Textures/Enemies/mummy1"));
            textures.Add("mummy2", content.Load<Texture2D>("Textures/Enemies/mummy2"));
            textures.Add("pilewrapping", content.Load<Texture2D>("Textures/Enemies/pilewrapping"));
            textures.Add("snake", content.Load<Texture2D>("Textures/Enemies/snake"));
            textures.Add("shade", content.Load<Texture2D>("Textures/Enemies/shade"));
            textures.Add("wisp", content.Load<Texture2D>("Textures/Enemies/wisp"));
            textures.Add("darkBrute", content.Load<Texture2D>("Textures/Enemies/darkBrute"));
            textures.Add("darkTower", content.Load<Texture2D>("Textures/Enemies/darkTower"));
            textures.Add("swampSlime", content.Load<Texture2D>("Textures/Enemies/swampSlime"));
            textures.Add("shadeEye", content.Load<Texture2D>("Textures/Enemies/shadeEye"));

            textures.Add("shuArtifactTower", content.Load<Texture2D>("Textures/Enemies/shuArtifactTower"));
            textures.Add("shuArtifact", content.Load<Texture2D>("Textures/Enemies/shuArtifact"));
            textures.Add("shu", content.Load<Texture2D>("Textures/Enemies/shu"));
            textures.Add("tefnutArtifact", content.Load<Texture2D>("Textures/Enemies/tefnutArtifact"));
            textures.Add("tefnut", content.Load<Texture2D>("Textures/Enemies/tefnut"));
            //Projectiles
            textures.Add("bolt", content.Load<Texture2D>("Textures/Projectiles/bolt"));
            textures.Add("bluebolt", content.Load<Texture2D>("Textures/Projectiles/bluebolt"));
            textures.Add("shield", content.Load<Texture2D>("Textures/Projectiles/shield"));
            textures.Add("gear", content.Load<Texture2D>("Textures/Projectiles/gear"));
            textures.Add("tearshot", content.Load<Texture2D>("Textures/Projectiles/tearshot"));
            textures.Add("spinner", content.Load<Texture2D>("Textures/Projectiles/spinner"));
            textures.Add("spinner2", content.Load<Texture2D>("Textures/Projectiles/spinner2"));
            textures.Add("star", content.Load<Texture2D>("Textures/Projectiles/star"));
            textures.Add("ball", content.Load<Texture2D>("Textures/Projectiles/ball"));
            textures.Add("arrow", content.Load<Texture2D>("Textures/Projectiles/arrow"));
            textures.Add("arrowtail", content.Load<Texture2D>("Textures/Projectiles/arrowtail"));
            textures.Add("icicle", content.Load<Texture2D>("Textures/Projectiles/icicle"));
            textures.Add("iceball", content.Load<Texture2D>("Textures/Projectiles/iceball"));
            textures.Add("elementalshot", content.Load<Texture2D>("Textures/Projectiles/elementalshot"));
            textures.Add("littlebullet", content.Load<Texture2D>("Textures/Projectiles/littlebullet"));
            textures.Add("tearSpinner", content.Load<Texture2D>("Textures/Projectiles/tearSpinner"));
            textures.Add("sun", content.Load<Texture2D>("Textures/Projectiles/sun"));
            textures.Add("redball", content.Load<Texture2D>("Textures/Projectiles/redball"));
            textures.Add("wrapper", content.Load<Texture2D>("Textures/Projectiles/wrapper"));
            textures.Add("fang", content.Load<Texture2D>("Textures/Projectiles/fang"));
            textures.Add("spiker", content.Load<Texture2D>("Textures/Projectiles/spiker"));
            textures.Add("spiker2", content.Load<Texture2D>("Textures/Projectiles/spiker2"));
            textures.Add("swipe", content.Load<Texture2D>("Textures/Projectiles/swipe"));
            textures.Add("darkSphere", content.Load<Texture2D>("Textures/Projectiles/darkSphere"));
            textures.Add("slimeShot", content.Load<Texture2D>("Textures/Projectiles/slimeShot"));

            textures.Add("projectilesFull", content.Load<Texture2D>("Textures/Projectiles/projectilesFull"));
            //Tiles
            textures.Add("beach1", content.Load<Texture2D>("Textures/Tiles/beach1"));
            textures.Add("beach2", content.Load<Texture2D>("Textures/Tiles/beach2"));
            textures.Add("rock1", content.Load<Texture2D>("Textures/Tiles/rock1"));
            textures.Add("water1", content.Load<Texture2D>("Textures/Tiles/water1"));
            textures.Add("grassLowlands1", content.Load<Texture2D>("Textures/Tiles/grassLowlands1"));   //TODO change back from beach 3-6
            textures.Add("grassMidlands1", content.Load<Texture2D>("Textures/Tiles/grassMidlands1"));
            textures.Add("grassHighlands1", content.Load<Texture2D>("Textures/Tiles/grassHighlands1"));
            textures.Add("grassAncientlands1", content.Load<Texture2D>("Textures/Tiles/grassAncientlands1"));
            textures.Add("rocksLowlands1", content.Load<Texture2D>("Textures/Tiles/rocksLowlands1"));
            textures.Add("rocksMidlands1", content.Load<Texture2D>("Textures/Tiles/rocksMidlands1"));
            textures.Add("rocksMidlands2", content.Load<Texture2D>("Textures/Tiles/rocksMidlands2"));
            textures.Add("rocksHighlands1", content.Load<Texture2D>("Textures/Tiles/rocksHighlands1"));
            textures.Add("rocksAncientlands1", content.Load<Texture2D>("Textures/Tiles/rocksAncientlands1"));
            textures.Add("rocksAncientlands2", content.Load<Texture2D>("Textures/Tiles/rocksAncientlands2"));
            textures.Add("rocksAncientlands3", content.Load<Texture2D>("Textures/Tiles/rocksAncientlands3"));
            textures.Add("brick1", content.Load<Texture2D>("Textures/Tiles/brick1"));
            textures.Add("brick2", content.Load<Texture2D>("Textures/Tiles/brick2"));
            textures.Add("brickTop", content.Load<Texture2D>("Textures/Tiles/brickTop"));
            textures.Add("sand1", content.Load<Texture2D>("Textures/Tiles/sand1"));
            textures.Add("sand2", content.Load<Texture2D>("Textures/Tiles/sand2"));
            textures.Add("sandWall1", content.Load<Texture2D>("Textures/Tiles/sandWall1"));
            textures.Add("sandWall2", content.Load<Texture2D>("Textures/Tiles/sandWall2"));
            textures.Add("sandWall3", content.Load<Texture2D>("Textures/Tiles/sandWall3"));
            textures.Add("sandWall4", content.Load<Texture2D>("Textures/Tiles/sandWall4"));
            textures.Add("sandWallTop1", content.Load<Texture2D>("Textures/Tiles/sandWallTop1"));
            textures.Add("purple0", content.Load<Texture2D>("Textures/Tiles/purple0"));
            textures.Add("purple1", content.Load<Texture2D>("Textures/Tiles/purple1"));
            textures.Add("purple2", content.Load<Texture2D>("Textures/Tiles/purple2"));

            textures.Add("tilesFull", content.Load<Texture2D>("Textures/Tiles/tilesFull"));
            textures.Add("bbTilesFull", content.Load<Texture2D>("Textures/Tiles/bbTilesFull"));
            //Items
            //Swords
            textures.Add("testItem1", content.Load<Texture2D>("Textures/Items/Weapons/testItem1"));
            textures.Add("swordIron", content.Load<Texture2D>("Textures/Items/Weapons/swordIron")); //t1
            textures.Add("swordBluesteel", content.Load<Texture2D>("Textures/Items/Weapons/swordBluesteel"));   //t2
            textures.Add("swordElvenwood", content.Load<Texture2D>("Textures/Items/Weapons/swordElvenwood"));   //t3
            textures.Add("swordBloodwood", content.Load<Texture2D>("Textures/Items/Weapons/swordBloodwood"));   //t4
            textures.Add("swordGold", content.Load<Texture2D>("Textures/Items/Weapons/swordGold")); //t5
            textures.Add("swordRuby", content.Load<Texture2D>("Textures/Items/Weapons/swordRuby")); //t6
            textures.Add("swordSapphire", content.Load<Texture2D>("Textures/Items/Weapons/swordSapphire")); //t7
            textures.Add("swordDiamond", content.Load<Texture2D>("Textures/Items/Weapons/swordDiamond"));   //t8
            textures.Add("swordCrystal", content.Load<Texture2D>("Textures/Items/Weapons/swordCrystal"));   //t9
            textures.Add("swordArbiter", content.Load<Texture2D>("Textures/Items/Weapons/swordArbiter"));   //t10
            textures.Add("swordIcebinder", content.Load<Texture2D>("Textures/Items/Weapons/swordIcebinder")); //t11
            textures.Add("swordDemonbane", content.Load<Texture2D>("Textures/Items/Weapons/swordDemonbane")); //t12
            textures.Add("testItem2", content.Load<Texture2D>("Textures/Items/Weapons/testItem2"));
            //Bows
            textures.Add("bowWood", content.Load<Texture2D>("Textures/Items/Weapons/bowWood"));
            textures.Add("bowPinksteel", content.Load<Texture2D>("Textures/Items/Weapons/bowPinksteel"));
            textures.Add("bowBlackwood", content.Load<Texture2D>("Textures/Items/Weapons/bowBlackwood"));
            textures.Add("bowBone", content.Load<Texture2D>("Textures/Items/Weapons/bowBone"));
            textures.Add("bowStone", content.Load<Texture2D>("Textures/Items/Weapons/bowStone"));
            textures.Add("bowThick", content.Load<Texture2D>("Textures/Items/Weapons/bowThick"));
            textures.Add("bowFirestone", content.Load<Texture2D>("Textures/Items/Weapons/bowFirestone"));
            textures.Add("bowCopper", content.Load<Texture2D>("Textures/Items/Weapons/bowCopper"));
            textures.Add("bowBrass", content.Load<Texture2D>("Textures/Items/Weapons/bowBrass"));
            textures.Add("bowSapphire", content.Load<Texture2D>("Textures/Items/Weapons/bowSapphire"));
            textures.Add("bowEmerald", content.Load<Texture2D>("Textures/Items/Weapons/bowEmerald"));
            textures.Add("bowJewelcrafter", content.Load<Texture2D>("Textures/Items/Weapons/bowJewlcrafter"));
            //Ability - quiver
            textures.Add("quiverSilkbound", content.Load<Texture2D>("Textures/Items/Abilities/quiverSilkbound"));
            textures.Add("quiverBloodstained", content.Load<Texture2D>("Textures/Items/Abilities/quiverBloodstained"));
            textures.Add("quiverRedwood", content.Load<Texture2D>("Textures/Items/Abilities/quiverRedwood"));
            textures.Add("quiverPinksteel", content.Load<Texture2D>("Textures/Items/Abilities/quiverPinksteel"));
            textures.Add("quiverEbonwood", content.Load<Texture2D>("Textures/Items/Abilities/quiverEbonwood"));
            textures.Add("quiverBloodstone", content.Load<Texture2D>("Textures/Items/Abilities/quiverBloodstone"));

            textures.Add("eleOrbs", content.Load<Texture2D>("Textures/Items/Weapons/eleOrbs"));

            textures.Add("pendants", content.Load<Texture2D>("Textures/Items/Abilities/pendants"));

            textures.Add("armorLight", content.Load<Texture2D>("Textures/Items/Armors/armorLight"));
            textures.Add("armorMedium", content.Load<Texture2D>("Textures/Items/Armors/armorMedium"));
            textures.Add("armorHeavy", content.Load<Texture2D>("Textures/Items/Armors/armorHeavy"));

            textures.Add("ringBase", content.Load<Texture2D>("Textures/Items/Rings/ringBase"));
            textures.Add("ringOverlay", content.Load<Texture2D>("Textures/Items/Rings/ringOverlay"));
            //Fonts
            fonts.Add("munro12", content.Load<SpriteFont>("Fonts/munro12"));
            fonts.Add("munro8", content.Load<SpriteFont>("Fonts/munro8"));
            fonts.Add("bitfontMunro12", content.Load<SpriteFont>("Fonts/bitfontMunro12"));
            fonts.Add("bitfontMunro8", content.Load<SpriteFont>("Fonts/bitfontMunro8"));
            fonts.Add("bitfontMunro23BOLD", content.Load<SpriteFont>("Fonts/bitfontMunro23BOLD"));

            shaders.Add("test", content.Load<Effect>("Effects/sharpen"));

            soundEffects.Add("buttonclick", content.Load<SoundEffect>("Sounds/Generic/buttonclick"));
            soundEffects.Add("error", content.Load<SoundEffect>("Sounds/Generic/error"));
            soundEffects.Add("genericImpact1", content.Load<SoundEffect>("Sounds/Hit/genericImpact1"));
            soundEffects.Add("spriteImpact1", content.Load<SoundEffect>("Sounds/Hit/spriteImpact1"));
            soundEffects.Add("woodImpact1", content.Load<SoundEffect>("Sounds/Hit/woodImpact1"));
            soundEffects.Add("woodImpact2", content.Load<SoundEffect>("Sounds/Hit/woodImpact2"));
            soundEffects.Add("monsterImpact1", content.Load<SoundEffect>("Sounds/Hit/monsterImpact1"));
            soundEffects.Add("monsterImpact2", content.Load<SoundEffect>("Sounds/Hit/monsterImpact2"));
            soundEffects.Add("monsterImpact3", content.Load<SoundEffect>("Sounds/Hit/monsterImpact3"));
            soundEffects.Add("squeakImpact1", content.Load<SoundEffect>("Sounds/Hit/squeakImpact1"));
            soundEffects.Add("playerImpact1", content.Load<SoundEffect>("Sounds/Hit/playerImpact1"));

            soundEffects.Add("deathMonster1", content.Load<SoundEffect>("Sounds/Death/deathMonster1"));

            soundEffects.Add("bowfire1", content.Load<SoundEffect>("Sounds/Weapon/bowfire1"));
            soundEffects.Add("eleorbfire1", content.Load<SoundEffect>("Sounds/Weapon/eleorbfire1"));
        }

        internal static void Unload()
        {
            foreach (KeyValuePair<string, Texture2D> kvp in textures)
                textures[kvp.Key].Dispose();

            textures.Clear();
        }

        public static Texture2D GetTexFromSource(Texture2D texture, Rectangle sourceRect)
        {
            Texture2D cropTexture = new Texture2D(device, sourceRect.Width, sourceRect.Height);
            Color[] data = new Color[sourceRect.Width * sourceRect.Height];
            texture.GetData(0, sourceRect, data, 0, data.Length);

            cropTexture.SetData(data);

            return cropTexture;
        }

        public static Texture2D GetTexFromSource(string texture, int x, int y)
        {
            Texture2D tex = GetTexture(texture);
            int xpos = x * 8;
            int ypos = y * 8;
            Texture2D cropTexture = new Texture2D(device, 8, 8);
            Color[] data = new Color[8 * 8];
            tex.GetData(0, new Rectangle(xpos, ypos, 8, 8), data, 0, data.Length);

            cropTexture.SetData(data);

            return cropTexture;
        }

        public static Rectangle GetSourceRect(Vector2 location, Vector2 texturesize)
        {
            return new Rectangle((location * texturesize).ToPoint(), texturesize.ToPoint());
        }
    }
}
