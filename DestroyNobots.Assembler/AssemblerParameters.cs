namespace DestroyNobots.Assembler
{
    public enum AssemblerParameters
    {
        Register = 0x1,
        Value = 0x2,
        Pointer = 0x4,
        Address = Pointer | Value,
        PointerInRegister = Pointer | Register
        //0x8
        //0x10
        //0x20 ...
    }
}
