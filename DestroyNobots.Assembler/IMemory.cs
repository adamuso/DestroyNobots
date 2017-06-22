using System;

namespace DestroyNobots.Assembler
{
    public interface IMemory : IDisposable
    {
        byte Read(Address address);
        T Read<T>(Address address) where T : struct;
        T Read<T>(Address address, out uint size) where T : struct;
        byte[] Read(Address address, uint len);
        void Write(Address address, byte value);
        uint Write<T>(Address address, T value) where T : struct;
        void Write(Address address, byte[] values);
        void Write(Address address, byte value, uint count);
        int MemorySize { get; }
    }
}
