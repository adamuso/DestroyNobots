using System;

namespace DestroyNobots.Assembler.Emulator.Registers
{
    public class RegisterProxy<T> : Register<T> where T : IConvertible
    {
        public Func<T> Get { get; set; }
        public Action<T> Set { get; set; }

        public override T Value { get { return Get(); } set { Set(value); } }
    }
}
