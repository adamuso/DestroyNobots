using DestroyNobots.Assembler.Emulator;

namespace DestroyNobots.Computers
{
    public class BasicComputer : Computer
    {
        public BasicComputer() 
            : base(new VCM86Processor(), new Assembler.SafeMemory(8, Assembler.BinaryMultiplier.KB))
        {

        }
    }
}
