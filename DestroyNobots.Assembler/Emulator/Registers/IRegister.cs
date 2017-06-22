using System;

namespace DestroyNobots.Assembler.Emulator.Registers
{
    public interface IRegister : IAssemblerInstructionParameter
    {
        IConvertible Value { get; }

        byte Size { get; } 

        void Increment();

        void Decrement();

        void Add(IConvertible value);
        void Substract(IConvertible value);
    }
}