using DestroyNobots.Assembler.Emulator.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DestroyNobots.Assembler
{
    public delegate void InstructionAction(AssemblerInstruction instruction, IRuntimeContext context,  params int[] _params);
    public delegate void InstructionAction<T>(AssemblerInstruction instruction, IRuntimeContext context, T param) 
        where T : IAssemblerInstructionParameter;
    public delegate void InstructionAction<T1, T2>(AssemblerInstruction instruction, IRuntimeContext context, T1 param1, T2 param2) 
        where T1 : IAssemblerInstructionParameter where T2 : IAssemblerInstructionParameter;
    public delegate void InstructionAction<T1, T2, T3>(AssemblerInstruction instruction, IRuntimeContext context, T1 param1, T2 param2, T3 param3)
        where T1 : IAssemblerInstructionParameter where T2 : IAssemblerInstructionParameter where T3 : IAssemblerInstructionParameter;
    public delegate void InstructionAction<T1, T2, T3, T4>(AssemblerInstruction instruction, IRuntimeContext context, T1 param1, T2 param2, T3 param3, T4 param4)
        where T1 : IAssemblerInstructionParameter where T2 : IAssemblerInstructionParameter where T3 : IAssemblerInstructionParameter where T4 : IAssemblerInstructionParameter;
    public delegate void InstructionAction<T1, T2, T3, T4, T5>(AssemblerInstruction instruction, IRuntimeContext context, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        where T1 : IAssemblerInstructionParameter where T2 : IAssemblerInstructionParameter where T3 : IAssemblerInstructionParameter where T4 : IAssemblerInstructionParameter where T5 : IAssemblerInstructionParameter;

    public class AssemblerInstruction
    {
        string name;
        InstructionAction action;
        AssemblerParameters[] parameters;

        public int ParametersCount { get { return parameters?.Length ?? 0; } }
        public AssemblerParameters[] Parameters { get { return parameters; } }
        public bool ConvertLabelsToOffsets { get; private set; }

        public AssemblerInstruction(string name, InstructionAction action, bool convertLabelsToOffset = false)
        {
            this.name = name;
            this.action = action;
            ConvertLabelsToOffsets = convertLabelsToOffset;
            parameters = null;
        }

        public AssemblerInstruction SetParameters(params AssemblerParameters[] parameters)
        {
            this.parameters = parameters;
            return this;
        }

        public virtual void Eval(IRuntimeContext context, params int[] @params)
        {
            if (@params.Length == ParametersCount)
                action(this, context, @params);
            else
                throw new ArgumentOutOfRangeException("This method is only applicable for " + ParametersCount + " arguments!");
        }

        public static AssemblerParameters[] GetParametersFromTypes(params Type[] types)
        {
            AssemblerParameters[] parameters = new AssemblerParameters[types.Length];

            for(int i = 0; i < types.Length; i++)
            {
                if (typeof(IRegister).IsAssignableFrom(types[i]))
                    parameters[i] = AssemblerParameters.Register;
                else if (typeof(Address) == types[i])
                    parameters[i] = AssemblerParameters.Address;
                else if (typeof(AssemblerParameterValue) == types[i])
                    parameters[i] = AssemblerParameters.Value;
                else if (typeof(IPointer).IsAssignableFrom(types[i]))
                    parameters[i] = AssemblerParameters.Pointer;
            }

            return parameters;
        }

        public static T GetValueFromParameter<T>(IRuntimeContext context, int parameter) where T : IAssemblerInstructionParameter
        {
            if (typeof(IRegister).IsAssignableFrom(typeof(T)))
                return (T)context.Processor.Registers[parameter];
            else if (typeof(T) == typeof(Address))
                return (T)(IAssemblerInstructionParameter)new Address((uint)parameter);
            else if (typeof(AssemblerParameterValue) == typeof(T))
                return (T)(IAssemblerInstructionParameter)new AssemblerParameterValue() { Value = parameter };
            else if (typeof(IPointer).IsAssignableFrom(typeof(T)))
            {
                if (typeof(IPointer) == typeof(T))
                {
                    //return (T)(IPointer)new Pointer(context.Memory, parameter);
                }
                else if (typeof(T).IsGenericType)
                {
                    Type pointerGenericArgumentType = typeof(T).GetGenericArguments()[0];
                    Type pointerType = typeof(Pointer<>);
                    Type pointerGenericType = pointerType.MakeGenericType(pointerGenericArgumentType);

                    Expression expr = Expression.New(
                        pointerGenericType.GetConstructor(new Type[] { typeof(IRuntimeContext), typeof(Address) }), 
                        Expression.Constant(context.Memory), 
                        Expression.Constant(parameter, typeof(Address)));

                    return Expression.Lambda<Func<T>>(expr).Compile()();
                }
            }

            throw new Exception("Invalid parameter type");
        }
    }

    public class AssemblerInstruction<T> : AssemblerInstruction where T : IAssemblerInstructionParameter
    {
        InstructionAction<T> action;

        public AssemblerInstruction(string name, InstructionAction<T> action, bool convertLabelsToOffset = false) 
            : base(name, null, convertLabelsToOffset)
        {
            this.action = action;

            SetParameters(GetParametersFromTypes(typeof(T)));
        }

        public override void Eval(IRuntimeContext context, params int[] @params)
        {
            if (@params.Length == ParametersCount)
            {
                T param = GetValueFromParameter<T>(context, @params[0]);
                action(this, context, param);
            }
            else
                throw new ArgumentOutOfRangeException("This method is only applicable for " + ParametersCount + " arguments!");

        }
    }
}
