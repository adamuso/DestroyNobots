using DestroyNobots.Assembler.Emulator.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DestroyNobots.Assembler
{
    public delegate void InstructionAction(AssemblerInstruction instruction, IRuntimeContext context,  params AssemblerParameterValue[] _params);
    public delegate void InstructionAction<T>(AssemblerInstruction instruction, IRuntimeContext context, T param) 
        where T : IAssemblerInstructionParameter;
    public delegate void InstructionAction<T1, T2>(AssemblerInstruction instruction, IRuntimeContext context, T1 param1, T2 param2) 
        where T1 : IAssemblerInstructionParameter where T2 : IAssemblerInstructionParameter;
    public delegate void InstructionAction<T1, T2, T3>(AssemblerInstruction instruction, IRuntimeContext context, T1 param1, T2 param2, T3 param3)
        where T1 : IAssemblerInstructionParameter where T2 : IAssemblerInstructionParameter where T3 : IAssemblerInstructionParameter;

    [System.Diagnostics.DebuggerDisplay("Instruction: {Name}, Parameters: {string.Join(\", \", parameters)}")]
    public class AssemblerInstruction
    {
        InstructionAction action;
        AssemblerParameters[] parameters;

        public int ParametersCount { get { return parameters?.Length ?? 0; } }
        public AssemblerParameters[] Parameters { get { return parameters; } }
        public string Name { get; private set; }
        public bool ConvertLabelsToOffsets { get; private set; }

        public AssemblerInstruction(string name, InstructionAction action, bool convertLabelsToOffset = false)
        {
            this.Name = name;
            this.action = action;
            ConvertLabelsToOffsets = convertLabelsToOffset;
            parameters = null;
        }

        public AssemblerInstruction SetParameters(params AssemblerParameters[] parameters)
        {
            if (this.parameters != null)
                return this;

            this.parameters = parameters;
            return this;
        }

        public virtual void Eval(IRuntimeContext context, params AssemblerParameterValue[] @params)
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
                else if (typeof(ImmediateValue) == types[i])
                    parameters[i] = AssemblerParameters.Value;
                else if (typeof(IPointer).IsAssignableFrom(types[i]))
                    parameters[i] = AssemblerParameters.Pointer;
            }

            return parameters;
        }

        public static T GetValueFromParameter<T>(IRuntimeContext context, AssemblerParameterValue parameter) where T : IAssemblerInstructionParameter
        {
            if (typeof(IRegister).IsAssignableFrom(typeof(T)))
                return (T)context.Processor.Registers[parameter];
            else if (typeof(T) == typeof(Address))
                return (T)(IAssemblerInstructionParameter)(Address)parameter;
            else if (typeof(ImmediateValue) == typeof(T))
                return (T)(IAssemblerInstructionParameter)new ImmediateValue() { Value = parameter.Value };
            else if (typeof(IPointer).IsAssignableFrom(typeof(T)))
            {
                if (typeof(IPointer) == typeof(T))
                {
                    if(parameter.PointerSize == PointerType.Bit8)
                        return (T)(IPointer)new Pointer<byte>(context.Memory, parameter);
                    else if (parameter.PointerSize == PointerType.Bit16)
                        return (T)(IPointer)new Pointer<short>(context.Memory, parameter);
                    else if (parameter.PointerSize == PointerType.Bit32)
                        return (T)(IPointer)new Pointer<int>(context.Memory, parameter);
                    else if (parameter.PointerSize == PointerType.Bit64)
                        return (T)(IPointer)new Pointer<long>(context.Memory, parameter);
                }
                else if (typeof(T).IsGenericType)
                {
                    Type pointerGenericArgumentType = typeof(T).GetGenericArguments()[0];
                    Type pointerType = typeof(Pointer<>);
                    Type pointerGenericType = pointerType.MakeGenericType(pointerGenericArgumentType);

                    Expression expr = Expression.New(
                        pointerGenericType.GetConstructor(new Type[] { typeof(IRuntimeContext), typeof(Address) }), 
                        Expression.Constant(context.Memory), 
                        Expression.Constant((Address)parameter));

                    return Expression.Lambda<Func<T>>(expr).Compile()();
                }
            }

            throw new Exception("Invalid parameter type");
        }
    }

    /*public class AssemblerInstruction<T> : AssemblerInstruction
        where T : IAssemblerInstructionParameter
    {
        InstructionAction<T> action;

        public AssemblerInstruction(string name, InstructionAction<T> action, bool convertLabelsToOffset = false) 
            : base(name, null, convertLabelsToOffset)
        {
            this.action = action;

            SetParameters(GetParametersFromTypes(typeof(T)));
        }

        public override void Eval(IRuntimeContext context, params AssemblerParameterValue[] @params)
        {
            if (@params.Length == 1)
            {
                T param = GetValueFromParameter<T>(context, @params[0]);
                action(this, context, param);
            }
            else
                throw new ArgumentOutOfRangeException("This method is only applicable for 1 arguments!");
        }
    }

    public class AssemblerInstruction<T1, T2> : AssemblerInstruction 
        where T1 : IAssemblerInstructionParameter where T2 : IAssemblerInstructionParameter
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
                T2 param2 = GetValueFromParameter<T2>(context, @params[0]);

                action(this, context, param1, param2);
            }
            else
                throw new ArgumentOutOfRangeException("This method is only applicable for 2 arguments!");
        }
    }

    public class AssemblerInstruction<T1, T2, T3> : AssemblerInstruction
        where T1 : IAssemblerInstructionParameter where T2 : IAssemblerInstructionParameter where T3 : IAssemblerInstructionParameter
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
                T2 param2 = GetValueFromParameter<T2>(context, @params[0]);
                T3 param3 = GetValueFromParameter<T3>(context, @params[0]);

                action(this, context, param1, param2, param3);
            }
            else
                throw new ArgumentOutOfRangeException("This method is only applicable for 3 arguments!");
        }
    }*/
}
