using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using Limestone.Utility;
using Limestone.Buffs;

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
        NONE
    }
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Entity
    {
        public Dictionary<string, float> counter = new Dictionary<string, float>();

        [JsonProperty]
        public Vector2 position;
        protected Vector2 prevPos;
        public Vector2 velocity;
        public float speed;

        public RotateableRectangle hitbox;

        public string textureName;
        private Texture2D _texture;
        public Texture2D texture { get { return _texture; } set { textureName = value.Name;  _texture = value; } }
        private Texture2D _shadowTexture;
        protected Texture2D shadowTexture { get { return _shadowTexture; } set { _shadowTexture = value; } }
        protected Color shadowTextureColor;
        public Vector2 textureSize; //size in pixels. Not expanded size
        protected Vector2 textureCoord;
        public Color color = Color.White;
        public bool flip;
        public float scale;

        public bool dead;
        public bool player = false;

        public bool tileCollides = true;

        public float angle;

        public EntityType tType;
        public abstract Vector2 center { get; set; }

        public abstract void Update(World world);
        public abstract void Die(World world);
        public abstract void Draw(SpriteBatch batch);
        public abstract void DrawOutline(SpriteBatch batch);

        public Entity()
        {
            shadowTexture = Assets.GetTexture("shadow");
        }
    }

    public abstract class EntityLiving : Entity
    {
        public bool active = true, paralyzed = false;
        public int health, maxHealth, defense;
        public bool stunned = false;

        public float activeDistance;

        public SoundEffect hitSound;
        public SoundEffect dieSound;

        public List<Buff> buffs = new List<Buff>();
        public List<DamageText> texts = new List<DamageText>();

        public abstract void TakeDamage(int amt, Projectile2 source, World world);

        public void Move(Vector2 direction)
        {
            if (!paralyzed)
            {
                position += direction;

                hitbox.MoveTo(center);
            }
        }

        public void ClampVel(float val)
        {
            velocity.X = MathHelper.Clamp(velocity.X, -val, val);
            velocity.Y = MathHelper.Clamp(velocity.Y, -val, val);
        }
    }
}
