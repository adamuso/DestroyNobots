using System;

namespace DestroyNobots.Assembler.Emulator.Registers
{
    public class Register<T> : IRegister where T : IConvertible
    {
        IConvertible IRegister.Value { get { return Value; } }

        public T Value { get; set; }

        public byte Size { get { return (byte)System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)); } }

        public void Increment()
        {
            if (typeof(T) == typeof(byte))
                Value = (T)(object)(Value.ToByte(null) + 1);
            if (typeof(T) == typeof(sbyte))
                Value = (T)(object)(Value.ToSByte(null) + 1);
            if (typeof(T) == typeof(short))
                Value = (T)(object)(Value.ToInt16(null) + 1);
            if (typeof(T) == typeof(ushort))
                Value = (T)(object)(Value.ToUInt16(null) + 1);
            if (typeof(T) == typeof(int))
                Value = (T)(object)(Value.ToInt32(null) + 1);
            if (typeof(T) == typeof(uint))
                Value = (T)(object)(Value.ToUInt32(null) + 1);
            if (typeof(T) == typeof(long))
                Value = (T)(object)(Value.ToInt64(null) + 1);
            if (typeof(T) == typeof(ulong))
                Value = (T)(object)(Value.ToUInt64(null) + 1);

        }

        public void Decrement()
        {
            if (typeof(T) == typeof(byte))
                Value = (T)(object)(Value.ToByte(null) - 1);
            if (typeof(T) == typeof(sbyte))
                Value = (T)(object)(Value.ToSByte(null) - 1);
            if (typeof(T) == typeof(short))
                Value = (T)(object)(Value.ToInt16(null) - 1);
            if (typeof(T) == typeof(ushort))
                Value = (T)(object)(Value.ToUInt16(null) - 1);
            if (typeof(T) == typeof(int))
                Value = (T)(object)(Value.ToInt32(null) - 1);
            if (typeof(T) == typeof(uint))
                Value = (T)(object)(Value.ToUInt32(null) - 1);
            if (typeof(T) == typeof(long))
                Value = (T)(object)(Value.ToInt64(null) - 1);
            if (typeof(T) == typeof(ulong))
                Value = (T)(object)(Value.ToUInt64(null) - 1);
        }
    }
}
