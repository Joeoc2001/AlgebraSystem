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
            public bool IsHoldingDouble { get; }
            private DataUnion _data;
            public DataUnion Data { get => _data; }
            public int Dest { get; }

            public DefaultHeapInstruction(DefaultOpcode opcode, int arg_1, int dest)
            {
                Opcode = opcode;
                _data = new DataUnion
                {
                    Arg_1 = arg_1,
                    Arg_2 = 0
                };
                IsHoldingDouble = false;
                Dest = dest;
            }

            public DefaultHeapInstruction(DefaultOpcode opcode, int arg_1, int arg_2, int dest)
            {
                Opcode = opcode;
                _data = new DataUnion
                {
                    Arg_1 = arg_1,
                    Arg_2 = arg_2,
                };
                IsHoldingDouble = false;
                Dest = dest;
            }

            public DefaultHeapInstruction(DefaultOpcode opcode, double value, int dest)
            {
                Opcode = opcode;
                _data = new DataUnion
                {
                    Value = value
                };
                IsHoldingDouble = true;
                Dest = dest;
            }

            public void IndirectByTable(int[] indirectionTable)
            {
                if (IsHoldingDouble)
                {
                    return;
                }
                _data.Arg_1 = indirectionTable[Data.Arg_1];
                _data.Arg_2 = indirectionTable[Data.Arg_2];
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
