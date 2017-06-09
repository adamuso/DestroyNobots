using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace DestroyNobots.Engine.Input
{
    public class InputElementManager : IInputElement
    {
        private IInputElement root;

        Rectangle IInputElement.Bounds { get { return Rectangle.Empty; } }
        public IInputElement Root { get { return root; } set { root = value; PrepareRoot(); } }
        public IInputElement Focused { get; private set; }

        public InputElementManager()
        {
            root = null;
        }

        private void PrepareRoot()
        {
            if(root is IInputElementContainer)
            {
                Focused = null;
            }
            else
            {
                Focused = Root;
            }
        }

        private void FocusOnPosition(Point position)
        {
            if (root is IInputElementContainer)
            {
                Focused = null;

                IInputElementContainer container = (IInputElementContainer)root;

                FocusOnPositionInContainer(position, container);
            }
        }

        private void FocusOnPositionInContainer(Point position, IInputElementContainer container)
        {
            if (container.Bounds.Contains(position))
            {
                foreach (IInputElement child in container.Children.Reverse())
                {
                    if(child.Bounds.Contains(position))
                    {
                        if (child is IInputElementContainer)
                        {
                            FocusOnPositionInContainer(position, (IInputElementContainer)child);

                            if (Focused != null)
                                break;
                        }
                        else
                        {
                            Focused = child;
                            break;
                        }
                    }
                }
            }
        }

        public void OnKeyDown(KeyboardEventArgs e)
        {
            if(Focused != null)
                Focused.OnKeyDown(e);
        }

        public void OnKeyUp(KeyboardEventArgs e)
        {
            if (Focused != null)
                Focused.OnKeyUp(e);
        }

        public void OnMouseDown(MouseEventArgs e)
        {
            FocusOnPosition(e.State.Position);

            if (Focused != null)
                Focused.OnMouseDown(e);
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            if (Focused != null)
                Focused.OnMouseMove(e);
        }

        public void OnMouseUp(MouseEventArgs e)
        {
            if (Focused != null)
                Focused.OnMouseUp(e);
        }
    }
}
