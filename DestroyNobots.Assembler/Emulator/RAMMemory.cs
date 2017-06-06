using System;

namespace DestroyNobots.Assembler.Emulator
{
    internal class RAMMemory : IMemory
    {
        private IMemory internalMemory;
        private bool isPowered;

        public int MemorySize { get { return internalMemory.MemorySize; } }

        public RAMMemory(IMemory internalMemory)
        {
            this.internalMemory = internalMemory;
        }

        public void PowerUp()
        {
            isPowered = true;
        }

        public void PowerDown()
        {
            internalMemory.Write(0, 0, (uint)internalMemory.MemorySize);
            isPowered = false;
        }

        public void Dispose()
        {
            internalMemory.Dispose();
        }

        public byte Read(Pointer address)
        {
            return internalMemory.Read(address);
        }

        public byte[] Read(Pointer address, uint len)
        {
            return internalMemory.Read(address, len);
        }

        public T1 Read<T1>(Pointer address) where T1 : struct
        {
            return internalMemory.Read<T1>(address);
        }

        public void Write(Pointer address, byte[] values)
        {
            internalMemory.Write(address, values);
        }

        public void Write(Pointer address, byte value)
        {
            internalMemory.Write(address, value);
        }

        public void Write(Pointer address, byte value, uint count)
        {
            internalMemory.Write(address, value, count);
        }

        public void Write<T1>(Pointer address, T1 value) where T1 : struct
        {
            internalMemory.Write(address, value);
        }
    }
}
