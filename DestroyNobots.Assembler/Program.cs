using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DestroyNobots.Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            Parser.MathParser m = new Parser.MathParser();
            m.eval("2 * 10 % 4");

            sw.Start();
            int i = 20000;
            while (i != 0)
            {
                
                i -= 2;
            }
            sw.Stop();
            TimeSpan el = sw.Elapsed;
            Computer test;
            test = new StandardComputer();
            test.Parser.Compile(new System.IO.FileStream("AsmTest.txt", System.IO.FileMode.Open));
            sw.Reset();
            sw.Start();
            test.Run();
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine("Program ended. Elapsed time: " + sw.Elapsed);
            Console.WriteLine("Program ended. Elapsed time: " + el);
            Console.ReadKey(true);
        }
    }
}
