using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace DestroyNobots.Engine.Entities
{
    public class EntityManager : IRenderable, IUpdateable
    {
        LinkedList<Entity> entities;
        public DestroyNobotsGame Game { get; set; }

        public EntityManager()
        {
            entities = new LinkedList<Entity>();
        }

        public T Create<T>() where T : Entity, new()
        {
            T entity = new T() { Game = Game };
            entities.AddLast(entity);

            return entity;
        }

        public Entity GetEntityAtPosition(Vector2 position)
        {
            foreach(Entity entity in entities)
            {
                if(entity.BoundingRectangle.Contains(position))
                {
                    return entity;
                }
            }

            return null;
        }

        public void Draw(GameTime gt)
        {
            var entityContainer = entities.First;

            while(entityContainer != null)
            {
                entityContainer.Value.RendererServices.Draw(entityContainer.Value, gt);

                entityContainer = entityContainer.Next;
            }
        }

        public void Update(GameTime gt)
        {
            var entityContainer = entities.First;

            while (entityContainer != null)
            {
                entityContainer.Value.Update(gt);

                if (entityContainer.Value.Destroyed)
                {
                    var next = entityContainer.Next;
                    entities.Remove(entityContainer);
                    entityContainer = next;
                }
                else
                    entityContainer = entityContainer.Next;
            }
        }
    }
}
