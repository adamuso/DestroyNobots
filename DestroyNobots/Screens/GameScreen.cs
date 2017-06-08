using DestroyNobots.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DestroyNobots.Screens
{
    public class GameScreen : Screen
    {
        GUI gui;

        public GameScreen()
        {
            gui = new GUI() { Screen = this };
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            Game.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, null, null, Game.Camera.View);
            Game.Level.Draw(gt);
            Game.EntityManager.Draw(gt);
            Game.SpriteBatch.End();

            Game.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default);
            gui.Draw(gt);
            Game.SpriteBatch.End();
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            Game.Camera.Update(gt);
            Game.EntityManager.Update(gt);
            gui.Update(gt);
        }
    }
}
