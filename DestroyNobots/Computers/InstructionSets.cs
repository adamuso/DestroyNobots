using DestroyNobots.Assembler;
using System.Collections.Generic;

namespace DestroyNobots.Computers
{
    public static class InstructionSets
    {
        public readonly static Dictionary<byte, AssemblerInstruction> VCM86 = new Dictionary<byte, AssemblerInstruction>()
        {
            #region Basic arithmetic operations
            { 0x1, new AssemblerInstruction((instruction, context, parameters) => // add (reg, reg)
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();

                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 + reg2;
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x2, new AssemblerInstruction((instruction, context, parameters) => // add (reg, val)
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();

                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 + parameters[1];
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x3, new AssemblerInstruction((instruction, context, parameters) => // mov (reg, reg)
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();

                    processor.Registers[parameters[0]].Value = processor.Registers[parameters[1]].Value;
                }
            )},

            { 0x4, new AssemblerInstruction((instruction, context, parameters) => // sub (reg, reg)
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();

                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 - reg2;
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x5, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                    processor.Registers[parameters[0]].Value = reg1 - parameters[1];
                }
            )},

            { 0x6, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Registers[parameters[0]].Value = parameters[1];
                }
            )},

            { 0x7, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1* reg2;
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x8, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1* parameters[1];
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x9, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 / reg2;
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0xA, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 / parameters[1];
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x27, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 % parameters[1];
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x28, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 % processor.Registers[parameters[1]].Value;
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x29, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Registers[parameters[0]].Value++;
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x2A, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Registers[parameters[0]].Value--;
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},
            #endregion

            #region Bit operations

            { 0xB, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 | reg2;
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0xC, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 | parameters[1];
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0xE, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 & reg2;
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0xF, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 & parameters[1];
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},


            { 0x10, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 ^ reg2;
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x11, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 ^ parameters[1];
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x12, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = ~reg1;
                }
            )},

            { 0x13, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 >> reg2;
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x14, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;
                    int reg2 = processor.Registers[parameters[1]].Value;

                    processor.Registers[parameters[0]].Value = reg1 << reg2;
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x15, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 >> parameters[1];
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},

            { 0x16, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    processor.Registers[parameters[0]].Value = reg1 << parameters[1];
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                }
            )},
            #endregion

            #region Basic jumps
            { 0x17, new AssemblerInstruction((instruction, context, parameters) => // br
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.ProgramCounter.Jump(parameters[0]);
                }
            )},

            { 0x18, new AssemblerInstruction((instruction, context, parameters) => // jmp
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.ProgramCounter.Branch(parameters[0]);
                }
            )},
            #endregion

            #region Stack operations
            { 0x19, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.StackPointer.Push(processor.Registers[parameters[0]].Value);
                }
            )},

            { 0x1A, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Registers[parameters[0]].Value = processor.StackPointer.Pop();
                }
            )},
            #endregion

            #region Conditional jumps, conditions
            { 0x1B, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.SetFlag(FlagType.EQUAL, processor.Registers[parameters[0]].Value == parameters[1]);
                    processor.SetFlag(FlagType.ZERO, processor.Registers[parameters[0]].Value == 0);
                    processor.SetFlag(FlagType.GREATER, processor.Registers[parameters[0]].Value > parameters[1]);
                }
            )},

            { 0x1C, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if(processor.GetFlag(FlagType.EQUAL))
                        processor.Registers[31].Value += parameters[0];
                }
            )},

            { 0x1D, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (processor.GetFlag(FlagType.ZERO))
                        processor.Registers[31].Value += parameters[0];
                }
            )},

            { 0x1E, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (!processor.GetFlag(FlagType.EQUAL))
                        processor.Registers[31].Value += parameters[0];
                }
            )},

            { 0x1F, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (!processor.GetFlag(FlagType.ZERO))
                        processor.Registers[31].Value += parameters[0];
                }
            )},

            { 0x20, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (processor.GetFlag(FlagType.GREATER))
                        processor.Registers[31].Value += parameters[0];
                }
            )},

            { 0x21, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (!processor.GetFlag(FlagType.GREATER) && !processor.GetFlag(FlagType.EQUAL))
                        processor.Registers[31].Value += parameters[0];
                }
            )},

            { 0x22, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (processor.GetFlag(FlagType.GREATER) || processor.GetFlag(FlagType.EQUAL))
                        processor.Registers[31].Value += parameters[0];
                }
            )},

            { 0x23, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (!processor.GetFlag(FlagType.GREATER))
                        processor.Registers[31].Value += parameters[0];
                }
            )},
            #endregion

            #region Additional operations

            { 0x24, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.StackPointer.Push(processor.Registers[31].Value);
                    processor.Registers[31].Value += parameters[0];
                }
            )},

            { 0x25, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Registers[31].Value = processor.StackPointer.Pop();
                }
            )},

            { 0x26, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Abort();
                }
            )},

            { 0x2B, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    //InterruptAction.Invoke(parameters[0]);
                }
            )},

            { 0x31, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    int reg1 = processor.Registers[parameters[0]].Value;

                    //InterruptAction.Invoke(reg1);
                }
            )},
            #endregion

            #region Memory operations

            { 0x2C, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (parameters[2] == 1)
                        processor.Registers[parameters[0]].Value = processor.Computer.Memory.Read<byte>(parameters[1]);
                    else if (parameters[2] == 2)
                        processor.Registers[parameters[0]].Value = processor.Computer.Memory.Read<short>(parameters[1]);
                    else if (parameters[2] == 4)
                        processor.Registers[parameters[0]].Value = processor.Computer.Memory.Read<int>(parameters[1]);
                }
            )},


            { 0x2D, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (parameters[2] == 1)
                        processor.Computer.Memory.Write<byte>(parameters[0], (byte)processor.Registers[parameters[1]].Value);
                    else if (parameters[2] == 2)
                        processor.Computer.Memory.Write<short>(parameters[0], (short)processor.Registers[parameters[1]].Value);
                    else if (parameters[2] == 4)
                        processor.Computer.Memory.Write<int>(parameters[0], processor.Registers[parameters[1]].Value);
                }
            )},

            { 0x2E, new AssemblerInstruction((instruction, context, parameters) =>
               {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    if (parameters[2] == 1)
                        processor.Computer.Memory.Write<byte>(parameters[0], (byte)parameters[1]);
                    else if (parameters[2] == 2)
                        processor.Computer.Memory.Write<short>(parameters[0], (short)parameters[1]);
                    else if (parameters[2] == 4)
                        processor.Computer.Memory.Write<int>(parameters[0], parameters[1]);
               }
            )},

            { 0x2F, new AssemblerInstruction((instruction, context, parameters) =>
               {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Computer.Memory.Write<int>(parameters[0], processor.Registers[parameters[1]].Value);
               }
            )},

            { 0x30, new AssemblerInstruction((instruction, context, parameters) =>
                {
                    var processor = context.GetContext<Assembler.Emulator.Computer>().GetSpecificProcessor<VCM86Processor>();
                    processor.Registers[parameters[0]].Value = processor.Computer.Memory.Read<int>(parameters[1]);
                }
            )},
            #endregion
        };
    }
}
