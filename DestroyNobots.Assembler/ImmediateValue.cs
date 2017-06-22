using System;

namespace DestroyNobots.Assembler
{
    public class ImmediateValue : IConvertible, IAssemblerInstructionParameter
    {
        public IConvertible Value { get; set; }

        public TypeCode GetTypeCode()
        {
            return Value.GetTypeCode();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return Value.ToBoolean(provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return Value.ToByte(provider);
        }

        public char ToChar(IFormatProvider provider)
        {
            return Value.ToChar(provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Value.ToDateTime(provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Value.ToDecimal(provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return Value.ToDouble(provider);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return Value.ToInt16(provider);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Value.ToInt32(provider);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Value.ToInt64(provider);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return Value.ToSByte(provider);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return Value.ToSingle(provider);
        }

        public string ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Value.ToType(conversionType, provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return Value.ToUInt16(provider);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return Value.ToUInt32(provider);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return Value.ToUInt64(provider);
        }
    }
}
