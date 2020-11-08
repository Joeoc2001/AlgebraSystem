using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Stack
    {
        internal interface IDefaultStackInstruction
        {
            DefaultOpcode Opcode { get; }
        }
    }
}
