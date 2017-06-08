using System;
using Microsoft.Xna.Framework;

namespace DestroyNobots.UI
{
    public class GUI : IUpdateable, IRenderable
    {
        public DestroyNobotsGame Game { get; set; }

        public EntitySelectionManager EntitySelectionManager  { get; private set; }

        public GUI()
        {
            EntitySelectionManager = new EntitySelectionManager();
        }

        public void Update(GameTime gt)
        {

        }

        public void Draw(GameTime gt)
        {

        }
    }
}
