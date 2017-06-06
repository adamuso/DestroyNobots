using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DestroyNobots.Assembler.Parser;

namespace DestroyNobots.Assembler
{
    public delegate void InterruptAction(int interput);

    public abstract class Computer
    {
        public const int WORD_SIZE = 32;
        public const int REGISTER_BITS = 5;
        public const int NUMBER_OF_REGISTERS = 1 << REGISTER_BITS;

        int ID = new Random((int)DateTime.Now.Ticks).Next(int.MaxValue);

        bool cancel;
        int flags;
        System.IO.StreamWriter stdOut;

        int stackMemory;
        int stackSize;
        private ProgramMemoryReader<int> programMemoryReader;

        internal Dictionary<byte, Parser.AssemblerInstruction> Instructions { get; private set; } // opcodes as keys
        public Register<int>[] Registers { get; private set; }
        internal AssemblerParser Parser { get; private set; }
        internal InterruptAction InterruptAction { get; private set; }

        public IMemory Memory { get; private set; }
        public int ProgramMemory { get; private set; }

        public bool Running { get; private set; } 

        public System.IO.Stream StandardOutput { set { stdOut = new System.IO.StreamWriter(value); } }

        public abstract int ProgramCountRegisterNumber { get; }
        public abstract int StackPointerRegisterNumber { get; }
        public abstract int RegistersCount { get; }

        public Computer()
        {
            Registers = new Register<int>[RegistersCount];
            Instructions = new Dictionary<byte, Parser.AssemblerInstruction>();
            InterruptAction = null;
            stdOut = null;

            for (int r = 0; r < RegistersCount; r++)
                Registers[r] = new Register<int>();

            PrepareMemory(8, BinaryMultiplier.KB);

            this.Parser = new AssemblerParser(this);
            Init();

            Parser.SetRegister("sp", StackPointerRegisterNumber);
            Parser.SetRegister("pc", ProgramCountRegisterNumber);
        }

        internal void RegisterInstruction(byte opcode, Parser.AssemblerInstruction instruction)
        {
            Instructions[opcode] = instruction;
        }

        public void RegisterInterruptAction(InterruptAction action)
        {
            InterruptAction = action;
        }

        public void PrepareMemory(int size, BinaryMultiplier multiplier)
        {
            if(Memory != null)
                Memory.Dispose();

            int programMemory;
            Memory = new SafeMemory(size, multiplier);
            programMemoryReader = null;// new ProgramMemoryReader<int>(this, Memory);
            InitMemory(out programMemory, out stackMemory, out stackSize);

            ProgramMemory = programMemory;
            Registers[30].Value = stackMemory;
            Registers[31].Value = ProgramMemory;
        }

        public void SetStackMemoryStart(int address)
        {
            stackMemory = address;
            Registers[30].Value = stackMemory;
        }

        public void StackPush(int value)
        {
            Memory.Write(Registers[30].Value, value);
            Registers[30].Value++;

            if (Registers[30].Value > stackMemory + stackSize * 4)
                throw new Exception("Stack overflow!");
        }

        public int StackPop()
        {
            Registers[30].Value--;

            if (Registers[30].Value < stackMemory)
                throw new Exception("Stack underflow!");

            return Memory.Read<int>(Registers[30].Value);
        }

        public void Run()
        {
            Running = true;
            RunProgram();
        }

        internal void RunProgram(bool step = false)
        {
            Register<int> current = Registers[31];
            //current.Value = program_memory;
            int instruction = -1;

            while (instruction != 0 && Running)
            {
                instruction = Memory.Read<int>((ushort)current.Value);

                if (instruction == 0 || instruction == -1)
                {
                    Running = false;
                    break;
                }

                byte opcode = (byte)(instruction & 0xFF); //Memory.read<byte>((ushort)current.Value);
                byte paramstypes = (byte)(instruction & 0xFF00); // Memory.read<byte>((ushort)current.Value + 1);
                uint mem = (uint)current.Value + 2;

                Parser.AssemblerInstruction asm = Instructions[opcode];
                int[] param = new int[asm.ParametersCount];

                for (int i = 0; i < asm.ParametersCount; i++)
                {
                    byte pt = (byte)((paramstypes & (0x03 << i * 2)) >> i * 2);

                    if (asm.Parameters[i] == AssemblerParameters.REGISTER)
                        param[i] = programMemoryReader.ReadRegister(ref mem, pt);
                    else if (asm.Parameters[i] == AssemblerParameters.VALUE)
                        param[i] = programMemoryReader.ReadValue(ref mem, pt);
                    else if (asm.Parameters[i] == AssemblerParameters.POINTER)
                        param[i] = programMemoryReader.ReadPointer(ref mem, pt);
                }

                current.Value = (int)mem;

                asm.Eval(param);

                if (cancel)
                {
                    cancel = false;
                    Running = false;
                    break;
                }

                if (step)
                    break;
            }
        }

