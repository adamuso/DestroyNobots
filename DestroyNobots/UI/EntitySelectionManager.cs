using System;
using Microsoft.Xna.Framework;
using DestroyNobots.Engine.Entities;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using DestroyNobots.Engine.Input;

namespace DestroyNobots.UI
{
    public class EntitySelectionManager : IUpdateable
    {
        private List<Entity> selected;

        public GUI GUI { get; set; }

        public EntitySelectionManager()
        {
            selected = new List<Entity>();
        }

        public void Clear()
        {
            foreach(Entity entity in selected)
            {
                entity.RendererServices.Remove<EntitySelectionRenderer>();
            }

            selected.Clear();
        }

        public void Add(Entity entity)
        {
            selected.Add(entity);
            entity.RendererServices.Add(new EntitySelectionRenderer(entity));
        }

        public void Remove(Entity entity)
        {
            entity.RendererServices.Remove<EntitySelectionRenderer>();
            selected.Remove(entity);
        }

        public void Update(GameTime gt)
        {
            if (GUI.Game.InputManager.MouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 inGamePosition = Vector2.Transform(GUI.Game.InputManager.GetMousePosition(), Matrix.Invert(GUI.Game.Camera.View));

                Entity entity = GUI.Game.EntityManager.GetEntityAtPosition(inGamePosition);

                if (entity != null)
                {
                    if (!GUI.Game.InputManager.IsKeyDown(ActionKey.MultiSelect))
                    {
                        Clear();
                        Add(entity);
                    }
                    else
                    {
                        if (selected.Contains(entity))
                            Remove(entity);
                        else
                            Add(entity);
                    }
                }
                else
                    Clear();

            }
        }
    }
}
