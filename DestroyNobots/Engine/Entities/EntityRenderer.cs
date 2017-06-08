using System;
using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine.Entities
{
    public class EntityRenderer : IRenderer
    {
        public void Draw(IRenderable renderable, GameTime gt)
        {
            renderable.Draw(gt);
        }
    }
}