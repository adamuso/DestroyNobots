using DestroyNobots.Assembler.Emulator;
using DestroyNobots.Assembler.Emulator.Peripherals;
using Microsoft.Xna.Framework;

namespace DestroyNobots.Engine.Entities.Vehicles
{
    public class Buggy : Vehicle, IPeripheral
    {
        public override Rectangle BoundingRectangle { get { return new Rectangle(Transform.Position.ToPoint(), new Point(32, 32)); } }

        public Buggy()
        {
            Transform.Position = new Vector2(200, 200);
            Transform.Origin = new Vector2(16, 24);
            Computer.ConnectPeripheral(this);
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            Vector2 point = Vector2.Transform(new Vector2(0, 0), Transform.Matrix);

            Game.SpriteBatch.Draw(Game.TextureManager.BuggyTexture, Transform.Position, null, Color.White, Transform.Rotation, Transform.Origin, Transform.Scale, Transform.Effect, Transform.Depth);
        }

        public void Install()
        {
            Computer.Ports[0] = new PeripheralPortHandler(this)
            {
                Out = (value, size) =>
                {
                    Transform.Rotation = value / 360.0f;
                }
            };
        }

        public void Uninstall()
        {
            Computer.Ports.Remove(0);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }
    }
}
