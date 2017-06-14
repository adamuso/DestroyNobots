using DestroyNobots.Engine.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DestroyNobots.Engine.Collision
{
    public class CollisionManager
    {
        private List<Collider> collisions;
        private EntityManager entities;

        public CollisionManager(EntityManager entities)
        {
            this.entities = entities;
            collisions = new List<Collider>();
        }

        public void AddCollisionCheckFor(Collider collider)
        {
            if(!collisions.Contains(collider))
                collisions.Add(collider);
        }

        public void ResolveCollision(Collider collider1, Collider collider2)
        {
            if (!collider1.BoundingRectangle.Intersects(collider2.BoundingRectangle) &&
                !collider1.BoundingRectangle.Contains(collider2.BoundingRectangle) &&
                !collider2.BoundingRectangle.Contains(collider1.BoundingRectangle))
                return;

            if (collider1 is StaticCollider && collider2 is StaticCollider)
                return;

            if(collider1 is StaticCollider || collider2 is StaticCollider)
            {
                StaticCollider staticCollider = (StaticCollider)(collider1 is StaticCollider ? collider1 : collider2);
                Collider collider = (StaticCollider)(!(collider1 is StaticCollider) ? collider1 : collider2);

                Vector2? intersection = collider.CollisionArea.GetIntersection(staticCollider.CollisionArea);

                if (intersection == null)
                    return;


            }
            else
            {

            }
        }
    }
}
