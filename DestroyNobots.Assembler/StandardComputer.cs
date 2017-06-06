using System;
using DestroyNobots.Assembler.Parser;

namespace DestroyNobots.Assembler
{
    public class StandardComputer : Computer
    {
        public override int ProgramCountRegisterNumber { get { return 31; } }
        public override int RegistersCount { get { return 32; } }
        public override int StackPointerRegisterNumber { get { return 30; } }

        public StandardComputer()
            : base()
        {
            Parser.SetConstant("BYTE", 1);
            Parser.SetConstant("byte", 1);
            Parser.SetConstant("WORD", 2);
            Parser.SetConstant("word", 2);
            Parser.SetConstant("DWORD", 4);
            Parser.SetConstant("dword", 4);
        }

        public override void Init()
        {
            for (int i = 0; i < 10; i++)
                Parser.SetRegister("r" + (i + 1), i);

            for (int i = 10; i < 20; i++)
                Parser.SetRegister("a" + (i + 1 - 10), i);

            #region Basic arithmetic operations
            Parser.SetInstruction("add", 0x1, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters) 
                {
                    int reg1 = Registers[parameters[0]].Value;
                    int reg2 = Registers[parameters[1]].Value;

                    Registers[parameters[0]].Value = reg1 + reg2;
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);

            Parser.SetInstruction("add", 0x2, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Registers[parameters[0]].Value = reg1 + parameters[1];
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);

            Parser.SetInstruction("mov", 0x3, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    Registers[parameters[0]].Value = Registers[parameters[1]].Value;
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);

            Parser.SetInstruction("sub", 0x4, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;
                    int reg2 = Registers[parameters[1]].Value;

                    Registers[parameters[0]].Value = reg1 - reg2;
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);

            Parser.SetInstruction("sub", 0x5, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Registers[parameters[0]].Value = reg1 - parameters[1];
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);

            Parser.SetInstruction("mov", 0x6, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    Registers[parameters[0]].Value = parameters[1];
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);

            Parser.SetInstruction("mul", 0x7, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;
                    int reg2 = Registers[parameters[1]].Value;

                    Registers[parameters[0]].Value = reg1 * reg2;
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);

            Parser.SetInstruction("mul", 0x8, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Registers[parameters[0]].Value = reg1 * parameters[1];
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);

            Parser.SetInstruction("div", 0x9, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;
                    int reg2 = Registers[parameters[1]].Value;

                    Registers[parameters[0]].Value = reg1 / reg2;
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);

            Parser.SetInstruction("div", 0xA, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Registers[parameters[0]].Value = reg1 / parameters[1];
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);

            Parser.SetInstruction("mod", 0x27, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Registers[parameters[0]].Value = reg1 % parameters[1];
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);

            Parser.SetInstruction("mod", 0x28, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Registers[parameters[0]].Value = reg1 % Registers[parameters[1]].Value;
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);

            Parser.SetInstruction("inc", 0x29, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    Registers[parameters[0]].Value++;
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 1, false, AssemblerParameters.REGISTER);

            Parser.SetInstruction("dec", 0x2A, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    Registers[parameters[0]].Value--;
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 1, false, AssemblerParameters.REGISTER);
            #endregion

            #region Bit operations

            Parser.SetInstruction("or", 0xB, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;
                    int reg2 = Registers[parameters[1]].Value;

                    Registers[parameters[0]].Value = reg1 | reg2;
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);

            Parser.SetInstruction("or", 0xC, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Registers[parameters[0]].Value = reg1 | parameters[1];
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);

            Parser.SetInstruction("and", 0xE, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;
                    int reg2 = Registers[parameters[1]].Value;

                    Registers[parameters[0]].Value = reg1 & reg2;
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);

            Parser.SetInstruction("and", 0xF, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Registers[parameters[0]].Value = reg1 & parameters[1];
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);


            Parser.SetInstruction("xor", 0x10, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;
                    int reg2 = Registers[parameters[1]].Value;

                    Registers[parameters[0]].Value = reg1 ^ reg2;
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);

            Parser.SetInstruction("xor", 0x11, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Registers[parameters[0]].Value = reg1 ^ parameters[1];
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);

            Parser.SetInstruction("not", 0x12, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Registers[parameters[0]].Value = ~reg1;
                }
                ), 2, false, AssemblerParameters.REGISTER);

