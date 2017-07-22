using Microsoft.Xna.Framework;

namespace DestroyNobots
{
    public interface IRenderable
    {
        DestroyNobotsGame Game { get; }
        void Draw(GameTime gt);
    }
}
