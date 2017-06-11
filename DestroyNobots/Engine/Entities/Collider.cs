using DestroyNobots.Engine.Entities;
using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine.Entities
{
    public class Collider : Entity
    {
        public override Rectangle BoundingRectangle { get { return CollisionArea.GetBounds(); } }
        public Region CollisionArea { get; private set; }

        public Collider()
        {
            CollisionArea = new Region();
            CollisionArea.Transform = Transform;
        }
    }
}
