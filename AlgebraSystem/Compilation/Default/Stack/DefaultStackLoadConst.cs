using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Stack
    {
        internal class DefaultStackLoadConst : IDefaultStackInstruction
        {
            public DefaultOpcode Opcode { get => DefaultOpcode.CONSTANT; }

            public double Value { get; }

            public DefaultStackLoadConst(double value)
            {
                Value = value;
            }

            public override bool Equals(object obj)
            {
                return obj is DefaultStackLoadConst @const &&
                       Value == @const.Value;
            }

            public override int GetHashCode()
            {
                return -1937169414 + Value.GetHashCode();
            }

            public override string ToString()
            {
                return $"Load Const {Value}";
            }
        }
    }
}
