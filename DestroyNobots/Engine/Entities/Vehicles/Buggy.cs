using System;
using DestroyNobots.Assembler.Emulator;
using DestroyNobots.Assembler.Emulator.Peripherals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DestroyNobots.Engine.Entities.Vehicles
{
    public class Buggy : Vehicle, IPeripheral
    {
        public override Rectangle BoundingRectangle { get { return new Rectangle(200, 200, 32, 32); } }

        public Buggy()
        {
            Computer.ConnectPeripheral(this);
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            Game.SpriteBatch.Draw(Game.TextureManager.BuggyTexture, new Vector2(200, 200), null, Color.White, rotation / 360.0f, new Vector2(16, 16), 1, SpriteEffects.None, 0.0f);
        }

        public void Install()
        {
            Computer.Ports[0] = new PeripheralPortHandler(this)
            {
                Out = (value, size) =>
                {
                    rotation = value;
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
