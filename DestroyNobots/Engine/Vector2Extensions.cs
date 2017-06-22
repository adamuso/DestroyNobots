using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine
{
    public static class Vector2Extensions
    {
        public static Vector2 ProjectInto(this Vector2 vec, Vector2 vec2)
        {
            vec2.Normalize();

            return Vector2.Dot(vec, vec2) * vec2;
        }

        public static float[] Dot(Vector2[] vec, Vector2 vec2)
        {
            float[] result = new float[vec.Length];
            vec2.Normalize();

            for (int i = 0; i < vec.Length; i++)
                result[i] = Vector2.Dot(vec[i], vec2);

            return result;
        }

        public static Vector2 RightNormal(this Vector2 vec)
        {
            return new Vector2(-vec.Y, vec.X);
        }

        public static Vector2 LeftNormal(this Vector2 vec)
        {
            return new Vector2(vec.Y, -vec.X);
        }
    }
}
