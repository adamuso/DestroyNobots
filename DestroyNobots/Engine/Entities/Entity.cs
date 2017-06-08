using System;
using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine.Entities
{
    public abstract class Entity : IUpdateable, IRenderable, IRendererServicesProvider
    {
        public bool Destroyed { get; private set; }
        public DestroyNobotsGame Game { get; set; }
        public RendererServiceContainer RendererServices { get; private set; }
        public Rectangle BoundingRectangle { get; protected set; }

        public Entity()
        {
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
