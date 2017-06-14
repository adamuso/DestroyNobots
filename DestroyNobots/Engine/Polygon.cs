using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DestroyNobots.Engine
{
    public class Polygon
    {
        private class AxisComparer : IEqualityComparer<Vector2>
        {
            public bool Equals(Vector2 x, Vector2 y)
            {
                if (Vector3.Cross(new Vector3(x, 0), new Vector3(y, 0)) == Vector3.Zero)
                    return true;

                return false;
            }

            public int GetHashCode(Vector2 obj)
            {
                return obj.GetHashCode();
            }
        }

        private Vector2[] points;
        private Vector2 min;
        private Vector2 max;

        public Vector2[] Points { get; internal set; }
        public Rectangle Bounds { get { return new Rectangle(min.ToPoint(), new Point((int)Math.Ceiling(max.X), (int)Math.Ceiling(max.Y)) - min.ToPoint()); } }

        public Polygon(Vector2[] points)
        {
            this.points = points;

            min = new Vector2(points.Select(p => p.X).Min(), points.Select(p => p.Y).Min());
            max = new Vector2(points.Select(p => p.X).Max(), points.Select(p => p.Y).Max());
        }

        public Vector2? GetIntersection(Polygon polygon)
        {
            List<Vector2> axes = GetAxes().Concat(polygon.GetAxes()).Distinct(new AxisComparer()).ToList();
            Vector2? smallestNormal = null;
            float smallestOverlap = float.MaxValue;

            for(int i = 0; i < axes.Count; i++)
            {
                Projection projection1 = Projection.Project(points, axes[i]);
                Projection projection2 = Projection.Project(polygon.points, axes[i]);

                float? overlap = projection1.GetOverlap(projection2);

                if (overlap == null)
                    return null;

                if (smallestOverlap > overlap)
                {
                    smallestNormal = axes[i];
                    smallestOverlap = overlap.Value;
                }
            }
   
            Vector2 normal = smallestNormal.Value;
            normal.Normalize();
            
            // find the MTV
            Vector2 mtv1 = normal * (smallestOverlap + 1);
            Vector2 mtv2 = normal * -(smallestOverlap + 1);
            Projection project1 = Projection.Project(points, mtv1, normal);
            Projection project2 = Projection.Project(points, mtv2, normal);

            float? overlap2 = project1.GetOverlap(project2);

            if (overlap2 != null)
                return mtv2;
            else
                return mtv1;
        }

        public bool Intersects(Polygon polygon)
        {
            List<Vector2> axes = GetAxes().Concat(polygon.GetAxes()).Distinct(new AxisComparer()).ToList();

            for (int i = 0; i < axes.Count; i++)
            {
                Projection projection1 = Projection.Project(points, axes[i]);
                Projection projection2 = Projection.Project(polygon.points, axes[i]);

                float? overlap = projection1.GetOverlap(projection2);

                if (overlap == null)
                    return false;
            }

            return true;
        }

        public bool Contains(Vector2 point)
        {
            List<Vector2> axes = GetAxes().Distinct(new AxisComparer()).ToList();

            for (int i = 0; i < axes.Count; i++)
            {
                Projection projection1 = Projection.Project(points, axes[i]);
                Projection projection2 = Projection.Project(new Vector2[] { point }, axes[i]);

                if (!projection1.Contains(projection2))
                    return false;
            }

            return true;
        }

        public bool Contains(Polygon polygon)
        {
            List<Vector2> axes = GetAxes().Concat(polygon.GetAxes()).Distinct(new AxisComparer()).ToList();

            for (int i = 0; i < axes.Count; i++)
            {
                Projection projection1 = Projection.Project(points, axes[i]);
                Projection projection2 = Projection.Project(polygon.points, axes[i]);

                float? overlap = projection1.GetOverlap(projection2);

                if (overlap == null)
                    return false;

                if (!projection1.Contains(projection2) && !projection2.Contains(projection1))
                    return false;
            }

            return true;
        }

        public Vector2[] GetEdges()
        {
            Vector2[] edges = new Vector2[points.Length];

            for (int i = 0; i < points.Length; i++)
                edges[i] = points[(i + 1) % points.Length] - points[i];

            return edges;
        }

        public Vector2[] GetAxes()
        {
            Vector2[] edges = GetEdges();
            Vector2[] axes = new Vector2[edges.Length];

            for (int i = 0; i < edges.Length; i++)
                axes[i] = edges[i].RightNormal();

            return axes;
        }

        public static implicit operator Polygon(Rectangle rect)
        {
            return new Polygon(new Vector2[] { new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom), new Vector2(rect.Left, rect.Bottom) });
        }
    }
}
