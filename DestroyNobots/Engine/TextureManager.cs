using Microsoft.Xna.Framework.Graphics;

namespace DestroyNobots.Engine
{
    public class TextureManager
    {
        public DestroyNobotsGame Game { get; private set; }
        public Texture2D BuggyTexture { get; private set; }

        public TextureManager(DestroyNobotsGame game)
        {
            Game = game;
        }

        public void Load()
        {
            BuggyTexture = Game.Content.Load<Texture2D>("buggy");
        }

        public void Unload()
        {

        }
    }
}
