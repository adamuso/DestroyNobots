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

            Body = BodyFactory.CreateBody(Game.World, ConvertUnits.ToSimUnits(new Vector2(400, 400)));
            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 20000;
            Body.AngularDamping = 0.1f;
            Body.LinearDamping = 0.0f;

            PolygonShape s = new PolygonShape(new FarseerPhysics.Common.Vertices(((Polygon)new Rectangle(0, 0, Game.TextureManager.BuggyTexture.Width, Game.TextureManager.BuggyTexture.Height)).Points.Select(p => ConvertUnits.ToSimUnits(p))), 1);
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
		}

        public void Install()
        {
            Computer.Ports[0] = new PeripheralPortHandler(this)
            {
                Out = (value, size) =>
                {
                    leftWheelsForce = value;
                    rightWheelsForce = value * 0.0f;
                }
            };
        }

        public void Uninstall()
        {
            Computer.Ports.Remove(0);
        }

        private Vector2 GetLateralVelocity()
        {
            Vector2 currentRightNormal = Body.GetWorldVector(new Vector2(0, 1));
            return Vector2.Dot(currentRightNormal, Body.LinearVelocity) * currentRightNormal;

        }

        private void UpdateFriction()
        {
            Vector2 impulse = Body.Mass * -GetLateralVelocity();
            Body.ApplyLinearImpulse(impulse, Body.WorldCenter);
            Body.ApplyAngularImpulse(0.005f * Body.Inertia * -Body.AngularVelocity);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            Transform.Position = ConvertUnits.ToDisplayUnits(Body.Position);
            Transform.Rotation = Body.Rotation;

            Vector2 currentForwardNormal = Body.GetWorldVector(new Vector2(1, 0));

            //if (currentForwardNormal.LengthSquared() > 0)
            //{
            //    currentForwardNormal.Normalize();

            //    float currentForwardSpeed = currentForwardNormal.Length();
            //    float dragForceMagnitude = -1 * currentForwardSpeed;
            //    Body.ApplyForce(dragForceMagnitude * currentForwardNormal, Body.WorldCenter);
            //}

            Body.ApplyForce(leftWheelsForce * currentForwardNormal, Body.WorldCenter + ConvertUnits.ToSimUnits(new Vector2(0, Game.TextureManager.BuggyTexture.Height * 0.5f)));
            //Body.ApplyForce(-leftWheelsForce * currentForwardNormal, Body.WorldCenter + ConvertUnits.ToSimUnits(new Vector2(0, -Game.TextureManager.BuggyTexture.Height * 0.5f)));

            //UpdateFriction();
        }
    }
}
