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
        Parser.AssemblerParameters[] parameters;
        bool pointerOffset;

        public int ParametersCount { get { return parameters?.Length ?? 0; } }
        public Parser.AssemblerParameters[] Parameters { get { return parameters; } }
        public bool PointerOffset { get { return pointerOffset; } }

        public AssemblerInstruction(InstructionAction action, bool pointerOffset = true)
        {
            this.action = action;
            this.pointerOffset = pointerOffset;
            parameters = null;
        }

        public AssemblerInstruction SetParameters(params Parser.AssemblerParameters[] parameters)
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