            Parser.SetInstruction("shr", 0x13, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;
                    int reg2 = Registers[parameters[1]].Value;

                    Registers[parameters[0]].Value = reg1 >> reg2;
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);

            Parser.SetInstruction("shl", 0x14, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;
                    int reg2 = Registers[parameters[1]].Value;

                    Registers[parameters[0]].Value = reg1 << reg2;
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);

            Parser.SetInstruction("shr", 0x15, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Registers[parameters[0]].Value = reg1 >> parameters[1];
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);

            Parser.SetInstruction("shl", 0x16, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Registers[parameters[0]].Value = reg1 << parameters[1];
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            #endregion

            #region Basic jumps
            Parser.SetInstruction("br", 0x17, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    Registers[31].Value += parameters[0] - 6; // minus size of a branch, which must be constant, otherwise it will not work
                }
                ), 1, true, AssemblerParameters.POINTER);

            Parser.SetInstruction("jmp", 0x18, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    Registers[31].Value = ProgramMemory + parameters[0];
                }
                ), 1, false, AssemblerParameters.POINTER);
            #endregion

            #region Stack operations
            Parser.SetInstruction("push", 0x19, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    StackPush(Registers[parameters[0]]);
                }
                ), 2, false, AssemblerParameters.REGISTER);

            Parser.SetInstruction("pop", 0x1A, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    Registers[parameters[0]].Value = StackPop();
                }
                ), 2, false, AssemblerParameters.REGISTER);
            #endregion

            #region Conditional jumps, conditions
            Parser.SetInstruction("cmp", 0x1B, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    SetFlag(FlagType.EQUAL, Registers[parameters[0]].Value == parameters[1]);
                    SetFlag(FlagType.ZERO, Registers[parameters[0]].Value == 0);
                    SetFlag(FlagType.GREATER, Registers[parameters[0]].Value > parameters[1]);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);

            Parser.SetInstruction("je", 0x1C, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    if(GetFlag(FlagType.EQUAL))
                        Registers[31].Value = ProgramMemory + parameters[0];
                }
                ), 1, false, AssemblerParameters.POINTER);

            Parser.SetInstruction("jz", 0x1D, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    if (GetFlag(FlagType.ZERO))
                        Registers[31].Value = ProgramMemory + parameters[0];
                }
                ), 1, false, AssemblerParameters.POINTER);

            Parser.SetInstruction("jne", 0x1E, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    if (!GetFlag(FlagType.EQUAL))
                        Registers[31].Value = ProgramMemory + parameters[0];
                }
                ), 1, false, AssemblerParameters.POINTER);

            Parser.SetInstruction("jnz", 0x1F, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    if (!GetFlag(FlagType.ZERO))
                        Registers[31].Value = ProgramMemory + parameters[0];
                }
                ), 1, false, AssemblerParameters.POINTER);

            Parser.SetInstruction("jgr", 0x20, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    if (GetFlag(FlagType.GREATER))
                        Registers[31].Value = ProgramMemory + parameters[0];
                }
                ), 1, false, AssemblerParameters.POINTER);

            Parser.SetInstruction("jlo", 0x21, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    if (!GetFlag(FlagType.GREATER) && !GetFlag(FlagType.EQUAL))
                        Registers[31].Value = ProgramMemory + parameters[0];
                }
                ), 1, false, AssemblerParameters.POINTER);

            Parser.SetInstruction("jeg", 0x22, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    if (GetFlag(FlagType.GREATER) || GetFlag(FlagType.EQUAL))
                        Registers[31].Value = ProgramMemory + parameters[0];
                }
                ), 1, false, AssemblerParameters.POINTER);

            Parser.SetInstruction("jel", 0x23, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    if (!GetFlag(FlagType.GREATER))
                        Registers[31].Value = ProgramMemory + parameters[0];
                }
                ), 1, false, AssemblerParameters.POINTER);
            #endregion

            #region Additional operations

            Parser.SetInstruction("call", 0x24, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    StackPush(Registers[31].Value);
                    Registers[31].Value = ProgramMemory + parameters[0];
                }
                ), 1, false, AssemblerParameters.POINTER);

            Parser.SetInstruction("ret", 0x25, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    Registers[31].Value = StackPop();
                }
                ), 0, false);

            Parser.SetInstruction("end", 0x26, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    CancelProgram();
                }
                ), 0, false);

            Parser.SetInstruction("int", 0x2B, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    InterruptAction.Invoke(parameters[0]);
                }
                ), 1, false, AssemblerParameters.VALUE);

            Parser.SetInstruction("int", 0x31, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]];

                    InterruptAction.Invoke(reg1);
                }
                ), 1, false, AssemblerParameters.REGISTER);
            #endregion

            #region Memory operations

            Parser.SetInstruction("mov", 0x2C, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    if (parameters[2] == 1)
                        Registers[parameters[0]].Value = Memory.Read<byte>(parameters[1]);
                    else if (parameters[2] == 2)
                        Registers[parameters[0]].Value = Memory.Read<short>(parameters[1]);
                    else if (parameters[2] == 4)
                        Registers[parameters[0]].Value = Memory.Read<int>(parameters[1]);
                }
                ), 3, false, AssemblerParameters.REGISTER, AssemblerParameters.POINTER, AssemblerParameters.VALUE);


            Parser.SetInstruction("mov", 0x2D, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    if (parameters[2] == 1)
                        Memory.Write<byte>(parameters[0], (byte)Registers[parameters[1]].Value);
                    else if (parameters[2] == 2)
                        Memory.Write<short>(parameters[0], (short)Registers[parameters[1]].Value);
                    else if (parameters[2] == 4)
                        Memory.Write<int>(parameters[0], Registers[parameters[1]].Value);
                }
                ), 3, false, AssemblerParameters.POINTER, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);

            Parser.SetInstruction("mov", 0x2E, new Parser.InstructionAction(
               delegate(Parser.AssemblerInstruction instruction, int[] parameters)
               {
                   if (parameters[2] == 1)
                       Memory.Write<byte>(parameters[0], (byte)parameters[1]);
                   else if (parameters[2] == 2)
                       Memory.Write<short>(parameters[0], (short)parameters[1]);
                   else if (parameters[2] == 4)
                       Memory.Write<int>(parameters[0], parameters[1]);
               }
               ), 3, false, AssemblerParameters.POINTER, AssemblerParameters.VALUE, AssemblerParameters.VALUE);

            Parser.SetInstruction("mov", 0x2F, new Parser.InstructionAction(
               delegate(Parser.AssemblerInstruction instruction, int[] parameters)
               {
                    Memory.Write<int>(parameters[0], Registers[parameters[1]]);
               }
               ), 2, false, AssemblerParameters.POINTER, AssemblerParameters.REGISTER);

            Parser.SetInstruction("mov", 0x30, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    Registers[parameters[0]].Value = Memory.Read<int>(parameters[1]);
                }
                ), 2, false, AssemblerParameters.REGISTER, AssemblerParameters.POINTER);
            #endregion

            Parser.SetInstruction("print", 0x32, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    int reg1 = Registers[parameters[0]].Value;

                    Console.WriteLine(reg1);
                }
                ), 1, false, AssemblerParameters.REGISTER);

            Parser.SetInstruction("print", 0x33, new Parser.InstructionAction(
                delegate(Parser.AssemblerInstruction instruction, int[] parameters)
                {
                    string s = "";
                    int pt = parameters[0];

                    while (Memory.Read<byte>(pt) != 0)
                    {
                        s += (char)Memory.Read<byte>(pt);
                        pt += 1;
                    }

                    Console.WriteLine(s);
                }
                ), 1, false, AssemblerParameters.POINTER);
        }

        public override void InitMemory(out int program_memory_address, out int stack_memory_address, out int stack_size)
        {
            program_memory_address = 0;
            stack_size = 1024;
            stack_memory_address = Memory.MemorySize - (stack_size * 4); 
        }

        public override void Update()
        {

        }
    }
}
