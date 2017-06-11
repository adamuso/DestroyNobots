using DestroyNobots.Assembler.Emulator;
using DestroyNobots.Assembler.Emulator.Peripherals;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DestroyNobots.Engine.Entities.Vehicles
{
    public class Buggy : Vehicle, IPeripheral
    {
        public override Rectangle BoundingRectangle { get { return new Rectangle(Transform.Position.ToPoint(), new Point(32, 32)); } }

		public bool IsDirty { get; private set; }

        public Buggy()
        {
            Transform.Position = new Vector2(400, 400);
            Computer.ConnectPeripheral(this);

			IsDirty = true;
		}

        public override void Draw(GameTime gt)
        {
			if(IsDirty) {
				IsDirty = false;

				var texture = Game.TextureManager.BuggyTexture;
				Transform.Origin = new Vector2(texture.Width, texture.Height) * 0.5f;
			}

			base.Draw(gt);
			
            Game.SpriteBatch.Draw(
				Game.TextureManager.BuggyTexture, 
				Transform.Position, 
				null, 
				Color.White, 
				Transform.Rotation, 
				Transform.Origin, 
				Transform.Scale, 
				Transform.Effect, 
				Transform.Depth
			);
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
