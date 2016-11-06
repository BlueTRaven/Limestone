using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Inp;
using Limestone.Guis;

namespace Limestone
{
    #region FC

    public class FrameCounter
    {
        public FrameCounter()
        {
        }

        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }

        public const int MAXIMUM_SAMPLES = 100;

        private Queue<float> _sampleBuffer = new Queue<float>();

        public bool Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            _sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                _sampleBuffer.Dequeue();
                AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
            return true;
        }
    }
    #endregion 

    public class Main : Game
    {
        public static string fps;

        public static int globalTimer;
        public bool paused;

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public World world;

        public static GameCamera camera;
        public static GameMouse mouse;
        public static GameKeyboard keyboard;
        public static Random rand;
        public static Chatbox cbox;
        public static PlayerSave playersave;
        public static readonly int WIDTH = 800, HEIGHT = 600;
        public static bool isActive = true;

        public static bool typing;


        public static bool hold = true;
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private FrameCounter _frameCounter = new FrameCounter();

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Logger.CreateNewLogFile();
            this.IsMouseVisible = true;
            rand = new Random(1);//DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) * DateTime.Now.Day * DateTime.Now.Month * DateTime.Now.Year * DateTime.Now.Second * DateTime.Now.Millisecond);
            //GIVE IT SOME REAAAAAAAAAAAAAAAAAAAAAL RANDOMNESS
            camera = new GameCamera(GraphicsDevice.Viewport);
            mouse = new GameMouse();
            keyboard = new GameKeyboard();
            cbox = new Chatbox();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Assets.Load(GraphicsDevice, Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            globalTimer++;
            Main.isActive = IsActive;
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Exit();

            cbox.Update(this);

            mouse.Update();
            keyboard.Update();
            camera.Update();

            if (Main.keyboard.KeyPressed(Keys.P))
                paused = !paused;

            //if (keyboard.KeyPressed(Keys.Y))

            if (!hold)
            {
                if (world != null)
                {
                    if (!paused)
                        world.Update();
                    else if (paused && Main.keyboard.KeyPressed(Keys.O))
                        world.Update();
                }
            }

            camera.PostUpdate(this);
            mouse.PostUpdate();
            keyboard.PostUpdate();
            base.Update(gameTime);
        }

        public static void AwaitNextKeyPress()
        {
            while (true)
            {
                if (!keyboard.KeyPressed(Keys.None))
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameCounter.Update(deltaTime);

            fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix());

            if (world != null) world.Draw(camera, spriteBatch);

            DrawHelper.StartDrawCameraSpace(spriteBatch);
            camera.Draw(this, spriteBatch);
            DrawHelper.StartDrawCameraSpace(spriteBatch);
            mouse.Draw(spriteBatch);
            spriteBatch.End();

            camera.oldCameraRotation = camera.Rotation;
            base.Draw(gameTime);
        }
    }
}