using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine
{
    public interface IRenderer
    {
        void Draw(IRenderable renderable, GameTime gt);
    }
}