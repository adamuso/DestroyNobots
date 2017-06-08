using Microsoft.Xna.Framework.Input;
using System;

namespace DestroyNobots.Engine.Input
{
    public class KeyboardEventArgs : EventArgs
    {
        public byte KeyCode { get { return (byte)Key; } }
        public Keys Key { get; private set; }
        public KeyboardState State { get; private set; }

        public bool Shift { get { return State.IsKeyDown(Keys.LeftShift) || State.IsKeyDown(Keys.RightShift); } }
        public bool Control { get { return State.IsKeyDown(Keys.LeftControl) || State.IsKeyDown(Keys.RightControl); } }
        public bool CapsLock { get { return State.CapsLock; } }

        public KeyboardEventArgs(Keys key, KeyboardState state)
        {
            Key = key;
            State = state;
        }
    }
}