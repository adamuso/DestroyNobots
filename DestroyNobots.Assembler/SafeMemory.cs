using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DestroyNobots.Assembler
{
    public class SafeMemory : IMemory
    {
        byte[] internalMemory;
        MemoryStream memory;
        int memsize;

        public SafeMemory(int size, BinaryMultiplier multiplier)
        {
            memsize = size * (int)multiplier;
            internalMemory = new byte[memsize];

            memory = new MemoryStream(internalMemory, true);
        }

        public byte Read(Pointer address)
        {
            memory.Seek(address, SeekOrigin.Begin);
            return (byte)memory.ReadByte();
        }

        public byte[] Read(Pointer address, uint len)
        {
            byte[] ret = new byte[len];

            memory.Seek(address, SeekOrigin.Begin);
            memory.Read(ret, 0, (int)len);

            return ret;
        }

        public T Read<T>(Pointer address) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] structure = Read(address, (uint)size);

            IntPtr mem = Marshal.AllocHGlobal(size);
            Marshal.Copy(structure, 0, mem, size);
            T ret = (T)Marshal.PtrToStructure(mem, typeof(T));
            Marshal.FreeHGlobal(mem);

            return ret;
        }

        public void Write(Pointer address, byte value)
        {
            memory.Seek(address, SeekOrigin.Begin);
            memory.WriteByte(value);
        }

        public void Write<T>(Pointer address, T value) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] structure = new byte[size];

            IntPtr mem = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, mem, false);
            Marshal.Copy(mem, structure, 0, size);
            Marshal.FreeHGlobal(mem);

            Write(address, structure);
        }

        public void Write(Pointer address, byte[] values)
        {
            memory.Seek(address, SeekOrigin.Begin);
            memory.Write(values, 0, values.Length);
        }

        public void Write(Pointer address, byte value, uint count)
        {
            memory.Seek(address, SeekOrigin.Begin);

            for(uint i = 0; i < count; i++)
            {
                memory.WriteByte(value);
            }
        }

        public void Dispose()
        {
            memory.Dispose();
        }


        public int MemorySize { get { return memsize; } }
    }
}
