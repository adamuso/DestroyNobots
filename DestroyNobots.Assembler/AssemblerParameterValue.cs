namespace DestroyNobots.Assembler
{
    public struct AssemblerParameterValue
    {
        public int Value { get; private set; }
        public PointerType? PointerSize { get; private set; }
        public RegisterType? RegisterType { get; private set; }

        public AssemblerParameterValue(int value, PointerType? pointerSize, RegisterType? registerType)
        {
            this.Value = value;
            this.PointerSize = pointerSize;
            this.RegisterType = registerType;
        }

        public static implicit operator AssemblerParameterValue(int value)
        {
            return new AssemblerParameterValue(value, null, null);
        }

        public static implicit operator int(AssemblerParameterValue value)
        {
            return value.Value;
        }

        public static implicit operator Address(AssemblerParameterValue value)
        {
            return (Address)value.Value;
        }
    }
}