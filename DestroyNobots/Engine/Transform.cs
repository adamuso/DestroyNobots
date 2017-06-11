using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DestroyNobots.Engine
{
    public class Transform
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public SpriteEffects Effect { get; set; }
        public float Depth { get; set; }

        public Matrix Matrix { get { return Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                                            Matrix.CreateScale(Scale.X, Scale.Y, 1) *
                                            Matrix.CreateRotationZ(Rotation) * 
                                            Matrix.CreateTranslation(new Vector3(Position, 0)); } }

        public Matrix NonRotationMatrix { get { return Matrix.CreateTranslation(new Vector3(-Origin, 0)) * 
                                                       Matrix.CreateScale(Scale.X, Scale.Y, 1) *
                                                       Matrix.CreateTranslation(new Vector3(Position, 0)); } }

        public Transform()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Origin = Vector2.Zero;
            Rotation = 0;
            Effect = SpriteEffects.None;
            Depth = 0;
        }

        public Vector2 Apply(Vector2 vector)
        {
            return Vector2.Transform(vector, Matrix);
        }

        public Point Apply(Point point)
        {
            return Vector2.Transform(point.ToVector2(), Matrix).ToPoint();
        }

        public Rectangle Apply(Rectangle rectangle)
        {
            Vector2 topLeft = rectangle.Location.ToVector2();
            Vector2 bottomRight = (rectangle.Location + rectangle.Size).ToVector2();
            topLeft = Vector2.Transform(topLeft, NonRotationMatrix);
            bottomRight = Vector2.Transform(bottomRight, NonRotationMatrix);

            return new Rectangle(topLeft.ToPoint(), (bottomRight - topLeft).ToPoint());
        }
    }
}