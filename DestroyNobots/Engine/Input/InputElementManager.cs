using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace DestroyNobots.Engine.Input
{
    public class InputElementManager : IInputElement
    {
        private IInputElement root;

        public bool IsFocusable { get { return false; } }
        Rectangle IInputElement.Bounds { get { return Rectangle.Empty; } }
        public IInputElement Root { get { return root; } set { root = value; PrepareRoot(); } }
        public IInputElement Focused { get; private set; }

        public InputElementManager()
        {
            root = null;
        }

        private void PrepareRoot()
        {
            if (Root.IsFocusable)
                Focused = Root;
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
                Focused = container;

                foreach (IInputElement child in container.Children.Reverse())
                {
                    if(child.Bounds.Contains(position))
                    {
                        if (child is IInputElementContainer)
                        {
                            if (child.IsFocusable)
                                Focused = child; 

                            FocusOnPositionInContainer(position, (IInputElementContainer)child);

                            if (Focused != null)
                                break;
                        }
                        else
                        {
                            if(child.IsFocusable)
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

            Root.OnMouseDown(e);
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            Root.OnMouseMove(e);
        }

        public void OnMouseUp(MouseEventArgs e)
        { 
            Root.OnMouseUp(e);
        }
    }
}
