using DestroyNobots.Assembler;
using DestroyNobots.Assembler.Emulator;
using DestroyNobots.Assembler.Emulator.Registers;
using System;
using System.Collections.Generic;

namespace DestroyNobots.Computers
{
    public static class InstructionSets
    {
        public readonly static Dictionary<byte, AssemblerInstruction> VCM86 = new Dictionary<byte, AssemblerInstruction>()
        {
            #region Basic arithmetic operations
            { 0x1, new AssemblerInstruction("add", (instruction, context, parameters) => // add (reg, reg)
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();

                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 + reg2;
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x2, new AssemblerInstruction("add", (instruction, context, parameters) => // add (reg, val)
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();

                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 + parameters[1];
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x3, new AssemblerInstruction("mov", (instruction, context, parameters) => // mov (reg, reg)
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();

                    processor.Registers[parameters[0]].Value = processor.Registers[parameters[1]].Value;
                }
            )},

            { 0x4, new AssemblerInstruction("sub", (instruction, context, parameters) => // sub (reg, reg)
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();

                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 - reg2;
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x5, new AssemblerInstruction("sub", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                    processor.Registers[parameters[0]].Value = reg1 - parameters[1];
                }
            )},

            { 0x6, new AssemblerInstruction("mov", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Registers[parameters[0]].Value = parameters[1];
                }
            )},

            { 0x7, new AssemblerInstruction("mul", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 * reg2;
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x8, new AssemblerInstruction("mul", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 * parameters[1];
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x9, new AssemblerInstruction("div", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    if(reg2 == 0)
                    {
                        processor.Interrupt(0);
                        return;
                    }

                    processor.Registers[parameters[0]].Value = reg1 / reg2;
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0xA, new AssemblerInstruction("div", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 / parameters[1];
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x27, new AssemblerInstruction("mod", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 % parameters[1];
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x28, new AssemblerInstruction("mod", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 % processor.Registers[parameters[1]].Value;
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x29, new AssemblerInstruction("inc", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Registers[parameters[0]].Value++;
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x2A, new AssemblerInstruction("dec", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Registers[parameters[0]].Value--;
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},
            #endregion

            #region Bit operations

            { 0xB, new AssemblerInstruction("or", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 | reg2;
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0xC, new AssemblerInstruction("or", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 | parameters[1];
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0xE, new AssemblerInstruction("and", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 & reg2;
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0xF, new AssemblerInstruction("and", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 & parameters[1];
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},


            { 0x10, new AssemblerInstruction("xor", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 ^ reg2;
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x11, new AssemblerInstruction("xor", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 ^ parameters[1];
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x12, new AssemblerInstruction("not", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = ~reg1;
                }
            )},

            { 0x13, new AssemblerInstruction("shr", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 >> reg2;
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x14, new AssemblerInstruction("shl", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 << reg2;
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x15, new AssemblerInstruction("shr", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 >> parameters[1];
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x16, new AssemblerInstruction("shl", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 << parameters[1];
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                }
            )},
            #endregion

            #region Basic jumps
            { 0x17, new AssemblerInstruction("br", (instruction, context, parameters) => // br
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.ProgramCounter.Jump(parameters[0]);
                }
            )},

            { 0x18, new AssemblerInstruction("jmp", (instruction, context, parameters) => // jmp
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.ProgramCounter.Branch(parameters[0]);
                }, true
            )},
            #endregion

            #region Stack operations
            { 0x19, new AssemblerInstruction("push", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.StackPointer.Push(processor.Registers[parameters[0]].Value);
                }
            )},

            { 0x1A, new AssemblerInstruction("pop", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Registers[parameters[0]].Value = processor.StackPointer.Pop<int>();
                }
            )},
            #endregion

            #region Conditional jumps, conditions
            { 0x1B, new AssemblerInstruction("cmp", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.SetFlag(FlagType.Equal, processor.Registers[parameters[0]].Value == parameters[1]);
                    processor.SetFlag(FlagType.Zero, processor.Registers[parameters[0]].Value == 0);
                    processor.SetFlag(FlagType.Greater, processor.Registers[parameters[0]].Value > parameters[1]);
                }
            )},

            { 0x1C, new AssemblerInstruction("je", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if(processor.GetFlag(FlagType.Equal))
                        processor.Registers[31].Value += parameters[0];
                }, true
            )},

            { 0x1D, new AssemblerInstruction("jz", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (processor.GetFlag(FlagType.Zero))
                        processor.Registers[31].Value += parameters[0];
                }, true
            )},

            { 0x1E, new AssemblerInstruction("jne", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (!processor.GetFlag(FlagType.Equal))
                        processor.Registers[31].Value += parameters[0];
                }, true
            )},

            { 0x1F, new AssemblerInstruction("jnz", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (!processor.GetFlag(FlagType.Zero))
                        processor.Registers[31].Value += parameters[0];
                }, true
            )},

            { 0x20, new AssemblerInstruction("jgr", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (processor.GetFlag(FlagType.Greater))
                        processor.Registers[31].Value += parameters[0];
                }, true
            )},

            { 0x21, new AssemblerInstruction("jlo", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (!processor.GetFlag(FlagType.Greater) && !processor.GetFlag(FlagType.Equal))
                        processor.Registers[31].Value += parameters[0];
                }, true
            )},

            { 0x22, new AssemblerInstruction("jeg", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (processor.GetFlag(FlagType.Greater) || processor.GetFlag(FlagType.Equal))
                        processor.Registers[31].Value += parameters[0];
                }, true
            )},

            { 0x23, new AssemblerInstruction("jel", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (!processor.GetFlag(FlagType.Greater))
                        processor.Registers[31].Value += parameters[0];
                }, true
            )},
            #endregion

            #region Additional operations

            { 0xD, new AssemblerInstruction("lidt", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.InterruptDescriptorTablePointer.Address = processor.Registers[parameters[0]].Value;
                }
            )},

            { 0x24, new AssemblerInstruction("call", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.ProgramCounter.Call(processor.ProgramCounter.Address + parameters[0]);
                }
            )},

            { 0x25, new AssemblerInstruction("ret", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.ProgramCounter.Return();
                }
            )},

