using System;
using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine
{
    public class Level : IRenderable
    {
        private const int TileSize = 256;

        Tile[] tiles;
        int width;
        int height;

        public TileSet TileSet { get; set; }
        public DestroyNobotsGame Game { get; set; }

        public Level(int width, int height)
        {
            tiles = new Tile[width * height];
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tiles[x + y * width] = new Tile() { Type = 0 };
        }

        public void Draw(GameTime gt)
        {
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    Game.SpriteBatch.Draw(TileSet.Texture, new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize), tiles[x + y * width].GetSourceRectangle(TileSet), Color.White);
                }
            }
        }

        public Tile this[int index] { get { return tiles[index]; } set { tiles[index] = value; } }

        public Tile this[int x, int y] { get { return tiles[x + y * width]; } set { tiles[x + y * width] = value; } }

        public static Level Load(string file)
        {
            return null;
        }
    }
}
