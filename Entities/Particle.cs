using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Entities
{
    public class Particle : Entity
    {
        public override Vector2 center { get { return Vector2.Zero; } set { } }

        public int timeleft;
        public int maxTimeleft;

        public Particle(Vector2 position, Vector2 direction, Color color, float scale, int timeleft)
        {
            this.tType = EntityType.Particle;
            this.position = position;
            this.velocity = direction;
            this.color = color;
            this.scale = scale;
            this.timeleft = timeleft;
            maxTimeleft = timeleft;

            hitbox = new RotateableRectangle(new Rectangle((int)position.X, (int)position.Y, (int)scale, (int)scale));
            texture = Assets.GetTexture("whitePixel");
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
            return new Particle(position, velocity, color, scale, maxTimeleft);
        }
    }
}
