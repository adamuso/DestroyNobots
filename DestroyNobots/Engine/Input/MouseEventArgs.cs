using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace DestroyNobots.Engine.Input
{
    public class MouseEventArgs : EventArgs
    {
        public MouseButtons Button { get; private set; }
        public MouseState State { get; private set; }

        public MouseEventArgs(MouseButtons button, MouseState state)
        {
            Button = button;
            State = state;
        }
    }
}