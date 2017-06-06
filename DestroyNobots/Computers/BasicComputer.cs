using DestroyNobots.Assembler.Emulator;

namespace DestroyNobots.Computers
{
    public class BasicComputer : Computer
    {
        private BasicComputer(IProcessorBase processor, Assembler.IMemory memory) 
            : base(new VCM86Processor(), new Assembler.SafeMemory(8, Assembler.BinaryMultiplier.KB))
        {

        }
    }
}
