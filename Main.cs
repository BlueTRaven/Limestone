using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;

namespace Limestone
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        public bool paused;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        World world;

        public static Camera2D camera;

        //public static ClientSocket2 socket;
        private int retryConnectionTimer = 60;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public static KeyboardState currentKBState;
        public static KeyboardState oldKBState;
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

            camera = new Camera2D(GraphicsDevice.Viewport);
            //socket = new ClientSocket2();

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
            currentKBState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (KeyPress(Keys.P))
                paused = !paused;
            if (world != null)
            {
                if (!paused)
                    world.Update();

                //if (world.player != null)
                    //world.player.index = socket.index;
            }
            else
            {
                world = new World(camera);
                //socket.world = world;
            }

            oldKBState = currentKBState;
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix());

            world.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
