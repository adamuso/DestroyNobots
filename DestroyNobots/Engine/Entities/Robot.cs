using DestroyNobots.Assembler.Emulator;
using DestroyNobots.Computers;

namespace DestroyNobots.Engine.Entities
{
    public class Robot : Collider
    {
        public Computer Computer { get; private set; }

        public Robot()
        {
            Computer = new BasicComputer();
        }
    }
}
