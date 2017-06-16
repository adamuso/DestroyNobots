using DestroyNobots.Assembler.Emulator;
using DestroyNobots.Assembler.Emulator.Peripherals;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Linq;

namespace DestroyNobots.Engine.Entities.Vehicles
{
    public class Buggy : Vehicle, IPeripheral
    {
        private float leftWheelsForce;
        private float rightWheelsForce;

        public override Rectangle BoundingRectangle { get { return new Rectangle(Transform.Position.ToPoint(), new Point(32, 32)); } }

        public Buggy()
        {
            Computer.ConnectPeripheral(this);
		}

        public override void Initialize()
        {
            base.Initialize();

            Transform.Origin = new Vector2(Game.TextureManager.BuggyTexture.Width, Game.TextureManager.BuggyTexture.Height) * 0.5f;

            Body = BodyFactory.CreateBody(Game.World, ConvertUnits.ToSimUnits(new Vector2(0, 0)));
            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 20000;
            Body.AngularDamping = 8f;
            Body.LinearDamping = 1f;

            PolygonShape s = new PolygonShape(new FarseerPhysics.Common.Vertices(((Polygon)new Rectangle(-Game.TextureManager.BuggyTexture.Width / 2, -Game.TextureManager.BuggyTexture.Height / 2, Game.TextureManager.BuggyTexture.Width, Game.TextureManager.BuggyTexture.Height)).Points.Select(p => ConvertUnits.ToSimUnits(p))), 1);
            Body.CreateFixture(s);
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

            Game.SpriteBatch.Draw(
                Game.BlankTexture, 
                new Rectangle(ConvertUnits.ToDisplayUnits(GetLeftTrackForcePoint()).ToPoint() - new Point(5, 5), new Point(10, 10)), 
                null, 
                Color.Red, 
                0f,
                Vector2.Zero, 
                Transform.Effect, 
                0f);

            Game.SpriteBatch.Draw(
                Game.BlankTexture, 
                new Rectangle(ConvertUnits.ToDisplayUnits(GetRightTrackForcePoint()).ToPoint() - new Point(5, 5), new Point(10, 10)),
                null,
                Color.Blue,
                0f,
                Vector2.Zero,
                Transform.Effect,
                0f);
        }

        public void Install()
        {
            Computer.Ports[0] = new PeripheralPortHandler(this)
            {
                Out = (value, size) =>
                {
                    leftWheelsForce = value;
                }
            };

            Computer.Ports[1] = new PeripheralPortHandler(this)
            {
                Out = (value, size) =>
                {
                    rightWheelsForce = value;
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

            Transform.Position = ConvertUnits.ToDisplayUnits(Body.Position);
            Transform.Rotation = Body.Rotation;

            Vector2 currentForwardNormal = Body.GetWorldVector(new Vector2(1, 0));

            Body.ApplyForce(leftWheelsForce * currentForwardNormal, GetLeftTrackForcePoint());
            Body.ApplyForce(rightWheelsForce * currentForwardNormal, GetRightTrackForcePoint());
        }

        private Vector2 GetLeftTrackForcePoint()
        {
            return Body.WorldCenter + Body.GetWorldVector(ConvertUnits.ToSimUnits(new Vector2(-Game.TextureManager.BuggyTexture.Width * 0.0f, -Game.TextureManager.BuggyTexture.Height * 0.5f)));
        }

        private Vector2 GetRightTrackForcePoint()
        {
            return Body.WorldCenter + Body.GetWorldVector(ConvertUnits.ToSimUnits(new Vector2(-Game.TextureManager.BuggyTexture.Width * 0.0f, Game.TextureManager.BuggyTexture.Height * 0.5f)));
        }
    }
}
