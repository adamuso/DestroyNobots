using System;

namespace DestroyNobots.Assembler.Emulator
{
    internal class RAMMemory : IMemory
    {
        private Computer computer;
        private IMemory internalMemory;
        private bool isPowered;

        public int MemorySize { get { return internalMemory.MemorySize; } }

        public RAMMemory(Computer computer, IMemory internalMemory)
        {
            this.computer = computer;
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
            if(address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            return internalMemory.Read(address);
        }

        public byte[] Read(Pointer address, uint len)
        {
            if (address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            return internalMemory.Read(address, len);
        }

        public T1 Read<T1>(Pointer address) where T1 : struct
        {
            if (address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            return internalMemory.Read<T1>(address);
        }

        public void Write(Pointer address, byte[] values)
        {
            if (address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            internalMemory.Write(address, values);
        }

        public void Write(Pointer address, byte value)
        {
            if (address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            internalMemory.Write(address, value);
        }

        public void Write(Pointer address, byte value, uint count)
        {
            if (address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            internalMemory.Write(address, value, count);
        }

        public void Write<T1>(Pointer address, T1 value) where T1 : struct
        {
            if (address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            internalMemory.Write(address, value);
        }
    }
}
