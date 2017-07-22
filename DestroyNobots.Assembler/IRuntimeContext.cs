using DestroyNobots.Assembler.Emulator;

namespace DestroyNobots.Assembler
{
    public interface IRuntimeContext
    {
        IProcessorBase Processor { get; }
        IMemory Memory { get; }

        T GetContext<T>();
    }
}
