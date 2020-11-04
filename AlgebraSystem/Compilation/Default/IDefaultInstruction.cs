using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default
    {
        internal interface IDefaultInstruction
        {
            DefaultOpcode Opcode { get; }
        }
    }
}
