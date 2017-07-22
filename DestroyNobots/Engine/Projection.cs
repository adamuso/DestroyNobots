using Microsoft.Xna.Framework;
using System.Linq;

namespace DestroyNobots.Engine
{
    public class Projection
    {
        private float start;
        private float end;

        private Projection(float start, float end)
        {
            this.start = start;
            this.end = end;
        }

        public float? GetOverlap(Projection projection)
        {
            // TODO: optimize range intersection

            if (start <= projection.start && projection.end <= end)
                return projection.end - projection.start;
            else if (projection.start <= start && end <= projection.end)
                return end - start;
            else if (start <= projection.end && projection.end <= end)
                return projection.end - start;
            else if (start <= projection.start && projection.start <= end)
                return end - projection.start;

            return null;
        }

        public bool Contains(float value)
        {
            return value >= start && value <= end;
        }

        public bool Contains(Projection projection)
        {
            return start >= projection.start && end <= projection.end;
        }

        public static Projection Project(Vector2[] points, Vector2 pointsOffset, Vector2 axis)
        {
            axis.Normalize();

            float[] dots = Vector2Extensions.Dot(points.Select(p => p + pointsOffset).ToArray(), axis);
            return new Projection(dots.Min(), dots.Max());
        }

        public static Projection Project(Vector2[] points, Vector2 axis)
        {
            return Project(points, Vector2.Zero, axis);
        }
    }
}