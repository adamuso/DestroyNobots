using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DestroyNobots.Assembler
{
    internal class UnsafeHGlobalMemory : IMemory
    {
        IntPtr memory;
        int memsize;

        public UnsafeHGlobalMemory(int size, BinaryMultiplier multiplier)
        {
            this.memsize = size * (int)multiplier;

            memory = Marshal.AllocHGlobal(memsize);
        }

        public byte Read(Address address)
        {
            return Marshal.ReadByte(new IntPtr(memory.ToInt64() + address));
        }

        public byte[] Read(Address address, int len)
        {
            byte[] ret = new byte[len];
            Marshal.Copy(new IntPtr(memory.ToInt64() + address), ret, 0, len);
            return ret;
        }

        public T Read<T>(Address address) where T : struct
        {
            T ret = new T();
            ret = (T)Marshal.PtrToStructure(new IntPtr(memory.ToInt64() + address), typeof(T));
            return ret;
        }

        public void Write(Address address, byte value)
        {
            Marshal.WriteByte(new IntPtr(memory.ToInt64() + address), value);
        }

        public void Write<T>(Address address, T value) where T : struct
        {
            Marshal.StructureToPtr(value, new IntPtr(memory.ToInt64() + address), true);
        }

        public void Write(Address address, byte[] values)
        {
            Marshal.Copy(values, 0, new IntPtr(memory.ToInt64() + address), values.Length);
        }

        public void Write(Address address, byte value, uint count)
        {
            for(uint i = 0; i < count; i++)
            {
                Marshal.WriteByte(new IntPtr(memory.ToInt64() + address + i), value);
            }
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(memory);
        }

        public byte[] Read(Address address, uint len)
        {
            throw new NotImplementedException();
        }

        internal ulong StartMemory { get { return (ulong)memory.ToInt64(); } }
        public int MemorySize { get { return memsize; } }
    }
}
