using DestroyNobots.Computers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DestroyNobots.Engine.Entities.Vehicles
{
    public class Buggy : Vehicle
    {
        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            Game.SpriteBatch.Draw(Game.TextureManager.BuggyTexture, new Vector2(200, 200), null, Color.White, rotation / 360.0f, Vector2.Zero, 1, SpriteEffects.None, 0.0f);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            rotation = Computer.GetSpecificProcessor<VCM86Processor>().Registers[2].Value;
            power = Computer.GetSpecificProcessor<VCM86Processor>().Registers[3].Value;
        }
    }
}
