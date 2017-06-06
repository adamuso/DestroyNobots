using System.Collections.Generic;

namespace DestroyNobots.Assembler
{
    public class AssemblerOpcodeSet
    {
        IInstructionSetProvider instructionSetProvider;
        List<byte> opcodes;
        byte _default;

        public AssemblerOpcodeSet(IInstructionSetProvider instructionSetProvider)
        {
            this.instructionSetProvider = instructionSetProvider;
            this.opcodes = new List<byte>();
        }

        public void Add(byte opcode)
        {
            opcodes.Add(opcode);
        }

        public byte Find(params Parser.AssemblerParameters[] parameters)
        {
            for (int i = 0; i < opcodes.Count; i++)
            {
                AssemblerInstruction ins = instructionSetProvider.InstructionSet[opcodes[i]];

                if (ins.ParametersCount != parameters.Length)
                    continue;

                bool found = true;

                for (int j = 0; j < parameters.Length; j++)
                {
                    // if parameter does not have pointer flag
                    if ((parameters[j] & Parser.AssemblerParameters.POINTER) == 0)
                    {
                        if (parameters[j] != ins.Parameters[j])
                        {
                            found = false;
                            break;
                        }
                    }
                    else
                    {
                        if (Parser.AssemblerParameters.POINTER != ins.Parameters[j])
                        {
                            found = false;
                            break;
                        }
                    }
                }

                if (!found)
                    continue;
                else
                    return opcodes[i];
            }

            return 255;
        }

        public int Count { get { return opcodes.Count; } }
        public List<byte> Opcodes { get { return opcodes; } }
        public byte Default { get { return _default; } set { _default = value; } }
    }
}
