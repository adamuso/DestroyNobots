using DestroyNobots.Engine.Collision;
using DestroyNobots.Engine.Entities;
using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine.Entities
{
    public class Collider : Entity
    {
        private bool isDirty;

        public override Rectangle BoundingRectangle { get { return CollisionArea.GetBounds(); } }
        public Region CollisionArea { get; private set; }
        public virtual bool CanMove { get { return true; } }

        public Collider()
        {
            CollisionArea = new Region();
            CollisionArea.Transform = Transform;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            Physics.Update(gt);

            if (isDirty || Physics.IsDirty)
            {
                UpdatePhysics();
                MakeClean();
            }
        }

        protected virtual void UpdatePhysics()
        {
            if(Physics != null)
                Physics.Apply(this);
        }

        protected virtual void MakeDirty()
        {
            isDirty = true;
        }

        protected virtual void MakeClean()
        {
            Game.CollisionManager.AddCollisionCheckFor(this);
            isDirty = false;
        }
    }
}
