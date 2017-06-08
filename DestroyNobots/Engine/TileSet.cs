using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DestroyNobots.Engine
{
    public class TileSet
    {
        private int tileSize;
        private int width;
        private int height;

        public Texture2D Texture { get; private set; }

        public TileSet(Texture2D texture, int tileSize)
        {
            this.tileSize = tileSize;
            Texture = texture;

            width = texture.Width / tileSize;
            height = texture.Height / tileSize;
        }

        public Rectangle GetSourceRectangle(int type)
        {
            return new Rectangle(type % width * tileSize, type / width * tileSize, tileSize, tileSize);
        }    

        public static TileSet Load(string file)
        {
            return null;
        }
    }
}