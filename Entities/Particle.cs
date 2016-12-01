using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;
using Limestone.Tiles;

namespace Limestone.Entities
{
    public class Particle : Entity
    {
        public override Vector2 center { get { return Vector2.Zero; } set { } }

        public int timeleft;
        public int maxTimeleft;

        private bool slowsDown;
        private float percentPerFrame;

        public Particle(Texture2D texture, Vector2 position, Vector2 direction, Color color, float scale, int timeleft) : base(position)
        {
            this.tType = EntityType.Particle;
            this.velocity = direction;
            this.color = color;
            this.scale = scale;
            this.timeleft = timeleft;
            maxTimeleft = timeleft;

            hitbox = new RotateableRectangle(new Rectangle((int)position.X, (int)position.Y, (int)scale, (int)scale));

            if (texture == null)
                this.texture = Assets.GetTexture("whitePixel");
            else this.texture = texture;
        }

        public Particle(Texture2D texture, Vector2 position, float angle, float speed, float distance, Color color, float scale) : base(position)
        {
            this.tType = EntityType.Particle;

            this.angle = angle;

            this.color = color;
            this.scale = scale;

            hitbox = new RotateableRectangle(new Rectangle((int)position.X, (int)position.Y, (int)scale, (int)scale));

            maxTimeleft = (int)(distance / speed);
            timeleft = maxTimeleft;

            velocity = Vector2.Normalize(Vector2.Transform(new Vector2(-1, 0), Matrix.CreateRotationZ(MathHelper.ToRadians(angle)))) * speed;

            if (texture == null)
                this.texture = Assets.GetTexture("whitePixel");
            else this.texture = texture;
        }

        public Particle SetSlowsDown(float percentPerFrame)
        {
            this.percentPerFrame = percentPerFrame;
            return this;
        }
        
        public override void Update(World world)
        {
            position += velocity;

            timeleft--;

            if (timeleft <= 0)
                Die(world);
        }

        public override void Die(World world)
        {
            dead = true;
        }

        protected override void RunFrameConfiguration()
        {
            frameConfiguration.Update();
        }

        public override void DrawOutline(SpriteBatch batch)
        {
            DrawHelper.DrawOutline(batch, texture, position, DrawHelper.GetTextureOffset(texture), scale, 1);

        }

        public override void Draw(SpriteBatch batch)
        {
            if (!dead && texture != null)
                batch.Draw(texture, position + texture.Bounds.Center.ToVector2() * scale, null, color, -Main.camera.Rotation, DrawHelper.GetTextureOffset(texture), scale, 0, 0);
        }

        public Particle Copy(Vector2 position, Vector2 velocity, Color color)
        {
            return new Particle(texture, position, velocity, color, scale, maxTimeleft);
        }

        public override void OnTileCollide(World world, Tile tile)
        {
            Die(world);
        }

        public override Entity Copy()
        {
            return new Particle(texture, position, velocity, color, scale, timeleft);
        }
    }
}
