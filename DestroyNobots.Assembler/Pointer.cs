using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DestroyNobots.Assembler
{
    public struct Pointer
    {
        public uint Value { get; private set; }

        public Pointer(uint value)
        {
            this.Value = value;
        }

        public static implicit operator uint(Pointer p)
        {
            return p.Value;
        }

        public static implicit operator Pointer(uint p)
        {
            return new Pointer(p) ;
        }

        public static implicit operator Pointer(int p)
        {
            return new Pointer((uint)p);
        }

        public static implicit operator Pointer(ushort p)
        {
            return new Pointer(p);
        }

        public static implicit operator Pointer(short p)
        {
            return new Pointer((uint)p);
        }
    }
}
