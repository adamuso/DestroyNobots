using DestroyNobots.Engine.Physics;

namespace DestroyNobots.Engine.Entities.Vehicles
{
    public class Vehicle : Robot
    {
        protected float power;
        protected float rotation;

        protected new TopDown2DPhysics Physics { get { return (TopDown2DPhysics)base.Physics; } }

        public Vehicle()
        {
            base.Physics = new TopDown2DPhysics();
        }
    }
}
