using DestroyNobots.Assembler.Emulator.Registers;
using DestroyNobots.Assembler.Parser;
using System;
using System.Collections.Generic;

namespace DestroyNobots.Assembler.Emulator
{
    public abstract class Processor<T> : IProcessorBase, IInstructionSetProvider
        where T : struct, IConvertible
    {
        public const int WORD_SIZE = 32;
        public const int REGISTER_BITS = 5;
        public const int NUMBER_OF_REGISTERS = 1 << REGISTER_BITS;

        bool abort;
        int flags;

        int stackMemory;
        int stackSize;
        private ProgramMemoryReader<T> programMemoryReader;

        public Dictionary<byte, AssemblerInstruction> InstructionSet { get; private set; } // opcodes as keys

        public Pointer? InterruptDescriptorTablePointer { get; set; }

        public Register<T>[] Registers { get; private set; }
        public ProgramCounter<T> ProgramCounter { get; private set; }
        public StackPointer<T> StackPointer { get; private set; }

        public bool Running { get; private set; }

        public Computer Computer { get; set; }

        public Processor(Dictionary<byte, AssemblerInstruction> instructions)
        {
            this.Computer = null;
            Registers = new Register<T>[RegistersCount];
            InstructionSet = instructions; 

            flags = 0;
            abort = false;

            for (int r = 0; r < RegistersCount; r++)
                Registers[r] = new Register<T>();

            StackPointer = new StackPointer<T>(this, Registers[StackPointerRegisterNumber], 0, 0);
            ProgramCounter = new ProgramCounter<T>(this, Registers[ProgramCountRegisterNumber]);
        }

        public void Run()
        {
            Running = true;
            RunProgram();
        }

        public bool Step()
        {
            Running = true;
            RunProgram(true);

            if (!Running)
                return false;

            return true;
        }

        public void Pause()
        {
            Running = false;
        }

        public void Abort()
        {
            abort = true;
        }

        public void Reset()
        {
            abort = false;
        }

        public void Interrupt(byte interrupt)
        {
            if (InterruptDescriptorTablePointer == null)
            {
#if DEBUG
                Console.WriteLine("Aborted, int: " + interrupt.ToString("X"));
#endif
                Abort();
            }
            else
                ProgramCounter.CallInterrupt(interrupt);
                
            throw new InterruptSignal(interrupt);
        }

        protected int DecodeAddress(int undecode)
        {
            int minus = undecode & 0x1;

            if (minus == 1)
                return -(undecode >> 1);

            return undecode >> 1;
        }

        public bool GetFlag(FlagType type)
        {
            return (flags & (0x1 << (int)type)) != 0;
        }

        public void SetFlag(FlagType type, bool val)
        {
            if (val)
                flags |= (0x1 << (int)type);
            else
                flags &= ~(0x1 << (int)type);
        }

        public abstract AssemblerCompiler GetAssociatedCompiler();

        protected void RegisterInstruction(byte opcode, AssemblerInstruction instruction)
        {
            InstructionSet.Add(opcode, instruction);
        }

        //protected void RegisterInterruptAction(InterruptAction action)
        //{
        //    InterruptAction = action;
        //}

        private void RunProgram(bool step = false)
        {
            if(programMemoryReader == null)
                programMemoryReader = new Assembler.ProgramMemoryReader<T>(this, Computer.Memory);

            int instruction = -1;

            while (instruction != 0 && Running)
            {
                if (abort)
                {
                    Running = false;
                    break;
                }

                instruction = Computer.Memory.Read<int>(ProgramCounter.Address);

                if (instruction == 0 || instruction == -1)
                {
                    Running = false;
                    break;
                }

                byte opcode = (byte)(instruction & 0xFF); //Memory.read<byte>((ushort)current.Value);
                byte paramstypes = (byte)((instruction & 0xFF00) >> 8); // Memory.read<byte>((ushort)current.Value + 1);
                uint mem = ProgramCounter.Address + 2;

                AssemblerInstruction asm = InstructionSet[opcode];
                int[] param = new int[asm.ParametersCount];

                for (int i = 0; i < asm.ParametersCount; i++)
                {
                    byte pt = (byte)((paramstypes & (0x03 << i * 2)) >> i * 2);

                    if (asm.Parameters[i] == AssemblerParameters.Register)
                        param[i] = programMemoryReader.ReadRegister(ref mem, pt);
                    else if ((asm.Parameters[i] & AssemblerParameters.Value) != 0)
                        param[i] = programMemoryReader.ReadValue(ref mem, pt);
                    else if (asm.Parameters[i] == AssemblerParameters.Pointer)
                        param[i] = programMemoryReader.ReadPointer(ref mem, pt);
                }

                ProgramCounter.Set(mem);

                try
                {
                    asm.Eval(Computer, param);
                }
                catch(InterruptSignal interrupt)
                {

                }

                if (step)
                    break;
            }
        }


        public abstract byte ProgramCountRegisterNumber { get; }
        public abstract byte StackPointerRegisterNumber { get; }
        public abstract byte RegistersCount { get; }

        public abstract void Update();
    }
}