        public bool Step()
        {
            //Running = true;
            //Register<int> current = Registers[31];

            //int instruction = Memory.read<int>((ushort)current.Value);

            //if (instruction == 0 || instruction == -1)
            //{
            //    Running = false;
            //    return false;
            //}

            //byte opcode = Memory.read<byte>((ushort)current.Value);
            //byte paramstypes = Memory.read<byte>((ushort)current.Value + 1);
            //int mem = current.Value + 2;

            //Parser.ASMInstruction asm = Instructions[opcode];
            //int[] param = new int[asm.ParametersCount];

            //for (int i = 0; i < asm.ParametersCount; i++)
            //{
            //    byte pt = (byte)((paramstypes & (0x03 << i * 2)) >> i * 2);

            //    if (asm.Parameters[i] == ASMParameters.REGISTER)
            //    {
            //        if (pt == 0x00)
            //        {
            //            param[i] = Memory.read<byte>(mem);
            //            mem++;
            //        }
            //    }
            //    else if (asm.Parameters[i] == ASMParameters.VALUE)
            //    {
            //        if (pt == 0x00)
            //        {
            //            param[i] = Memory.read<byte>(mem);
            //            mem++;
            //        }
            //        else if (pt == 0x01)
            //        {
            //            param[i] = Memory.read<short>(mem);
            //            mem += 2;
            //        }
            //        else if (pt == 0x02)
            //        {
            //            param[i] = Memory.read<int>(mem);
            //            mem += 4;
            //        }
            //    }
            //    else if (asm.Parameters[i] == ASMParameters.POINTER)
            //    {
            //        if (pt == 0x00)
            //        {
            //            param[i] = Memory.read<int>(mem);
            //            mem += 4;
            //        }
            //        else if (pt == 0x01)
            //        {
            //            param[i] = Registers[Memory.read<byte>(mem)].Value;
            //            mem += 1;
            //        }
            //    }
            //}

            //current.Value = mem;

            //asm.Eval(param);

            //if (cancel)
            //{
            //    cancel = false;
            //    Running = false;
            //    return false;
            //}

            //return true;

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

        public void CancelProgram()
        {
            cancel = true;
        }

        public void ResetComputer()
        {

        }

        public void Compile(string text)
        {
            Parser.Compile(text);
        }

        public void Compile(System.IO.Stream stream)
        {
            Parser.Compile(stream);

            OutLine("Compiled succesfully!");
        }

        internal int DecodeAddress(int undecode)
        {
            int minus = undecode & 0x1;

            if (minus == 1)
                return -(undecode >> 1);

            return undecode >> 1;
        }

        internal bool GetFlag(FlagType type)
        {
            return (flags & (0x1 << (int)type)) != 0;
        }

        internal void SetFlag(FlagType type, bool val)
        {
            if (val)
                flags |= (0x1 << (int)type);
            else
                flags &= ~(0x1 << (int)type);
        }

        internal void OutLine(string text)
        {
            if (stdOut == null)
                return;

            if (!stdOut.BaseStream.CanWrite)
                return;

            stdOut.WriteLine("[COMPUTER: " + Convert.ToString(ID, 16) + "] " + text);
            stdOut.Flush();
        }

        public abstract void Init();
        public abstract void InitMemory(out int program_memory_address, out int stack_memory_address, out int stack_size);
        public abstract void Update();
    }
}
