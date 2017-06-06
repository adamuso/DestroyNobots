using System.Collections.Generic;

namespace DestroyNobots.Assembler
{
    public interface IInstructionSetProvider
    {
        Dictionary<byte, AssemblerInstruction> InstructionSet { get; }
    }
}