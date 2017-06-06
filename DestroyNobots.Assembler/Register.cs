using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DestroyNobots.Assembler
{
    public class Register<T> where T : struct
    {
        T value;

        public Register()
        {
            value = new T();
        }

        public static implicit operator T(Register<T> reg)
        {
            return reg.value;
        }

        public T Value { get { return value; } set { this.value = value; } }
    }
}
