using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace DestroyNobots.Engine
{
    public class Region
    {
        private HashSet<Rectangle> rectangles;

        public Transform Transform { get; set; }

        public Region()
        {
            rectangles = new HashSet<Rectangle>();
            Transform = new Transform();
        }

        public void Add(Rectangle rectangle)
        {
            rectangles.Add(rectangle);
        }

        public Rectangle GetBounds()
        {
            Rectangle bounds = Rectangle.Empty;

            foreach (Rectangle rectangle in rectangles)
            {
                Rectangle transformed = Transform.Apply(rectangle);

                bounds.X = transformed.X < bounds.X ? transformed.X : bounds.X;
                bounds.Y = transformed.Y < bounds.Y ? transformed.Y : bounds.Y;
                bounds.Width = transformed.Right > bounds.Right ? (transformed.Right - bounds.X) : bounds.Width;
                bounds.Height = transformed.Bottom > bounds.Bottom ? (transformed.Bottom - bounds.Y) : bounds.Height;
            }

            return bounds;
        }

        public bool Contains(Point point)
        {
            foreach (Rectangle rectangle in rectangles)
            {
                if (Transform.Apply(rectangle).Contains(point))
                    return true;
            }

            return false;
        }

        public bool Contains(Rectangle rect)
        {
            foreach (Rectangle rectangle in rectangles)
            {
                if (Transform.Apply(rectangle).Contains(rect))
                    return true;
            }

            return false;
        }

        public bool Intersects(Rectangle rect)
        {
            foreach (Rectangle rectangle in rectangles)
            {
                if (Transform.Apply(rectangle).Intersects(rect))
                    return true;
            }
            
            return false;
        }

        public bool Intersects(Region region)
        {
            foreach(Rectangle rectangle1 in region.rectangles)
                foreach(Rectangle rectangle2 in region.rectangles)
                    if (Transform.Apply(rectangle1).Intersects(Transform.Apply(rectangle2)))
                        return true;

            return false;
        }

        public bool Contains(Region region)
        {
            foreach (Rectangle rectangle1 in region.rectangles)
                foreach (Rectangle rectangle2 in region.rectangles)
                    if (!Transform.Apply(rectangle1).Contains(Transform.Apply(rectangle2)))
                        return false;

            return true;
        }

        public Rectangle? GetIntersectingRectangleWith(Rectangle rect)
        {
            foreach (Rectangle rectangle in rectangles)
            {
                if (Transform.Apply(rectangle).Intersects(rect))
                    return rectangle;
            }

            return null;
        }

        public Tuple<Rectangle, Rectangle> GetIntersectingRectanglesWith(Region region)
        {
            foreach (Rectangle rectangle1 in rectangles)
                foreach (Rectangle rectangle2 in region.rectangles)
                    if (Transform.Apply(rectangle1).Intersects(Transform.Apply(rectangle2)))
                        return new Tuple<Rectangle, Rectangle>(rectangle1, rectangle2);

            return null;
        }
    }
}