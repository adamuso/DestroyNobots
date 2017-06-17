using System;

namespace DestroyNobots.Assembler.Emulator.Registers
{
    public class Register<T> : IRegister where T : IConvertible
    {
        IConvertible IRegister.Value { get { return Value; } }

        public T Value { get; set; }

        public byte Size { get { return (byte)System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)); } }

        public void Add(IConvertible value)
        {
            if (typeof(T) == typeof(byte))
                Value = (T)(object)(Value.ToByte(null) + value.ToByte(null));
            if (typeof(T) == typeof(sbyte))
                Value = (T)(object)(Value.ToSByte(null) + value.ToSByte(null));
            if (typeof(T) == typeof(short))
                Value = (T)(object)(Value.ToInt16(null) + value.ToInt16(null));
            if (typeof(T) == typeof(ushort))
                Value = (T)(object)(Value.ToUInt16(null) + value.ToUInt16(null));
            if (typeof(T) == typeof(int))
                Value = (T)(object)(Value.ToInt32(null) + value.ToInt32(null));
            if (typeof(T) == typeof(uint))
                Value = (T)(object)(Value.ToUInt32(null) + value.ToUInt32(null));
            if (typeof(T) == typeof(long))
                Value = (T)(object)(Value.ToInt64(null) + value.ToInt64(null));
            if (typeof(T) == typeof(ulong))
                Value = (T)(object)(Value.ToUInt64(null) + value.ToUInt64(null));
        }

        public void Substract(IConvertible value)
        {
            if (typeof(T) == typeof(byte))
                Value = (T)(object)(Value.ToByte(null) - value.ToByte(null));
            if (typeof(T) == typeof(sbyte))
                Value = (T)(object)(Value.ToSByte(null) - value.ToSByte(null));
            if (typeof(T) == typeof(short))
                Value = (T)(object)(Value.ToInt16(null) - value.ToInt16(null));
            if (typeof(T) == typeof(ushort))
                Value = (T)(object)(Value.ToUInt16(null) - value.ToUInt16(null));
            if (typeof(T) == typeof(int))
                Value = (T)(object)(Value.ToInt32(null) - value.ToInt32(null));
            if (typeof(T) == typeof(uint))
                Value = (T)(object)(Value.ToUInt32(null) - value.ToUInt32(null));
            if (typeof(T) == typeof(long))
                Value = (T)(object)(Value.ToInt64(null) - value.ToInt64(null));
            if (typeof(T) == typeof(ulong))
                Value = (T)(object)(Value.ToUInt64(null) - value.ToUInt64(null));
        }

        public void Increment()
        {
            Add(1);
        }

        public void Decrement()
        {
            Substract(1);
        }
    }
}
