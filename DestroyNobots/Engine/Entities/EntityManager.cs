using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace DestroyNobots.Engine.Entities
{
    class EntityManager : IRenderable, IUpdateable
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

        public void Render(GameTime gt)
        {
            var entityContainer = entities.First;

            while(entityContainer != null)
            {
                entityContainer.Value.Render(gt);

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
