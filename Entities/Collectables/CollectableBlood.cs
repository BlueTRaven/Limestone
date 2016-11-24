using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using Limestone.Utility;
using Limestone.Items;
using Limestone.Entities;

using Limestone.Interface;
using Limestone.Tiles;

namespace Limestone.Entities.Collectables
{
    public class CollectableBlood : Collectable
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        private Vector2 endPosition;

        private readonly float moveSpeed;
        private float velZ, gravity = -.5f;

        private float timeToGround, timeToGroundMax, timeDistance;

        private Player2 player;

        public int timeLeft;

        public CollectableBlood(Player2 player, Vector2 position, float angle, float distance, float speed) : base(position)
        {
            this.player = player;

            this.timeDistance = distance;
            this.angle = angle;
            this.moveSpeed = speed;
            velocity = Vector2.Normalize(Vector2.Transform(new Vector2(-1, 0), Matrix.CreateRotationZ(MathHelper.ToRadians(-angle)))) * moveSpeed;

            timeToGroundMax = timeDistance / moveSpeed;
            timeToGround = timeToGroundMax;

            endPosition = position + velocity * timeToGround;

            velZ = (-gravity / 2) * timeToGround;

            SetDefaults();
        }

        private void SetDefaults()
        {
            collectOverride = true;
            collectTicks = 60;  //we can't collect them for one second.

            timeLeft = 720; //time out after 12 seconds.

            texture = Assets.GetTexture("blood");

            scale = 1.5f;
            shadowScale = 2f;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(16)));
            activeDistance = 1024;

            setSize = new Rectangle(0, 0, 8, 8);
            frameConfiguration = new FrameConfiguration(FrameConfiguration.FrameActionPreset3, this, new FrameCollection(false,
                new Frame(5, new Rectangle(0, 0, 8, 8)),
                new Frame(5, new Rectangle(8, 0, 8, 8)),
                new Frame(5, new Rectangle(16, 0, 8, 8)),
                new Frame(5, new Rectangle(24, 0, 8, 8))));
        }

        public override void Update(World world)
        {
            base.Update(world);

            if (collectable)
                timeLeft--;

            timeToGround--;
            elapsedTime++;

            if (timeToGround > 0)
            {
                velZ += gravity;
                height += velZ;

                position += velocity;
                hitbox.MoveTo(position);
                hitbox.RotateTo(position, angle);
            }

            if (height < 0)
                height = 0;

            if (timeLeft <= 0)
                Die(world);
        }

        public override void OnPickup(World world)
        {
            Die(world);
            player.collectedBloods++;
        }

        public override void Die(World world)
        {
            base.Die(world);

            for (int i = 0; i < Main.rand.Next(2, 5); i++)
                world.CreateParticle(new Particle(null, center, new Vector2((float)Main.rand.NextDouble(-1, 1), (float)Main.rand.NextDouble(-1, 1)), Color.Red, 3, 12));
        }

        protected override void RunFrameConfiguration()
        {
            frameConfiguration.Update();
        }

        float breathe = 60, breatheMax = 60;
        bool breatheUp;

        public override void DrawOutline(SpriteBatch batch)
        {
            float breatheP = breathe / breatheMax;

            if (breatheUp)
                breathe++;
            else breathe--;

            if (breathe <= 0 || breathe > 60)
                breatheUp = !breatheUp;

            float breatheScale = (collectOverride ? .01f : .15f) * breatheP;

            Vector2 offset = Main.camera.up * ((shadowTexture.Height * scale) / 32) + Main.camera.up * height;
            batch.Draw(Assets.GetTexture("glowMask"), position + offset, null, Color.Red, -Main.camera.Rotation, new Vector2(64, 64), .15f + breatheScale, 0, 0);

            base.DrawOutline(batch);
        }

        public override void Draw(SpriteBatch batch)
        {
            if (frameConfiguration.currentFrame == null)
                frameConfiguration.currentFrame = new Frame(-1, setSize);

            batch.Draw(shadowTexture, position, null, shadowTextureColor, -Main.camera.Rotation, ShadowOffset(), shadowScale / 8, 0, 0);

            Vector2 offset = Main.camera.up * ((shadowTexture.Height * scale) / 32) + Main.camera.up * height;
            batch.Draw(texture, position + offset, frameConfiguration.currentFrame.size, color, -Main.camera.Rotation, TextureOffset(), scale, 0, 0);

            if (Options.DEBUGDRAWCOLLECTABLEHITBOXES)
                hitbox.DebugDraw(batch);
        }

        public override Entity Copy()
        {
            return new CollectableBlood(player, position, angle, timeDistance, speed);
        }
    }
}
