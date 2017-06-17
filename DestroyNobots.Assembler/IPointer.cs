namespace DestroyNobots.Assembler
{
    public interface IPointer : IAssemblerInstructionParameter
    {
        Address Address { get; set; }
        int Size { get; }

        void SetValue<T>(T value) where T : struct;
        void SetValue<T>(int offset, T value) where T : struct;
        T GetValue<T>() where T : struct;
        T GetValue<T>(int offset) where T : struct;
        Pointer<T> As<T>() where T : struct;
    }
}