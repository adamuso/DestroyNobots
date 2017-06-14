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
                Polygon transformed = Transform.Apply(rectangle);

                bounds.X = transformed.Bounds.X < bounds.X ? transformed.Bounds.X : bounds.X;
                bounds.Y = transformed.Bounds.Y < bounds.Y ? transformed.Bounds.Y : bounds.Y;
                bounds.Width = transformed.Bounds.Right > bounds.Right ? (transformed.Bounds.Right - bounds.X) : bounds.Width;
                bounds.Height = transformed.Bounds.Bottom > bounds.Bottom ? (transformed.Bounds.Bottom - bounds.Y) : bounds.Height;
            }

            return bounds;
        }

        public bool Contains(Vector2 point)
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

        public Vector2? GetIntersection(Polygon polygon)
        {
            foreach (Rectangle rectangle in rectangles)
            {
                Vector2? intersection = Transform.Apply(rectangle).GetIntersection(polygon);

                if (intersection != null)
                    return intersection;
            }

            return null;
        }

        public Vector2? GetIntersection(Region region)
        {
            foreach (Rectangle rectangle1 in rectangles)
                foreach (Rectangle rectangle2 in region.rectangles)
                {
                    Vector2? intersection = Transform.Apply(rectangle1).GetIntersection(Transform.Apply(rectangle2));

                    if (intersection != null)
                        return intersection;
                }

            return null;
        }
    }
}