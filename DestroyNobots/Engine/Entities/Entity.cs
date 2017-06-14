using DestroyNobots.Engine.Physics;
using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine.Entities
{
    public abstract class Entity : IUpdateable, IRenderable, IRendererServicesProvider
    {
        public bool Destroyed { get; private set; }
        public DestroyNobotsGame Game { get; set; }
        public RendererServiceContainer RendererServices { get; private set; }
        public Transform Transform { get; set; }
        public virtual Rectangle BoundingRectangle { get { return Rectangle.Empty; } }
        public IPhysics Physics { get; protected set; }

        public Vector2 Front { get; protected set; }
        public Vector2 Forward { get { return Vector2.Transform(Front, Matrix.CreateRotationZ(Transform.Rotation)); } }

        public Entity()
        {
            Transform = new Transform();

            RendererServices = new RendererServiceContainer();
            RendererServices.Add<EntityRenderer>();
        }

        public virtual void Initialize()
        {

        }

        public virtual void Update(GameTime gt)
        {

        }

        public virtual void Draw(GameTime gt)
        {

        }
    }
}
