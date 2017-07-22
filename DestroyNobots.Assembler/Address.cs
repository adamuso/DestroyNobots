namespace DestroyNobots.Assembler
{
    public struct Address : IAssemblerInstructionParameter
    {
        public uint Value { get; private set; }
        public static Address Null { get { return new Address(0); } }

        public Address(uint value)
        {
            Value = value;
        }

        public static Address operator +(Address address, uint value)
        {
            return new Address(address.Value + value);
        }

        public static Address operator +(Address address, ulong value)
        {
            return new Address(address.Value + (uint)value);
        }

        public static Address operator +(Address address, int value)
        {
            return new Address(address.Value + (uint)value);
        }

        public static Address operator +(Address address, long value)
        {
            return new Address(address.Value + (uint)value);
        }

        public static implicit operator uint(Address p)
        {
            return p.Value;
        }

        public static implicit operator Address(uint p)
        {
            return new Address(p) ;
        }

        public static implicit operator Address(int p)
        {
            return new Address((uint)p);
        }

        public static implicit operator Address(ushort p)
        {
            return new Address(p);
        }

        public static implicit operator Address(short p)
        {
            return new Address((uint)p);
        }
    }
}
