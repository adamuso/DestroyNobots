using DestroyNobots.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DestroyNobots
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DestroyNobotsGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Terrain terrain;
        Matrix view, projection;
        BasicEffect effect;

        Vector3 position;
        Vector2 rotation, lastRotation;

        public DestroyNobotsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            terrain = Terrain.FromHeightmap(this, Content.Load<Texture2D>("heightmap"));
            terrain.Game = this;
            // TODO: use this.Content to load your game content here

            view = Matrix.CreateLookAt(new Vector3(0, 10, 0), new Vector3(0, 0, 0), new Vector3(0, 0, -1));
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.1f, 300.0f);

            effect = new BasicEffect(GraphicsDevice);
            effect.LightingEnabled = true;

            //effect.EnableDefaultLighting();
            //effect.AmbientLightColor = new Vector3(0.3f);

            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.Direction = new Vector3(-1, -1, 0);
            effect.DirectionalLight0.DiffuseColor = Color.Gray.ToVector3();
            effect.DirectionalLight0.SpecularColor = Color.Black.ToVector3();
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 delta = mouseState.Position.ToVector2() - lastRotation;

                rotation.X += delta.X / 200;
                rotation.Y += delta.Y / 200;
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                position += new Vector3((float)Math.Cos(rotation.X) * 4, 0, (float)Math.Sin(rotation.X) * 4) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                position += new Vector3(-(float)Math.Cos(rotation.X) * 4, 0, -(float)Math.Sin(rotation.X) * 4) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                position += new Vector3((float)Math.Cos(rotation.X - Math.PI / 2) * 4, 0, (float)Math.Sin(rotation.X - Math.PI / 2) * 4) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                position += new Vector3((float)Math.Cos(rotation.X + Math.PI / 2) * 4, 0, (float)Math.Sin(rotation.X + Math.PI / 2) * 4) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                position += new Vector3(0, 4f, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyboardState.IsKeyDown(Keys.LeftShift))
            {
                position += new Vector3(0, -4f, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            view = Matrix.CreateLookAt(position, position + new Vector3((float)Math.Cos(rotation.X) * 3, -(float)Math.Tan(rotation.Y) * 3, (float)Math.Sin(rotation.X) * 3), new Vector3(0, 1, 0));

            lastRotation = mouseState.Position.ToVector2();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            RasterizerState rasterizer = new RasterizerState();
            //rasterizer.CullMode = CullMode.None;
            //rasterizer.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rasterizer;

            effect.VertexColorEnabled = true;
            effect.View = view;
            effect.Projection = projection;
            effect.World = Matrix.Identity;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                terrain.Render(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
