using System;

namespace DestroyNobots.Assembler.Emulator.Registers
{
    public class ProgramCounter<T>
        where T : struct, IConvertible
    {
        private Processor<T> processor;
        private Register<T> register;

        public uint Address { get { return register.Value.ToUInt32(null); } }

        public ProgramCounter(Processor<T> processor, Register<T> register)
        {
            this.processor = processor;
            this.register = register;
        }

        public void Set(int address)
        {
            register.Value = (T)Convert.ChangeType(address, typeof(T));
        }

        public void Set(uint address)
        {
            register.Value = (T)Convert.ChangeType(address, typeof(T));
        }

        public void Branch(int address)
        {
            register.Value = (T)Convert.ChangeType(register.Value.ToUInt32(null) + address, typeof(T));
        }

        public void Jump(Address address)
        {
            register.Value = (T)Convert.ChangeType(address.Value, typeof(T));
        }

        public void Call(Address address)
        {
            processor.StackPointer.Push(register.Value);
            register.Value = (T)Convert.ChangeType(address.Value, typeof(T));
        }

        public void CallInterrupt(byte interrupt)
        {
            if (processor.InterruptDescriptorTablePointer != null)
            {
                Call(processor.InterruptDescriptorTablePointer.GetValue(interrupt * 4));
            }
        }

        public void Return()
        {
            register.Value = processor.StackPointer.Pop();
        }
    }
}
