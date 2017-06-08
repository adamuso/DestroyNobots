using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine
{
    public class RendererServiceContainer : IRenderer
    {
        private static Dictionary<Type, IRenderer> rendererInstances;
        private Dictionary<Type, IRenderer> renderers;

        static RendererServiceContainer()
        {
            rendererInstances = new Dictionary<Type, IRenderer>();
        }

        public RendererServiceContainer()
        {
            renderers = new Dictionary<Type, IRenderer>();
        }

        public void Add<T>() where T : IRenderer, new()
        {
            IRenderer value;
            rendererInstances.TryGetValue(typeof(T), out value);
    
            if(value == null)
            {
                value = new T();
                rendererInstances.Add(typeof(T), value);
            }

            renderers.Add(typeof(T), value);
        }

        public void Add(IRenderer renderer)
        {
            renderers.Add(renderer.GetType(), renderer);
        }

        public void Remove(IRenderer renderer)
        {
            if(renderers.ContainsKey(renderer.GetType()))
                renderers.Remove(renderers.GetType());
        }

        public void Remove<T>()
        {
            if (renderers.ContainsKey(typeof(T)))
                renderers.Remove(typeof(T));
        }

        public void Draw(IRenderable renderable, GameTime gt)
        {
            foreach (IRenderer renderer in renderers.Values)
            {
                renderer.Draw(renderable, gt);
            }
        }
    }
}