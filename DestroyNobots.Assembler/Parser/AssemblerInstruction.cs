using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DestroyNobots.Assembler.Parser
{
    public delegate void InstructionAction(AssemblerInstruction instruction,  params int[] _params);

    public class AssemblerInstruction
    {
        AssemblerCompiler compiler;
        InstructionAction action;
        AssemblerParameters[] parameters;
        bool labelOffset;

        public AssemblerCompiler Compiler { get { return compiler; } }
        public int ParametersCount { get { return parameters?.Length ?? 0; } }
        public AssemblerParameters[] Parameters { get { return parameters; } }
        public bool LabelOffset { get { return labelOffset; } }

        public AssemblerInstruction(AssemblerCompiler compiler, InstructionAction action, bool labelOffset = true)
        {
            this.compiler = compiler;
            this.action = action;
            this.labelOffset = labelOffset;
            parameters = null;
        }

        public AssemblerInstruction SetParameters(params AssemblerParameters[] parameters)
        {
            this.parameters = parameters;
            return this;
        }

        public void Eval(params int[] @params)
        {
            if (@params.Length == ParametersCount)
                action.DynamicInvoke(this, @params);
            else
                throw new ArgumentOutOfRangeException("This method is only applicable for " + ParametersCount + " arguments!");
        }

    }
}
