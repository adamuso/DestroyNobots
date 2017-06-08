using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DestroyNobots.Screens
{
    public class GameScreen : Screen
    {
        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            Game.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, null, null, Game.Camera.View);
            Game.Level.Draw(gt);
            Game.EntityManager.Draw(gt);
            Game.SpriteBatch.End();
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            Game.EntityManager.Update(gt);
        }
    }
}
