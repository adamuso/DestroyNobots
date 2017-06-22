using System;
using System.Runtime.InteropServices;

namespace DestroyNobots.Assembler
{
    public class Pointer<T> : IPointer where T : struct
    {
        IMemory memory;

        public Address Address { get; set; }
        public int Size { get { return Marshal.SizeOf(typeof(T)); } }

        public Pointer(IMemory memory, Address address)
        {
            this.Address = address;
            this.memory = memory;
        }

        public T GetValue()
        {
            return memory.Read<T>(Address);
        }

        public T GetValue(int offset)
        {
            return memory.Read<T>(Address + offset * Size);
        }

        public void SetValue(T value)
        {
            memory.Write(Address, value);
        }

        public void SetValue(int offset, T value)
        {
            memory.Write(Address + offset * Size, value);
        }

        public Pointer<T1> As<T1>() where T1 : struct
        {
            return new Pointer<T1>(memory, Address);
        }

        void IPointer.SetValue<T1>(T1 value)
        {
            memory.Write(Address, value);
        }

        void IPointer.SetValue<T1>(int offset, T1 value)
        {
            memory.Write(Address + offset, value);
        }

        T1 IPointer.GetValue<T1>()
        {
            return memory.Read<T1>(Address);
        }

        T1 IPointer.GetValue<T1>(int offset)
        {
            return memory.Read<T1>(Address + offset);
        }

        public static int TypeSize { get { return Marshal.SizeOf(typeof(T)); } }
    }
}
