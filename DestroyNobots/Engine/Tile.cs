using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DestroyNobots.Engine
{
    public class Tile
    {
        public int Type { get; set; }

        public Rectangle GetSourceRectangle(TileSet tileSet)
        {
            return tileSet.GetSourceRectangle(Type);
        }
    }
}