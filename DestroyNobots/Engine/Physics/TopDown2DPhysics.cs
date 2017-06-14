using DestroyNobots.Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DestroyNobots.Engine.Physics
{
    public class TopDown2DPhysics : IPhysics
    {
        private struct ForceVector2
        {
            public Vector2 PointOfApplication { get; set; }
            public Vector2 Force { get; set; }
        }

        private List<ForceVector2> forces;
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Friction { get; set; }
        public Vector2 AngularAcceleration { get; set; }
        public Vector2 AngularVelocity { get; set; } 
        private Vector2 Orientation { get; set; }
        public float RotationFriction { get; set; }
        public Vector2 CenterOfMass { get; set; }
        public float Mass { get; set; }

        public bool IsDirty { get; private set; }

        public TopDown2DPhysics()
        {
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            Friction = 0;
            AngularAcceleration = Vector2.Zero;
            AngularVelocity = Vector2.Zero;
            RotationFriction = 0;
            Mass = 1;
            forces = new List<ForceVector2>();
        }

        public void AddForce(Vector2 pointOfApplication, Vector2 force)
        {
            forces.Add(new ForceVector2() { PointOfApplication = pointOfApplication, Force = force });
        }

        public void Apply(Collider collider)
        {
            Acceleration = Vector2.Zero;
            AngularAcceleration = Vector2.Zero;

            if(forces.Count > 0)
            {
                for (int i = 0; i < forces.Count; i++)
                {
                    Acceleration += forces[i].Force;

                    Vector2 momentArm = Vector2.Transform(forces[i].PointOfApplication - CenterOfMass, Matrix.CreateRotationZ(collider.Transform.Rotation));
                    Vector2 parallelComponent = forces[i].Force.ProjectInto(momentArm);
                    Vector2 angularForce = forces[i].Force - parallelComponent;
                    Vector2 torque = angularForce * forces[i].PointOfApplication.Length();

                    if(!float.IsNaN(torque.X) && !float.IsNaN(torque.Y))
                        AngularAcceleration = torque / Mass;
                }

                Acceleration /= Mass;
            }

            Velocity += Acceleration;
            collider.Transform.Position += Velocity;

            Velocity *= Friction;

            AngularVelocity += AngularAcceleration;
            Orientation += AngularVelocity;

            collider.Transform.Rotation = (float)Math.Atan2(Orientation.Y, Orientation.X);

            forces.Clear();
        }

        public void Update(GameTime gt)
        {
            if (Math.Abs(Velocity.X) < 0.00001f)
                Vector2.Multiply(Velocity, new Vector2(0, 1));

            if (Math.Abs(Velocity.Y) < 0.00001f)
                Vector2.Multiply(Velocity, new Vector2(1, 0));

            if (Math.Abs(AngularVelocity.X) < 0.00001f)
                Vector2.Multiply(AngularAcceleration, new Vector2(0, 1));

            if (Math.Abs(AngularVelocity.Y) < 0.00001f)
                Vector2.Multiply(AngularAcceleration, new Vector2(1, 0));


            IsDirty = Velocity.X != 0 || Velocity.Y != 0 || AngularVelocity.X != 0|| AngularVelocity.Y != 0 || forces.Count > 0;
        }
    }
}
