using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestroyNobots.Assembler.Emulator
{
    public interface IProcessorBase
    {
        void Run();
        bool Step();
        void Pause();
        void Abort();
        void Reset();

        AssemblerCompiler GetAssociatedCompiler();

        void Interrupt(byte interrupt);
        bool Running { get; }
        Computer Computer { get; set; }
    }
}
