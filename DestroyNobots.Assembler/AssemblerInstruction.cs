using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DestroyNobots.Assembler
{
    public delegate void InstructionAction(AssemblerInstruction instruction, IRuntimeContext context,  params int[] _params);

    public class AssemblerInstruction
    {
        InstructionAction action;
        AssemblerParameters[] parameters;

        public int ParametersCount { get { return parameters?.Length ?? 0; } }
        public AssemblerParameters[] Parameters { get { return parameters; } }
        public bool ConvertLabelsToOffsets { get; private set; }

        public AssemblerInstruction(InstructionAction action, bool convertLabelsToOffset = false)
        {
            this.action = action;
            ConvertLabelsToOffsets = convertLabelsToOffset;
            parameters = null;
        }

        public AssemblerInstruction SetParameters(params AssemblerParameters[] parameters)
        {
            this.parameters = parameters;
            return this;
        }

        public void Eval(IRuntimeContext context, params int[] @params)
        {
            if (@params.Length == ParametersCount)
                action(this, context, @params);
            else
                throw new ArgumentOutOfRangeException("This method is only applicable for " + ParametersCount + " arguments!");
        }

    }
}
