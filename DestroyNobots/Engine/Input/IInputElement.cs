using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine.Input
{
    public interface IInputElement
    {
        bool IsFocusable { get; }
        Rectangle Bounds { get; }

        void OnKeyDown(KeyboardEventArgs e);
        void OnKeyUp(KeyboardEventArgs e);
        void OnMouseMove(MouseEventArgs e);
        void OnMouseDown(MouseEventArgs e);
        void OnMouseUp(MouseEventArgs e);
    }
}
