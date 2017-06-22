using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine
{
    public interface IRenderer
    {
        int Priority { get; }

        void Draw(IRenderable renderable, GameTime gt);
    }
}