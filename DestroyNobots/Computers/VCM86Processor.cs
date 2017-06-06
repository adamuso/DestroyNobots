using System;
using DestroyNobots.Assembler.Emulator;

namespace DestroyNobots.Computers
{
    public class VCM86Processor : Processor<int>
    {
        public override byte ProgramCountRegisterNumber { get { return 9; } }
        public override byte RegistersCount { get { return 10; } }
        public override byte StackPointerRegisterNumber { get { return 8; } }

        public VCM86Processor() 
            : base(InstructionSets.VCM86)
        {

        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
