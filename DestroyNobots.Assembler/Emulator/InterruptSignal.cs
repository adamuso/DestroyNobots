using System;

namespace DestroyNobots.Assembler.Emulator
{
    public class InterruptSignal : Exception
    {
        public byte Interrupt { get; private set; }

        public InterruptSignal(byte interrupt)
        {
            Interrupt = interrupt;
        }
    }
}