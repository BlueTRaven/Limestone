using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using Limestone.Utility;

using Limestone.Interface;
using Limestone.Tiles;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Limestone.Entities
{
    public enum EntityType
    {
        Player,
        Enemy,
        Enemy2,
        Projectile,
        Bag,
        Spawner,
        Particle,
        Collectable,
        Bomb,
        NPC,
        NONE
    }
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public abstract class Entity : ISerializable
    {
        public Dictionary<string, float> counter = new Dictionary<string, float>();

        [JsonProperty]
        public Vector2 position;
        protected Vector2 prevPos;
        public Vector2 velocity;
        public float speed;

        public RotateableRectangle hitbox;

        public Texture2D texture;
        public Color color = Color.White;

        protected FrameConfiguration frameConfiguration;

        public float height = 0;
        public float shadowScale;
        protected Texture2D shadowTexture;
        protected Color shadowTextureColor;

        public Vector2 textureSize; //size in pixels. Not expanded size

        protected Vector2 textureCoord;

        protected Rectangle setSize;

        public bool flip;
        public float scale;

        public bool dead;
        public bool isPlayer = false;

        public bool moving = false;

        public bool tileCollides = true;

        public float angle;

        public EntityType tType;

        protected float elapsedTime;
        public abstract Vector2 center { get; set; }

        public abstract Entity Copy();
        public abstract void Update(World world);
        public abstract void OnTileCollide(World world, Tile tile);
        public abstract void Die(World world);
        protected abstract void RunFrameConfiguration();
        public abstract void Draw(SpriteBatch batch);
        public abstract void DrawOutline(SpriteBatch batch);

        protected Vector2 startPosition;
        public Entity(Vector2 position)
        {
            shadowTexture = Assets.GetTexture("shadow");

            frameConfiguration = new FrameConfiguration();

            this.position = position;
            this.startPosition = position;
        }

        protected Vector2 ShadowOffset()
        {
            return new Vector2(shadowTexture.Width / 2, shadowTexture.Height / 2);
        }
        protected Vector2 TextureOffset()
        {
            return new Vector2(setSize.Width / 2, setSize.Height / 2);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) { /*NOOP Used for serialization only*/ }
    }

    [Serializable]
    public abstract class EntityLiving : Entity, ISerializable
    {
        protected Queue<MoveStyle> moveQueue = new Queue<MoveStyle>();
        protected MoveStyle currentMove, previousMove;

        public bool active = true, paralyzed = false;
        public int health, maxHealth, defense;
        public bool stunned = false;

        public float activeDistance;

        public SoundEffect hitSound;
        public SoundEffect dieSound;

        public List<DamageText> texts = new List<DamageText>();

        protected bool flips;

        protected Color baseColor;
        protected Color flashColor;
        protected float flashDuration, flashDurationMax;
        protected int flashTotalDuration = -1;

        public bool invulnerable = false;
        public bool invulnOverride = false;
        public bool untargetable = false;
        public bool untOverride = false;
        private int _invulnTicks;
        public int invulnTicks { get { return _invulnTicks; } set { _invulnTicks = value; invulnerable = true; } }
        private int _untargetTicks;
        public int untargetTicks { get { return _untargetTicks; } set { _untargetTicks = value; untargetable = true; } }

        public EntityLiving(Vector2 position) : base(position)
        {   //NOOP

        }

        public override void Update(World world )
        {
            elapsedTime++;
            if (tType != EntityType.Spawner && tType != EntityType.Player)
            {
                if (hitbox == null || hitbox.ToRectangle() == Rectangle.Empty)
                    throw new Exception("Hitbox not correctly assigned!");
                if (setSize == null || setSize == Rectangle.Empty)
                    Console.WriteLine("Setsize not assigned to; enemy will appear invisible. SetSize: " + setSize == null ? "null" : setSize.ToString() + "\nBy entity of type: " + tType.ToString());
                if (scale <= 0)
                    Console.WriteLine("Scale is less than 0 - texture will not appear!\nScale: " + scale + "\nBy entity of type: " + tType.ToString());
            }

            RunFrameConfiguration();

            if (flashTotalDuration >= 0)
            {
                flashTotalDuration--;
                flashDuration--;

                if (flashDuration <= 0)
                    flashDuration = flashDurationMax;

                if (flashDuration <= flashDurationMax / 2)
                    color = Color.Lerp(baseColor, flashColor, flashDuration / flashDurationMax);
                else
                    color = Color.Lerp(flashColor, baseColor, flashDuration / flashDurationMax);
            }

            if (flips)
            {
                float angle = VectorHelper.GetAngleBetweenPoints(center, world.player.center) + MathHelper.ToDegrees(Main.camera.Rotation);

                if (angle >= 360)
                    angle -= 360;
                if (angle < 0)
                    angle += 360;

                if (angle >= 90 && angle <= 270)
                    flip = false;
                else
                    flip = true;
            }

            if (invulnTicks > 0)
                invulnTicks--;
            if (invulnTicks <= 0 && !invulnOverride)
                invulnerable = false;

            if (untargetTicks > 0)
                untargetTicks--;
            if (untargetTicks <= 0 && !untOverride)
                untargetable = false;

            for (int i = texts.Count - 1; i >= 0; i--)
            {
                texts[i].center = center;
                texts[i].Update();
                if (texts[i].dead)
                    texts.RemoveAt(i--);
            }
        }

        public void Move(Vector2 direction)
        {
            if (!paralyzed)
            {
                position += direction;

                hitbox.MoveTo(position);
            }
        }

        public void ClampVel(float val)
        {
            velocity.X = MathHelper.Clamp(velocity.X, -val, val);
            velocity.Y = MathHelper.Clamp(velocity.Y, -val, val);
        }

        public abstract void TakeDamage(int amt, IDamageDealer source, World world);
    }
}
