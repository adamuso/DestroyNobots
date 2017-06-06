using Microsoft.Xna.Framework;
using System;

namespace DestroyNobots.Engine
{
    public class Camera
    {
        public Vector3 Position { get; set; }

        public Matrix View { get { return Matrix.CreateTranslation(Position); } }
    }
}
