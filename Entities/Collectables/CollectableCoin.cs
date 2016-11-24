using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using Limestone.Utility;
using Limestone.Entities;

namespace Limestone.Entities.Enemies.Collectables
{
    public class CollectableCoin : Collectable
    {
        public override Vector2 center { get { return hitbox.center; } set { } }

        float gravity = -.5f;
        float velZ;

        int bounces;
        bool doneBouncing;
        SoundEffectInstance bounceSound;

        public CollectableCoin(Vector2 position) : base(position)
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            texture = Assets.GetTexture("coin");
            collectSound = Assets.GetSoundEffect("coinPickup1").CreateInstance();
            bounceSound = Assets.GetSoundEffect("groundCoin1").CreateInstance();
            bounceSound.Volume = .2f;
            bounceSound.Pitch = .8f;

            scale = 1.5f;
            shadowScale = 2f;
            height = 0;
            velZ = -16;
            //height = -144;

            hitbox = new RotateableRectangle(new Rectangle(position.ToPoint(), new Point(16)));
            activeDistance = 1024;

            collectTicks = 120;

            setSize = new Rectangle(0, 0, 16, 16);
            frameConfiguration = new FrameConfiguration(FrameConfiguration.FrameActionPreset3, this, new FrameCollection(false, 
                new Frame(5, new Rectangle(0, 0, 16, 16)),
                new Frame(5, new Rectangle(16, 0, 16, 16)),
                new Frame(5, new Rectangle(32, 0, 16, 16)),
                new Frame(5, new Rectangle(48, 0, 16, 16)),
                new Frame(5, new Rectangle(64, 0, 16, 16)),
                new Frame(5, new Rectangle(80, 0, 16, 16))));
        }

        public override void Update(World world)
        {
            base.Update(world);

            moving = true;

            velZ += gravity;
            height += velZ;

            if (height <= 4 && !doneBouncing)
            {
                velZ = -velZ * .65f;

                height = 4;

                bounces++;

                if (bounces > 1 && bounces <= 4)
                {
                    bounceSound.Play();
                }
            }

            if (bounces > 4)
            {
                height = 4;
                doneBouncing = true;
            }
        }

        public override Entity Copy()
        {
            return new CollectableCoin(position);
        }

        public override void OnPickup(World world)
        {
            collectSound.Play();
            float particleInCircle = Main.rand.Next(3, 7);

            for (float i = 0; i <= 360; i += 360 / particleInCircle)
            {
                Particle p = new Particle(Assets.GetTexFromSource("particlesFull", 0, 0), center, Vector2.Transform(new Vector2(-(float)Main.rand.NextDouble(3, 5), 0), Matrix.CreateRotationZ(MathHelper.ToRadians(i))), Color.White, 0.65f, 10);
                world.CreateParticle(p);
            }
            Die(world);
        }

        protected override void RunFrameConfiguration()
        {
            frameConfiguration.Update();
        }
    }
}
