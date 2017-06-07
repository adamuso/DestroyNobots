using System;
using DestroyNobots.Assembler;
using DestroyNobots.Assembler.Emulator;
using DestroyNobots.Assembler.Parser;

namespace DestroyNobots.Computers
{
    public class VCM86Processor : Processor<int>
    {
        public override byte ProgramCountRegisterNumber { get { return 9; } }
        public override byte RegistersCount { get { return 10; } }
        public override byte StackPointerRegisterNumber { get { return 8; } }

        public VCM86Processor() 
            : base(InstructionSets.VCM86)
        {

        }

        public override void Update()
        {

        }

        public override AssemblerCompiler GetAssociatedCompiler()
        {
            AssemblerCompiler compiler = new AssemblerCompiler(this);

            #region Basic arithmetic operations
            compiler.SetInstruction("add", 0x1, 2,  AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);
            compiler.SetInstruction("add", 0x2, 2,  AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("mov", 0x3, 2,  AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);
            compiler.SetInstruction("sub", 0x4, 2,  AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);
            compiler.SetInstruction("sub", 0x5, 2,  AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("mov", 0x6, 2,  AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("mul", 0x7, 2,  AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);
            compiler.SetInstruction("mul", 0x8, 2,  AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("div", 0x9, 2,  AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);
            compiler.SetInstruction("div", 0xA, 2,  AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("mod", 0x27, 2,  AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("mod", 0x28, 2,  AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);
            compiler.SetInstruction("inc", 0x29, 1,  AssemblerParameters.REGISTER);
            compiler.SetInstruction("dec", 0x2A, 1,  AssemblerParameters.REGISTER);
            #endregion

            #region Bit operations
            compiler.SetInstruction("or", 0xB, 2,  AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);
            compiler.SetInstruction("or", 0xC, 2,  AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("and", 0xE, 2,  AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);
            compiler.SetInstruction("and", 0xF, 2,  AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("xor", 0x10, 2,  AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);
            compiler.SetInstruction("xor", 0x11, 2,  AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("not", 0x12, 2,  AssemblerParameters.REGISTER);
            compiler.SetInstruction("shr", 0x13, 2,  AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);
            compiler.SetInstruction("shl", 0x14, 2,  AssemblerParameters.REGISTER, AssemblerParameters.REGISTER);
            compiler.SetInstruction("shr", 0x15, 2,  AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("shl", 0x16, 2,  AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            #endregion

            #region Basic jumps
            compiler.SetInstruction("br", 0x17, 1,  AssemblerParameters.POINTER);
            compiler.SetInstruction("jmp", 0x18, 1,  AssemblerParameters.POINTER);
            #endregion

            #region Stack operations
            compiler.SetInstruction("push", 0x19, 2,  AssemblerParameters.REGISTER);
            compiler.SetInstruction("pop", 0x1A, 2,  AssemblerParameters.REGISTER);
            #endregion

            #region Conditional jumps, conditions
            compiler.SetInstruction("cmp", 0x1B, 2,  AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("je", 0x1C, 1,  AssemblerParameters.POINTER);
            compiler.SetInstruction("jz", 0x1D, 1,  AssemblerParameters.POINTER);
            compiler.SetInstruction("jne", 0x1E, 1,  AssemblerParameters.POINTER);
            compiler.SetInstruction("jnz", 0x1F, 1,  AssemblerParameters.POINTER);
            compiler.SetInstruction("jgr", 0x20, 1,  AssemblerParameters.POINTER);
            compiler.SetInstruction("jlo", 0x21, 1,  AssemblerParameters.POINTER);
            compiler.SetInstruction("jeg", 0x22, 1,  AssemblerParameters.POINTER);
            compiler.SetInstruction("jel", 0x23, 1,  AssemblerParameters.POINTER);
            #endregion

            #region Additional operations
            compiler.SetInstruction("lidt", 0xD, 1, AssemblerParameters.REGISTER);
            compiler.SetInstruction("call", 0x24, 1,  AssemblerParameters.POINTER);
            compiler.SetInstruction("ret", 0x25, 1,  AssemblerParameters.VALUE);
            compiler.SetInstruction("int", 0x31, 1,  AssemblerParameters.REGISTER);
            #endregion

            #region Memory operations
            compiler.SetInstruction("mov", 0x2C, 3,  AssemblerParameters.REGISTER, AssemblerParameters.POINTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("mov", 0x2D, 3,  AssemblerParameters.POINTER, AssemblerParameters.REGISTER, AssemblerParameters.VALUE);
            compiler.SetInstruction("mov", 0x2E, 3,  AssemblerParameters.POINTER, AssemblerParameters.VALUE, AssemblerParameters.VALUE);
            compiler.SetInstruction("mov", 0x2F, 2,  AssemblerParameters.POINTER, AssemblerParameters.REGISTER);
            compiler.SetInstruction("mov", 0x30, 2,  AssemblerParameters.REGISTER, AssemblerParameters.POINTER);
            #endregion

            #region I/O Operations
            compiler.SetInstruction("out", 0x32, 2, AssemblerParameters.VALUE, AssemblerParameters.VALUE);
            compiler.SetInstruction("out", 0x33, 2, AssemblerParameters.VALUE, AssemblerParameters.REGISTER);
            compiler.SetInstruction("in", 0x34, 2, AssemblerParameters.VALUE, AssemblerParameters.REGISTER);
            #endregion

            for (int i = 0; i < RegistersCount - 2; i++)
                compiler.SetRegister("r" + (i + 1), i);

            compiler.SetRegister("sp", StackPointerRegisterNumber);
            compiler.SetRegister("pc", ProgramCountRegisterNumber);

            return compiler;
        }
    }
}
