using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DestroyNobots.Assembler
{
    public interface IMemory : IDisposable
    {
        byte Read(Pointer address);
        T Read<T>(Pointer address) where T : struct;
        byte[] Read(Pointer address, uint len);
        void Write(Pointer address, byte value);
        void Write<T>(Pointer address, T value) where T : struct;
        void Write(Pointer address, byte[] values);
        void Write(Pointer address, byte value, uint count);
        int MemorySize { get; }
    }
}
