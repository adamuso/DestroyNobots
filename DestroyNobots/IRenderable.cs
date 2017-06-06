using Microsoft.Xna.Framework;

namespace DestroyNobots
{
    interface IRenderable
    {
        DestroyNobotsGame Game { get; set; }
        void Draw(GameTime gt);
    }
}
