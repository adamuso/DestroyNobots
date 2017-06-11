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

        public Entity()
        {
            Transform = new Transform();

            RendererServices = new RendererServiceContainer();
            RendererServices.Add<EntityRenderer>();
        }

        public virtual void Update(GameTime gt)
        {

        }

        public virtual void Draw(GameTime gt)
        {

        }
    }
}
