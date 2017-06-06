using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DestroyNobots.Assembler
{
    public enum BinaryMultiplier : int
    {
        B = 1,
        KB = 1 << 10,
        MB = 1 << 20,
        GB = 1 << 30
    }
}
