using DestroyNobots.Engine;
using DestroyNobots.Engine.Entities;
using DestroyNobots.Engine.Entities.Vehicles;
using DestroyNobots.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DestroyNobots
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DestroyNobotsGame : Game
    {
        GraphicsDeviceManager graphics;
        Screen currentScreen;

        public TimerManager TimerManager { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public EntityManager EntityManager { get; private set; }
        public TextureManager TextureManager { get; private set; }
        public Camera Camera { get; private set; }

        public DestroyNobotsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void SwitchScreen<T>() where T : Screen, new()
        {
            SwitchScreen(new T());
        }

        public void SwitchScreen(Screen newScreen)
        {
            if (currentScreen != null)
                currentScreen.Unload();

            currentScreen = newScreen;

            if (currentScreen != null)
            {
                currentScreen.Game = this;
                currentScreen.Load();
            }
        }

        Buggy b;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            IsFixedTimeStep = false;
            IsMouseVisible = true;

            TimerManager = new TimerManager();
            EntityManager = new EntityManager() { Game = this };
            Camera = new Camera();

            Services.AddService(TimerManager);
            Services.AddService(EntityManager);
            Services.AddService(TextureManager);

            b = EntityManager.Create<Buggy>();
            byte[] code = b.Computer.Processor.GetAssociatedCompiler().Compile(System.IO.File.Open("BuggyCode.txt", System.IO.FileMode.Open));
            var memory = new Assembler.SafeMemory(code.Length, Assembler.BinaryMultiplier.B);
            memory.Write(0, code);
            b.Computer.SwitchROM(memory);
            b.Computer.PowerUp();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            TextureManager = new TextureManager(this);

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            TextureManager.Load();

            SwitchScreen<GameScreen>();
        }

        protected override void UnloadContent()
        {
            TextureManager.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            b.Computer.Step();
            TimerManager.Update(gameTime);
            currentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            currentScreen.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
