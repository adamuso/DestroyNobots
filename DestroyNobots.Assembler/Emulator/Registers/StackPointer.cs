using System;

namespace DestroyNobots.Assembler.Emulator.Registers
{
    public class StackPointer : IStackPointer
    {
        private IProcessorBase processor;
        private IRegister register;
        private uint stackMemoryStart;
        private uint stackSize;

        public StackPointer(IProcessorBase processor, IRegister register, uint stackMemoryStart, uint stackSize)
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

        public void Push<T1>(T1 value) where T1 : struct
        {
            uint size = processor.Context.Memory.Write(register.Value.ToUInt32(null), value);
            register.Add(size);

            if (register.Value.ToUInt32(null) > stackMemoryStart + stackSize * 4)
            {
                processor.Interrupt(2);
                throw new Exception("Stack overflow!");
            }
        }

        public T1 Pop<T1>() where T1 : struct
        {
            if (register.Value.ToUInt32(null) < stackMemoryStart)
            {
                processor.Interrupt(3);
                throw new Exception("Stack underflow!");
            }

            uint size;
            T1 value = processor.Context.Memory.Read<T1>(register.Value.ToUInt32(null), out size);
            register.Substract(size);

            return value;
        }
    }
}
