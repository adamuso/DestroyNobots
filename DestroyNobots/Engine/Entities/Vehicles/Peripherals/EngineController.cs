using System;
using DestroyNobots.Assembler.Emulator.Peripherals;
using Microsoft.Xna.Framework;
using DestroyNobots.Assembler.Emulator;

namespace DestroyNobots.Engine.Entities.Vehicles.Peripherals
{
    public class EngineController : IPeripheral, IUpdateable
    {
        private Vector2 oldPosition;
        private Vehicle vehicle;
        bool wheelsSideFlag;
        bool wheelsMode;

        public float DrivenDistance { get; private set; }
        public float LeftWheelsForce { get; private set; }
        public float RightWheelsForce { get; private set; }

        public EngineController(Vehicle vehicle)
        {
            this.vehicle = vehicle;
            wheelsSideFlag = false;
            wheelsMode = false;
            oldPosition = Vector2.Zero;
            LeftWheelsForce = 0;
            RightWheelsForce = 0;
            DrivenDistance = 0;
        }

        public void Install()
        {
            vehicle.Computer.Ports[0] = new PeripheralPortHandler(this)
            {
                Out = (value, size) =>
                {
                    if (!wheelsMode)
                    {
                        if ((value & 0x80) != 0)
                        {
                            wheelsMode = true;
                            value = value & (~0x80);
                        }

                        if (value == 0)
                            wheelsSideFlag = false;
                        else if(value == 1)
                            wheelsSideFlag = true;
                    }
                    else
                    {
                        if(!wheelsSideFlag)
                            LeftWheelsForce = value;
                        else
                            RightWheelsForce = value;

                        wheelsMode = false;
                    }
                },
                In = (size) =>
                {


                    return 0;
                }
            };

            vehicle.Computer.Ports[1] = new PeripheralPortHandler(this)
            {
                Out = (value, size) =>
                {
                }
            };
        }

        public void Uninstall()
        {
            vehicle.Computer.Ports.Remove(0);
            vehicle.Computer.Ports.Remove(1);
        }

        public void Update(GameTime gt)
        {
            if (oldPosition == Vector2.Zero)
                oldPosition = vehicle.Body.WorldCenter;

            DrivenDistance += (vehicle.Body.WorldCenter - oldPosition).Length();

            oldPosition = vehicle.Body.WorldCenter;
        }
    }
}
