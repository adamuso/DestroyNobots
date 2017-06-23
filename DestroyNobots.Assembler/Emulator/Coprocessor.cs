using DestroyNobots.Assembler.Emulator.Registers;
using System;

namespace DestroyNobots.Assembler.Emulator
{
    public class Coprocessor
    {
        private double[] stack;
        private byte stackTop;

        public Coprocessor()
        {
            stack = new double[8];
            stackTop = 0;
        }

        public void Push(double value)
        {
            if (stackTop >= 7)
                throw new Exception("Stack overflow");

            stack[stackTop] = value;
            stackTop++;
        }

        public double Pop()
        {
            if (stackTop == 0)
                throw new Exception("Stack underflow");

            stackTop--;
            double value = stack[stackTop];

            return value;
        }

        public Register<double> GetRegister(int index)
        {
            return new RegisterProxy<double>() { Get = () => stack[(index + stackTop) % 8], Set = (value) => stack[(index + stackTop) % 8] = value };
        }
    }
}
