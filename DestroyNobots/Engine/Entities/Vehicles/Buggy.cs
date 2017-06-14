using DestroyNobots.Assembler.Emulator;
using DestroyNobots.Assembler.Emulator.Peripherals;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DestroyNobots.Engine.Entities.Vehicles
{
    public class Buggy : Vehicle, IPeripheral
    {
        private float leftWheelsForce;
        private float rightWheelsForce;

        public override Rectangle BoundingRectangle { get { return new Rectangle(Transform.Position.ToPoint(), new Point(32, 32)); } }

        public Buggy()
        {
            Transform.Position = new Vector2(400, 400);
            Computer.ConnectPeripheral(this);
            Front = new Vector2(1, 0);

            Physics.Friction = 0.99f;
		}

        public override void Initialize()
        {
            base.Initialize();

            Transform.Origin = new Vector2(Game.TextureManager.BuggyTexture.Width, Game.TextureManager.BuggyTexture.Height) * 0.5f;
            Physics.CenterOfMass = new Vector2(Game.TextureManager.BuggyTexture.Width, Game.TextureManager.BuggyTexture.Height) * 0.5f;
        }

        public override void Draw(GameTime gt)
        {
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
                    leftWheelsForce = value / 100.0f;
                    rightWheelsForce = value / 200.0f;
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

            Physics.AddForce(new Vector2(Game.TextureManager.BuggyTexture.Width * 0.5f, Game.TextureManager.BuggyTexture.Height), leftWheelsForce * Forward);
            //Physics.AddForce(new Vector2(1, 0), rightWheelsForce * Forward);

        }
    }
}
