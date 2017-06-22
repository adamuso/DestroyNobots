using DestroyNobots.Assembler.Emulator.Registers;

namespace DestroyNobots.Assembler.Emulator
{
    public interface IProcessorBase
    {
        bool Running { get; }
        IRuntimeContext Context { get; set; }
        IRegister[] Registers { get; }
        IStackPointer StackPointer { get; }
        Pointer<Address> InterruptDescriptorTablePointer { get; }

        void Initialize();
        void Run();
        bool Step();
        void Pause();
        void Abort();
        void Reset();

        AssemblerCompiler GetAssociatedCompiler();

        void Interrupt(byte interrupt);
    }
}
