using System;

namespace DestroyNobots.Assembler.Emulator.Registers
{
    public class StackPointer<T> 
        where T : struct, IConvertible
    {
        private Processor<T> processor;
        private Register<T> register;
        private uint stackMemoryStart;
        private uint stackSize;

        public StackPointer(Processor<T> processor, Register<T> register, uint stackMemoryStart, uint stackSize)
        {
            this.processor = processor;
            this.register = register;
            this.stackMemoryStart = stackMemoryStart;
            this.stackSize = stackSize;
        }

        public void Set(uint stackMemoryStart, uint stackSize)
        {
            this.stackMemoryStart = stackMemoryStart;
            this.stackSize = stackSize;
        }

        public void Push(T value)
        {
            processor.Computer.Memory.Write(register.Value.ToUInt32(null), value);
            register.Increment();

            if (register.Value.ToUInt32(null) > stackMemoryStart + stackSize * 4)
            {
                processor.Interrupt(2);
                throw new Exception("Stack overflow!");
            }
        }

        public T Pop()
        {
            register.Decrement();

            if (register.Value.ToUInt32(null) < stackMemoryStart)
            {
                processor.Interrupt(3);
                throw new Exception("Stack underflow!");
            }

            return processor.Computer.Memory.Read<T>(register.Value.ToUInt32(null));
        }
    }
}
