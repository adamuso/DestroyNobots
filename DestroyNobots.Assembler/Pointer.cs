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
    }
}
