using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace DestroyNobots.Engine
{
    public class Camera : IUpdateable
    {
        public DestroyNobotsGame Game { get; set; }
        public Vector2 Position { get; set; }
        public Point SceneSize { get; set; }

        public Matrix View { get { return Matrix.CreateTranslation(new Vector3(Position, 0f)) * Matrix.CreateScale((float)Game.GraphicsDevice.Viewport.Width / SceneSize.X, (float)Game.GraphicsDevice.Viewport.Height / SceneSize.Y, 1); } }

        public void Update(GameTime gt)
        {
            MouseState state = Mouse.GetState();
            
            if(state.X < Game.GraphicsDevice.Viewport.X + 50)
            {
                Position += new Vector2(1f, 0);
            }

            if (state.X > Game.GraphicsDevice.Viewport.X + Game.GraphicsDevice.Viewport.Width - 50)
            {
                Position -= new Vector2(1f, 0);
            }

            if (state.Y < Game.GraphicsDevice.Viewport.Y + 50)
            {
                Position += new Vector2(0, 1f);
            }

            if (state.Y > Game.GraphicsDevice.Viewport.Y + Game.GraphicsDevice.Viewport.Height - 50)
            {
                Position -= new Vector2(0, 1f);
            }
        }    
    }
}
