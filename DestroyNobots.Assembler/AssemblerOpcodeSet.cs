using System.Collections.Generic;
using System.Linq;

namespace DestroyNobots.Assembler
{
    public class AssemblerOpcodeSet
    {
        IInstructionSetProvider instructionSetProvider;
        HashSet<byte> opcodes;
        byte @default;

        public AssemblerOpcodeSet(IInstructionSetProvider instructionSetProvider)
        {
            this.instructionSetProvider = instructionSetProvider;
            this.opcodes = new HashSet<byte>();
        }

        public bool Add(byte opcode)
        {
            return opcodes.Add(opcode);
        }

        public byte Find(params AssemblerParameters[] parameters)
        {
            foreach (byte opcode in opcodes)
            {
                AssemblerInstruction ins = instructionSetProvider.InstructionSet[opcode];

                if (ins.ParametersCount != parameters.Length)
                    continue;

                bool found = true;

                for (int j = 0; j < parameters.Length; j++)
                {
                    // if parameter does not have pointer flag
                    if ((parameters[j] & AssemblerParameters.Pointer) == 0)
                    {
                        if (parameters[j] != ins.Parameters[j])
                        {
                            found = false;
                            break;
                        }
                    }
                    else
                    {
                        if ((parameters[j] & AssemblerParameters.Value) != 0 && (ins.Parameters[j] & AssemblerParameters.Value) != 0)
                            continue;

                        if (AssemblerParameters.Pointer == ins.Parameters[j] && (parameters[j] & AssemblerParameters.Value) == 0)
                            continue;

                        found = false;
                        break;
                    }
                }

                if (!found)
                    continue;
                else
                    return opcode;
            }

            return 255;
        }

        public int Count { get { return opcodes.Count; } }
        public IList<byte> Opcodes { get { return opcodes.ToList(); } }
        public byte Default { get { return @default; } set { @default = value; } }
    }
}
