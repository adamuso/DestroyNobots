using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System;
using System.Collections.Generic;

namespace DestroyNobots.Engine
{
    public class Terrain : IRenderable
    {
        private float[] data;
        private int width, height;
        private float scale;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        public DestroyNobotsGame Game { get; set; }

        private Terrain(int width, int height, float scale = 0.2f)
        {
            this.width = width;
            this.height = height;
            this.scale = scale;

            data = new float[width * height];
        }

        private void GenerateTerrain()
        {
            VertexPositionNormalColor[] vertices = data.Select(
                (val, i) => new VertexPositionNormalColor(
                    new Vector3((i % width - width / 2) * scale, val * 20, (i / width - height / 2) * scale),
                    new Vector3(0, 1, 0),
                    Color.White
                )).ToArray();

            int[] indices = PrepareIndices();
            PrepareNormals(vertices, indices);
        }

        private int[] PrepareIndices()
        {
            int[] indices = new int[(width - 1) * (height - 1) * 6];
            int counter = 0;
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    int lowerLeft = x + y * width;
                    int lowerRight = (x + 1) + y * width;
                    int topLeft = x + (y + 1) * width;
                    int topRight = (x + 1) + (y + 1) * width;

                    indices[counter++] = lowerLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = topLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = topRight;

                    //List<Vector3[]> triangles = new List<Vector3[]>();

                    //for(int ty = MathHelper.Clamp(y - 1, 0, height - 1); ty <= MathHelper.Clamp(y + 1, 0, height - 1); ty++)
                    //    for(int tx = MathHelper.Clamp(x - 1, 0, width - 1); tx <= MathHelper.Clamp(x + 1, 0, width - 1); tx++)
                    //    {
                    //        int ll = tx + ty * width;
                    //        int lr = (tx + 1) + ty * width;
                    //        int tl = tx + (ty + 1) * width;
                    //        int tr = (tx + 1) + (ty + 1) * width;

                    //        if (ll == x + y * width || lr == x + y * width || tl == x + y * width || tr == x + y * width)
                    //        {
                    //            triangles.Add(new Vector3[] { vertices[ll].Position, vertices[lr].Position, vertices[tl].Position });
                    //            triangles.Add(new Vector3[] { vertices[tl].Position, vertices[lr].Position, vertices[tr].Position });
                    //        }
                    //    }

                    //Vector3 avgNormal = Vector3.Zero;

                    //for(int i = 0; i < triangles.Count; i++)
                    //{
                    //    Vector3 normal = Vector3.Cross(triangles[i][2] - triangles[i][0], triangles[i][1] - triangles[i][0]);
                    //    avgNormal += normal;
                    //}

                    //avgNormal /= triangles.Count;
                    //avgNormal.Normalize();

                    //vertices[x + y * width].Normal = avgNormal;
                }
            }

            indexBuffer = new IndexBuffer(Game.GraphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);

            return indices;
        }

        private void PrepareNormals(VertexPositionNormalColor[] vertices, int[] indices)
        {
            int[] verticesNormalsCount = new int[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                verticesNormalsCount[index1]++;
                vertices[index1].Normal += normal;
                verticesNormalsCount[index2]++;
                vertices[index2].Normal += normal;
                verticesNormalsCount[index3]++;
                vertices[index3].Normal += normal;
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                //vertices[i].Normal /= verticesNormalsCount[i];
                vertices[i].Normal.Normalize();
            }

            vertexBuffer = new VertexBuffer(Game.GraphicsDevice, VertexPositionNormalColor.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }

        public static Terrain FromHeightmap(DestroyNobotsGame game, Texture2D texture, float scale = 0.2f)
        {
            Terrain t = new Terrain(texture.Width, texture.Height, scale) { Game = game };
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels, 0, texture.Width * texture.Height);

            t.data = pixels.Select(px => px.ToVector3()).Select(v => (v.X + v.Y + v.Z) / 3.0f - 0.5f).ToArray();

            t.GenerateTerrain();

            return t;
        }

        public void Render(GameTime gt)
        {
            Game.GraphicsDevice.Indices = indexBuffer;
            Game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indexBuffer.IndexCount / 3);
        }

    }
}
