using System;
using System.Threading;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using System.Collections.Generic;

namespace Limestone
{
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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        public bool paused;

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public World world;

        public static Camera2D camera;
        public static Random rand;
        public static GameMouse mouse;
        public static Chatbox cbox;
        public static readonly int WIDTH = 800, HEIGHT = 600;
        public static bool isActive = true;

        public static bool typing;
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public static KeyboardState currentKBState;
        public static KeyboardState oldKBState;

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
            camera = new Camera2D(GraphicsDevice.Viewport);
            rand = new Random(1);//DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) * DateTime.Now.Day * DateTime.Now.Month * DateTime.Now.Year * DateTime.Now.Second * DateTime.Now.Millisecond);
            //GIVE IT SOME REAAAAAAAAAAAAAAAAAAAAAL RANDOMNESS
            mouse = new GameMouse();
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
            // TODO: use this.Content to load your game content here
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
            Main.isActive = IsActive;
            currentKBState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            cbox.Update(this);

            mouse.Update();
            if (KeyPress(Keys.P))
                paused = !paused;
            if (world != null)
            {
                if (!paused)
                    world.Update();
                else if (paused && KeyPress(Keys.O))
                    world.Update();

                //if (world.player != null)
                    //world.player.index = socket.index;
            }
            else
            {
                world = new World(camera);
            }

            oldKBState = currentKBState;
            camera.prevPosition = camera.Position;
            mouse.prevState = mouse.state;
            base.Update(gameTime);
        }

        public static bool KeyPress(Keys key)
        {
            return currentKBState.IsKeyDown(key) && oldKBState.IsKeyUp(key);
        }

        public static bool KeyPressContinuous(Keys key)
        {
            return currentKBState.IsKeyDown(key);
        }

        public static bool AnyTwoKeysPressedTogether(Keys[] keys)
        {
            int num = 0;
            foreach (Keys k in keys)
            {
                if (currentKBState.IsKeyDown(k))
                    num++;
            }

            if (num >= 2)
                return true;
            else return false;
        }

        public static void AwaitNextKeyPress()
        {
            while (true)
            {
                if (!KeyPress(Keys.None))
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

            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix());

            world.Draw(spriteBatch);

            DrawHelper.StartDrawCameraSpace(spriteBatch);
            spriteBatch.DrawString(Assets.GetFont("bitfontMunro12"), fps, new Vector2(1, 96 - 16), Color.White);
            DrawHelper.StartDrawCameraSpace(spriteBatch);
            mouse.Draw(spriteBatch);
            spriteBatch.End();

            camera.oldCameraRotation = camera.Rotation;
            base.Draw(gameTime);
        }
    }
}