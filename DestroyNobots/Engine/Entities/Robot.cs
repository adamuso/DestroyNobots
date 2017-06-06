using DestroyNobots.Assembler.Emulator;
using DestroyNobots.Computers;

namespace DestroyNobots.Engine.Entities
{
    public class Robot : Entity
    {
        public Computer Computer { get; private set; }

        public Robot()
        {
            Computer = new BasicComputer();
        }
    }
}
