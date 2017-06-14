using DestroyNobots.Engine.Entities;
using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine.Physics
{
    public interface IPhysics : IUpdateable
    {
        void Apply(Collider collider);
        bool IsDirty { get; }
    }
}