using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace DestroyNobots.Engine.Input
{
    public class InputManager : IUpdateable
    {
        private Dictionary<ActionKey, Keys> keyMapping;
        private KeyboardState oldState;
        private Keys lastKey;
        private int lastKeyAccumulator;
        private int lastKeyRepeatAccumulator;

        public MouseState MouseState { get { return Mouse.GetState(); } }
        public bool UseEvents { get; set; }
        public IInputElement FocusedElement { get; set; }

        public InputManager()
        {
            keyMapping = new Dictionary<ActionKey, Keys>();
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
                KeyboardState newState = Keyboard.GetState();

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
                        FocusedElement.OnKeyDown(new KeyboardEventArgs(lastKey, newState));
                        lastKeyRepeatAccumulator -= 30;
                    }
                }

                for (int i = 0; i <= 255; i++)
                {
                    if (newState.IsKeyDown((Keys)i))
                    {
                        if (!oldState.IsKeyDown((Keys)i))
                        {
                            lastKeyAccumulator = 0;
                            lastKeyRepeatAccumulator = 0;
                            lastKey = (Keys)i;
                            FocusedElement.OnKeyDown(new KeyboardEventArgs((Keys)i, newState));
                        }
                    }
                    else if (oldState.IsKeyDown((Keys)i))
                    {
                        if ((Keys)i == lastKey)
                        {
                            lastKeyAccumulator = 0;
                            lastKeyRepeatAccumulator = 0;
                            lastKey = Keys.None;
                        }

                        FocusedElement.OnKeyUp(new KeyboardEventArgs((Keys)i, newState));
                    }
                }

                oldState = newState;
            }
        }


    }
}
