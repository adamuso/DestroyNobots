﻿using Microsoft.Xna.Framework;

namespace DestroyNobots
{
    public interface IRenderable
    {
        DestroyNobotsGame Game { get; set; }
        void Draw(GameTime gt);
    }
}
