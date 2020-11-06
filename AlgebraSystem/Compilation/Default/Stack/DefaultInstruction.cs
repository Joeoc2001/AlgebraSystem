using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Stack
    {
        internal class DefaultInstruction : IDefaultInstruction
        {
            public DefaultOpcode Opcode { get; }

            public DefaultInstruction(DefaultOpcode opcode)
            {
                this.Opcode = opcode;
            }

            public override bool Equals(object obj)
            {
                return obj is DefaultInstruction instruction &&
                       Opcode == instruction.Opcode;
            }

            public override int GetHashCode()
            {
                return 1881446463 + Opcode.GetHashCode();
            }

            public override string ToString()
            {
                return $"Stack Instr {Opcode}";
            }
        }
    }
}
