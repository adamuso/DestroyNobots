using System;

namespace DestroyNobots.Assembler
{
    internal class ProgramMemoryReader
    {
        private Computer computer;
        private IMemory memory;

        public ProgramMemoryReader(Computer computer, IMemory memory)
        {
            this.memory = memory;
        }

        public int ReadRegister(ref uint mem, byte pt)
        {
            int? output = null;

            if (pt == 0x00)
            {
                output = memory.Read<byte>(mem);
                mem++;
            }

            if(output == null)
                throw new Exception();

            return output.Value;
        }

        public int ReadValue(ref uint mem, byte pt)
        {
            int? output = null;

            if (pt == 0x00)
            {
                output = memory.Read<byte>(mem);
                mem++;
            }
            else if (pt == 0x01)
            {
                output = memory.Read<short>(mem);
                mem += 2;
            }
            else if (pt == 0x02)
            {
                output = memory.Read<int>(mem);
                mem += 4;
            }

            if(output == null)
                throw new Exception();

            return output.Value;
        }

        public int ReadPointer(ref uint mem, byte pt)
        {
            int? output = null;

            if (pt == 0x00)
            {
                output = memory.Read<int>(mem);
                mem += 4;
            }
            else if (pt == 0x01)
            {
                output = computer.Registers[memory.Read<byte>(mem)].Value;
                mem += 1;
            }

            if (output == null)
                throw new Exception();

            return output.Value;
        }
    }
}