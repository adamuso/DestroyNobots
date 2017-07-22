using System;
using Microsoft.Xna.Framework;
using DestroyNobots.Screens;

namespace DestroyNobots.UI
{
    public class GUI : IUpdateable, IRenderable
    {
        public GameScreen Screen { get; set; }
        public DestroyNobotsGame Game { get { return Screen.Game; } }
        public EntitySelectionManager EntitySelectionManager  { get; private set; }
        public AssemblerEditor AssemblerEditor { get; private set; }

        public GUI()
        {
            EntitySelectionManager = new EntitySelectionManager() { GUI = this };
            AssemblerEditor = new AssemblerEditor() { GUI = this };
        }

        public void Update(GameTime gt)
        {
            //if (!Game.InputManager.UseEvents)
            //{
            //    Game.InputManager.UseEvents = true;
            //    AssemblerEditor.Show();
            //}

            EntitySelectionManager.Update(gt);
        }

        public void Draw(GameTime gt)
        {
            AssemblerEditor.Draw(gt);
        }
    }
}
