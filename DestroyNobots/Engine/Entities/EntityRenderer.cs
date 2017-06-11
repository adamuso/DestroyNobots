using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine.Entities
{
    public class EntityRenderer : IRenderer
    {
        public int Priority { get; set; }

        public EntityRenderer()
        {
            Priority = 0;
        }

        public void Draw(IRenderable renderable, GameTime gt)
        {
            

            renderable.Draw(gt);
        }
    }
}