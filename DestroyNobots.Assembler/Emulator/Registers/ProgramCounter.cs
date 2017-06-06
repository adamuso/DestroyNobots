using System;

namespace DestroyNobots.Assembler.Emulator.Registers
{
    public class ProgramCounter<T>
        where T : struct, IConvertible
    {
        private Processor<T> processor;
        private Register<T> register;

        public ProgramCounter(Processor<T> processor, Register<T> register)
        {
            this.processor = processor;
            this.register = register;
        }

        public void Set(int address)
        {
            register.Value = (T)(object)address;
        }

        public void Set(uint address)
        {
            register.Value = (T)(object)address;
        }

        public void Jump(Pointer address)
        {
            register.Value = (T)(object)(uint)address; 
        }

        public void Call(Pointer address)
        {
            processor.StackPointer.Push(register.Value);
            register.Value = (T)(object)(uint)address;
        }

        public void Return()
        {
            register.Value = processor.StackPointer.Pop();
        }

        public uint Address { get { return (uint)(object)register.Value; } }
    }
}
