using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DestroyNobots.Engine.Input
{
    public class InputManager
    {
        private Dictionary<ActionKey, Keys> keyMapping;

        public InputManager()
        {
            keyMapping = new Dictionary<ActionKey, Keys>();
        }

        public bool IsKeyDown(ActionKey key)
        {
            return Keyboard.GetState().IsKeyDown(keyMapping[key]);
        }

        public bool IsKeyUp(ActionKey key)
        {
            return Keyboard.GetState().IsKeyUp(keyMapping[key]);
        }

        public Vector2 GetMousePosition()
        {
            Point position = Mouse.GetState().Position;

            return new Vector2(position.X, position.Y);
        }

        public MouseState MouseState { get { return Mouse.GetState(); } }
    }
}
