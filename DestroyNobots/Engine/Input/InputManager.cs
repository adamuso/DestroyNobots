using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace DestroyNobots.Engine.Input
{
    public class InputManager : IUpdateable
    {
        private Dictionary<ActionKey, Keys> keyMapping;
        private KeyboardState oldKeyboardState;
        private MouseState oldMouseState;
        private Keys lastKey;
        private int lastKeyAccumulator;
        private int lastKeyRepeatAccumulator;

        public MouseState MouseState { get { return Mouse.GetState(); } }
        public bool UseEvents { get; set; }
        public InputElementManager InputElementManager { get; private set; }

        public InputManager()
        {
            keyMapping = new Dictionary<ActionKey, Keys>();
            InputElementManager = new InputElementManager();
            UseEvents = false;
        }

        public bool IsKeyDown(ActionKey key)
        {
            Keys value;
            keyMapping.TryGetValue(key, out value);

            if (value == Keys.None)
                return false;

            return Keyboard.GetState().IsKeyDown(value);
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

        public void Update(GameTime gt)
        {
            if (UseEvents)
            {
                KeyboardState newKeyboardState = Keyboard.GetState();

                if (lastKeyAccumulator < 400)
                {
                    lastKeyAccumulator += (int)gt.ElapsedGameTime.TotalMilliseconds;
                    lastKeyRepeatAccumulator = 0;
                }
                else if(lastKey != Keys.None)
                {
                    lastKeyRepeatAccumulator += (int)gt.ElapsedGameTime.TotalMilliseconds;

                    while (lastKeyRepeatAccumulator > 30)
                    {
                        InputElementManager.OnKeyDown(new KeyboardEventArgs(lastKey, newKeyboardState));
                        lastKeyRepeatAccumulator -= 30;
                    }
                }

                for (int i = 0; i <= 255; i++)
                {
                    if (newKeyboardState.IsKeyDown((Keys)i))
                    {
                        if (!oldKeyboardState.IsKeyDown((Keys)i))
                        {
                            lastKeyAccumulator = 0;
                            lastKeyRepeatAccumulator = 0;
                            lastKey = (Keys)i;
                            InputElementManager.OnKeyDown(new KeyboardEventArgs((Keys)i, newKeyboardState));
                        }
                    }
                    else if (oldKeyboardState.IsKeyDown((Keys)i))
                    {
                        if ((Keys)i == lastKey)
                        {
                            lastKeyAccumulator = 0;
                            lastKeyRepeatAccumulator = 0;
                            lastKey = Keys.None;
                        }

                        InputElementManager.OnKeyUp(new KeyboardEventArgs((Keys)i, newKeyboardState));
                    }
                }

                oldKeyboardState = newKeyboardState;

                MouseState newMouseState = Mouse.GetState();

                if (newMouseState.X != oldMouseState.X || newMouseState.Y != oldMouseState.Y)
                    InputElementManager.OnMouseMove(new MouseEventArgs(MouseButtons.None, newMouseState));

                if (newMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (oldMouseState.LeftButton == ButtonState.Released)
                        InputElementManager.OnMouseDown(new MouseEventArgs(MouseButtons.Left, newMouseState));
                }
                else if (oldMouseState.LeftButton == ButtonState.Pressed)
                    InputElementManager.OnMouseUp(new MouseEventArgs(MouseButtons.Left, newMouseState));

                if (newMouseState.RightButton == ButtonState.Pressed)
                {
                    if (oldMouseState.RightButton == ButtonState.Released)
                        InputElementManager.OnMouseDown(new MouseEventArgs(MouseButtons.Right, newMouseState));
                }
                else if (oldMouseState.RightButton == ButtonState.Pressed)
                    InputElementManager.OnMouseUp(new MouseEventArgs(MouseButtons.Right, newMouseState));

                if (newMouseState.MiddleButton == ButtonState.Pressed)
                {
                    if (oldMouseState.MiddleButton == ButtonState.Released)
                        InputElementManager.OnMouseDown(new MouseEventArgs(MouseButtons.Middle, newMouseState));
                }
                else if (oldMouseState.MiddleButton == ButtonState.Pressed)
                    InputElementManager.OnMouseUp(new MouseEventArgs(MouseButtons.Middle, newMouseState));

                if (newMouseState.XButton1 == ButtonState.Pressed)
                {
                    if (oldMouseState.XButton1 == ButtonState.Released)
                        InputElementManager.OnMouseDown(new MouseEventArgs(MouseButtons.X1, newMouseState));
                }
                else if (oldMouseState.XButton1 == ButtonState.Pressed)
                    InputElementManager.OnMouseUp(new MouseEventArgs(MouseButtons.X1, newMouseState));

                if (newMouseState.XButton2 == ButtonState.Pressed)
                {
                    if (oldMouseState.XButton2 == ButtonState.Released)
                        InputElementManager.OnMouseDown(new MouseEventArgs(MouseButtons.X2, newMouseState));
                }
                else if (oldMouseState.XButton2 == ButtonState.Pressed)
                    InputElementManager.OnMouseUp(new MouseEventArgs(MouseButtons.X2, newMouseState));

                oldMouseState = newMouseState;
            }
        }


    }
}