            { 0x26, new AssemblerInstruction("halt", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Abort();
                }
            )},

            { 0x2B, new AssemblerInstruction("int", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    //InterruptAction.Invoke(parameters[0]);
                }
            )},

            { 0x31, new AssemblerInstruction("int", (instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    //InterruptAction.Invoke(reg1);
                }
            )},
            #endregion

            #region Memory operations

            { 0x2C, new AssemblerInstruction<Register<int>, IPointer>("mov", (instruction, context, register, pointer) =>
                {
                    if (pointer.Size == 1)
                        register.Value = pointer.As<byte>().GetValue();
                    else if (pointer.Size == 2)
                        register.Value = pointer.As<short>().GetValue();
                    else if (pointer.Size == 4)
                        register.Value = pointer.As<int>().GetValue();
                    else
                        throw new Exception("Register too small");
                }
            )},


            { 0x2D, new AssemblerInstruction<IPointer, Register<int>>("mov", (instruction, context, pointer, register) =>
                {
                    if (pointer.Size == 1)
                        pointer.As<byte>().SetValue((byte)register.Value);
                    else if (pointer.Size == 2)
                        pointer.As<short>().SetValue((short)register.Value);
                    else if (pointer.Size == 4)
                        pointer.As<int>().SetValue(register.Value);
                    else if (pointer.Size == 8)
                        pointer.As<long>().SetValue(register.Value);
                }
            )},

            { 0x2E, new AssemblerInstruction<IPointer, ImmediateValue>("mov", (instruction, context, pointer, value) =>
                {
                    if(pointer.Size == 1)
                        pointer.As<byte>().SetValue(value.ToByte(null));
                    if(pointer.Size == 2)
                        pointer.As<short>().SetValue(value.ToInt16(null));
                    if(pointer.Size == 4)
                        pointer.As<int>().SetValue(value.ToInt32(null));
                    if(pointer.Size == 8)
                        pointer.As<long>().SetValue(value.ToInt64(null));
               }
            )},
            #endregion

            #region I/O Operations
            { 0x32, new AssemblerInstruction("out", (instruction, context, parameters) =>
                {
                    var computer = context.GetContext<Computer>();

                    if(computer.Ports.ContainsKey((ushort)parameters[0]))
                    {
                        computer.Ports[(ushort)parameters[0]].Out(parameters[1], 4);
                    }
                }
            )},

            { 0x33, new AssemblerInstruction<ImmediateValue, Register<int>>("out", (instruction, context, value, register) =>
                {
                    var computer = context.GetContext<Computer>();

                    if(computer.Ports.ContainsKey(value.ToUInt16(null)))
                    {
                        computer.Ports[value.ToUInt16(null)].Out(register.Value, register.Size);
                    }
                }
            )},

            { 0x34, new AssemblerInstruction("in", (instruction, context, parameters) =>
                {
                    var computer = context.GetContext<Computer>();
                    var processor = computer.GetSpecificProcessor<VCM86Processor>();
                    var register = processor.Registers[parameters[1]];

                    if(computer.Ports.ContainsKey((ushort)parameters[0]))
                    {
                        register.Value = (int)computer.Ports[(ushort)parameters[0]].In(register.Size);
                    }
                }
            )},
            #endregion
        };
    }
}
