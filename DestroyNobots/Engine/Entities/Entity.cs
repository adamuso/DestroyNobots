using System;
using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine.Entities
{
    public abstract class Entity : IUpdateable, IRenderable
    {
        public bool Destroyed { get; private set; }
        public DestroyNobotsGame Game { get; set; }

        public virtual void Update(GameTime gt)
        {

        }

        public virtual void Render(GameTime gt)
        {

        }
    }
}
