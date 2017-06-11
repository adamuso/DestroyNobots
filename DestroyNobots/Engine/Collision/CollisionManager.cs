using DestroyNobots.Engine.Entities;
using Microsoft.Xna.Framework;
using System;

namespace DestroyNobots.Engine.Collision
{
    public class CollisionManager
    {
        private EntityManager entities;

        public CollisionManager(EntityManager entities)
        {
            this.entities = entities;
        }

        public void ResolveCollision(Collider collider1, Collider collider2)
        {
            if (!collider1.BoundingRectangle.Intersects(collider2.BoundingRectangle) &&
                !collider1.BoundingRectangle.Contains(collider2.BoundingRectangle) &&
                !collider2.BoundingRectangle.Contains(collider1.BoundingRectangle))
                return;

            Tuple<Rectangle, Rectangle> intersection = collider1.CollisionArea.GetIntersectingRectanglesWith(collider2.CollisionArea);

            if(intersection != null)
            {

            }
        }
    }
}
