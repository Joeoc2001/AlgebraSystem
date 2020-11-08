using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Stack
    {
        internal struct DefaultHeapInstruction<ADDRESS_SIZE>
        {
            public DefaultOpcode Opcode { get; }
            public ADDRESS_SIZE Arg_1 { get; }
            public ADDRESS_SIZE Arg_2 { get; }

            public DefaultHeapInstruction(DefaultOpcode opcode, ADDRESS_SIZE arg_1, ADDRESS_SIZE arg_2)
            {
                Opcode = opcode;
                Arg_1 = arg_1;
                Arg_2 = arg_2;
            }

            public override bool Equals(object obj)
            {
                return obj is DefaultHeapInstruction<ADDRESS_SIZE> instruction &&
                       Opcode == instruction.Opcode &&
                       EqualityComparer<ADDRESS_SIZE>.Default.Equals(Arg_1, instruction.Arg_1) &&
                       EqualityComparer<ADDRESS_SIZE>.Default.Equals(Arg_2, instruction.Arg_2);
            }

            public override int GetHashCode()
            {
                int hashCode = -1128337416;
                hashCode = hashCode * -1521134295 + Opcode.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<ADDRESS_SIZE>.Default.GetHashCode(Arg_1);
                hashCode = hashCode * -1521134295 + EqualityComparer<ADDRESS_SIZE>.Default.GetHashCode(Arg_2);
                return hashCode;
            }

            public override string ToString()
            {
                return $"Heap Instr {Opcode} {Arg_1} {Arg_2}";
            }
        }
    }
}
