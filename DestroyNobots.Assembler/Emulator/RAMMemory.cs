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

        public byte Read(Address address)
        {
            if(address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            return internalMemory.Read(address);
        }

        public byte[] Read(Address address, uint len)
        {
            if (address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            return internalMemory.Read(address, len);
        }

        public T1 Read<T1>(Address address) where T1 : struct
        {
            if (address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            return internalMemory.Read<T1>(address);
        }

        public void Write(Address address, byte[] values)
        {
            if (address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            internalMemory.Write(address, values);
        }

        public void Write(Address address, byte value)
        {
            if (address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            internalMemory.Write(address, value);
        }

        public void Write(Address address, byte value, uint count)
        {
            if (address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            internalMemory.Write(address, value, count);
        }

        public void Write<T1>(Address address, T1 value) where T1 : struct
        {
            if (address.Value >= MemorySize)
                computer.Processor.Interrupt(1);

            internalMemory.Write(address, value);
        }
    }
}
