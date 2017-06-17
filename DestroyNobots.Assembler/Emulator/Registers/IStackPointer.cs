namespace DestroyNobots.Assembler.Emulator.Registers
{
    public interface IStackPointer
    {
        void Push<T>(T value) where T : struct;
        T Pop<T>() where T : struct;
    }
}