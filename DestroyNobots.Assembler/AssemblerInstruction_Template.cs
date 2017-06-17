using System;

namespace DestroyNobots.Assembler
{
	public class AssemblerInstruction<T1> : AssemblerInstruction
		 where T1 : IAssemblerInstructionParameter 
	{
		InstructionAction<T1> action;

		public AssemblerInstruction(string name, InstructionAction<T1> action, bool convertLabelsToOffset = false) 
			: base(name, null, convertLabelsToOffset)
		{
			this.action = action;

			SetParameters(GetParametersFromTypes(typeof(T1)));
		}

		public override void Eval(IRuntimeContext context, params AssemblerParameterValue[] @params)
		{
			if (@params.Length == 1)
			{ 
				T1 param1 = GetValueFromParameter<T1>(context, @params[0]); 

				action(this, context, param1);
			}
				
			else
				throw new ArgumentOutOfRangeException("This method is only applicable for 1 arguments!");
		}
	}
	public class AssemblerInstruction<T1, T2> : AssemblerInstruction
		 where T1 : IAssemblerInstructionParameter  where T2 : IAssemblerInstructionParameter 
	{
		InstructionAction<T1, T2> action;

		public AssemblerInstruction(string name, InstructionAction<T1, T2> action, bool convertLabelsToOffset = false) 
			: base(name, null, convertLabelsToOffset)
		{
			this.action = action;

			SetParameters(GetParametersFromTypes(typeof(T1), typeof(T2)));
		}

		public override void Eval(IRuntimeContext context, params AssemblerParameterValue[] @params)
		{
			if (@params.Length == 2)
			{ 
				T1 param1 = GetValueFromParameter<T1>(context, @params[0]); 
				T2 param2 = GetValueFromParameter<T2>(context, @params[1]); 

				action(this, context, param1, param2);
			}
				
			else
				throw new ArgumentOutOfRangeException("This method is only applicable for 2 arguments!");
		}
	}
	public class AssemblerInstruction<T1, T2, T3> : AssemblerInstruction
		 where T1 : IAssemblerInstructionParameter  where T2 : IAssemblerInstructionParameter  where T3 : IAssemblerInstructionParameter 
	{
		InstructionAction<T1, T2, T3> action;

		public AssemblerInstruction(string name, InstructionAction<T1, T2, T3> action, bool convertLabelsToOffset = false) 
			: base(name, null, convertLabelsToOffset)
		{
			this.action = action;

			SetParameters(GetParametersFromTypes(typeof(T1), typeof(T2), typeof(T3)));
		}

		public override void Eval(IRuntimeContext context, params AssemblerParameterValue[] @params)
		{
			if (@params.Length == 3)
			{ 
				T1 param1 = GetValueFromParameter<T1>(context, @params[0]); 
				T2 param2 = GetValueFromParameter<T2>(context, @params[1]); 
				T3 param3 = GetValueFromParameter<T3>(context, @params[2]); 

				action(this, context, param1, param2, param3);
			}
				
			else
				throw new ArgumentOutOfRangeException("This method is only applicable for 3 arguments!");
		}
	}
	 
}