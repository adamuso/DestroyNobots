using DestroyNobots.Engine.Entities;
using Microsoft.Xna.Framework;
using DestroyNobots.Engine;

namespace DestroyNobots.UI
{
    public class EntitySelectionRenderer : IRenderer
    {
        private Entity entity;

        public EntitySelectionRenderer(Entity entity)
        {
            this.entity = entity;
        }

        public void Draw(IRenderable renderable, GameTime gt)
        {

        }
    }
}
