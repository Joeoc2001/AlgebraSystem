using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Heap
    {
        internal struct DefaultHeapInstruction
        {
            // Very cheeky bit fiddling for performance
            [StructLayout(LayoutKind.Explicit)]
            public struct DataUnion
            {
                [FieldOffset(0)]
                public int Arg_1;
                [FieldOffset(4)]
                public int Arg_2;
                [FieldOffset(0)]
                public double Value;
            }

            public DefaultOpcode Opcode { get; }
            public DataUnion Data { get; }
            public int Dest { get; }

            public DefaultHeapInstruction(DefaultOpcode opcode, int arg_1, int dest)
            {
                Opcode = opcode;
                Data = new DataUnion
                {
                    Arg_1 = arg_1,
                    Arg_2 = 0
                };
                Dest = dest;
            }

            public DefaultHeapInstruction(DefaultOpcode opcode, int arg_1, int arg_2, int dest)
            {
                Opcode = opcode;
                Data = new DataUnion
                {
                    Arg_1 = arg_1,
                    Arg_2 = arg_2,
                };
                Dest = dest;
            }

            public DefaultHeapInstruction(DefaultOpcode opcode, double value, int dest)
            {
                Opcode = opcode;
                Data = new DataUnion
                {
                    Value = value
                };
                Dest = dest;
            }

            public DefaultHeapInstruction IndirectByTable(int[] indirectionTable)
            {
                int newDest = indirectionTable[Dest];
                if (Opcode == DefaultOpcode.CONSTANT)
                {
                    return new DefaultHeapInstruction(Opcode, Data.Value, newDest);
                }
                if (Opcode == DefaultOpcode.VARIABLE)
                {
                    return new DefaultHeapInstruction(Opcode, Data.Arg_1, Data.Arg_2, newDest);
                }
                int arg_1 = indirectionTable[Data.Arg_1];
                int arg_2 = indirectionTable[Data.Arg_2];
                return new DefaultHeapInstruction(Opcode, arg_1, arg_2, newDest);
            }

            public override bool Equals(object obj)
            {
                return obj is DefaultHeapInstruction instruction &&
                       Opcode == instruction.Opcode &&
                       Data.Value == instruction.Data.Value;
            }

            public override int GetHashCode()
            {
                int hashCode = -1128337416;
                hashCode = hashCode * -1521134295 + Opcode.GetHashCode();
                hashCode = hashCode * -1521134295 + Data.Arg_1.GetHashCode();
                hashCode = hashCode * -1521134295 + Data.Arg_2.GetHashCode();
                return hashCode;
            }

            public override string ToString()
            {
                if (Opcode == DefaultOpcode.CONSTANT)
                {
                    return $"Heap Instr {Opcode} {Dest} {Data.Value}";
                }
                return $"Heap Instr {Opcode} {Dest} {Data.Arg_1} {Data.Arg_2}";
            }
        }
    }
}
