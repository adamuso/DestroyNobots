using DestroyNobots.Engine.Entities;
using Microsoft.Xna.Framework;
using DestroyNobots.Engine;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DestroyNobots.UI
{
    public class EntitySelectionRenderer : IRenderer
    {
        private Texture2D texture;
        private Entity entity;

        public int Priority { get; set; }

        public EntitySelectionRenderer(Entity entity)
        {
            this.entity = entity;
            Priority = 0;

            texture = new Texture2D(entity.Game.GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });
        }

        public void Draw(IRenderable renderable, GameTime gt)
        {
            if(renderable == entity)
            {
                renderable.Game.SpriteBatch.Draw(texture, entity.BoundingRectangle, Color.White);
            }
        }
    }
}
