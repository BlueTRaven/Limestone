using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;

namespace Limestone
{
    public abstract class Entity
    {
        public Dictionary<string, int> counter = new Dictionary<string, int>();

        public Vector2 position;
        protected Vector2 prevPos;
        public Vector2 velocity;
        public float speed;

        public Texture2D texture;
        public Vector2 textureSize; //size in pixels. Not expanded size
        protected Vector2 textureCoord;
        public Color color = Color.White;
        public bool flip;
        public float scale;

        public bool dead;

        public float angle;

        public abstract Vector2 center { get; set; }

        public abstract void Update(World world);
        public abstract void Die(World world);
        public abstract void Draw(SpriteBatch batch);
        public abstract void DrawOutline(SpriteBatch batch);
    }

    public abstract class EntityLiving : Entity
    {
        public int health, maxHealth;

        public abstract void TakeDamage(int amt, Projectile source);
    }
}
