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

        public byte Read(Address address)
        {
            memory.Seek(address, SeekOrigin.Begin);
            return (byte)memory.ReadByte();
        }

        public byte[] Read(Address address, uint len)
        {
            byte[] ret = new byte[len];

            memory.Seek(address, SeekOrigin.Begin);
            memory.Read(ret, 0, (int)len);

            return ret;
        }

        public T Read<T>(Address address) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] structure = Read(address, (uint)size);

            IntPtr mem = Marshal.AllocHGlobal(size);
            Marshal.Copy(structure, 0, mem, size);
            T ret = (T)Marshal.PtrToStructure(mem, typeof(T));
            Marshal.FreeHGlobal(mem);

            return ret;
        }

        public T Read<T>(Address address, out uint size) where T : struct
        {
            size = (uint)Marshal.SizeOf(typeof(T));
            byte[] structure = Read(address, size);

            IntPtr mem = Marshal.AllocHGlobal((int)size);
            Marshal.Copy(structure, 0, mem, (int)size);
            T ret = (T)Marshal.PtrToStructure(mem, typeof(T));
            Marshal.FreeHGlobal(mem);

            return ret;
        }


        public void Write(Address address, byte value)
        {
            memory.Seek(address, SeekOrigin.Begin);
            memory.WriteByte(value);
        }

        public uint Write<T>(Address address, T value) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] structure = new byte[size];

            IntPtr mem = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, mem, false);
            Marshal.Copy(mem, structure, 0, size);
            Marshal.FreeHGlobal(mem);

            Write(address, structure);

            return (uint)size;
        }

        public void Write(Address address, byte[] values)
        {
            memory.Seek(address, SeekOrigin.Begin);
            memory.Write(values, 0, values.Length);
        }

        public void Write(Address address, byte value, uint count)
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
