using DestroyNobots.Assembler.Emulator;

namespace DestroyNobots.Computers
{
    public class BasicComputer : Computer
    {
        public BasicComputer() 
            : base(new VCM86Processor(), new Assembler.SafeMemory(8, Assembler.BinaryMultiplier.KB))
        {
            GetSpecificProcessor<VCM86Processor>().StackPointer.Set(8 * 1024 - 512 - 1, 512);
        }
    }
}
