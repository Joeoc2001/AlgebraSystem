using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default
    {
        internal class DefaultLoadConst : IDefaultInstruction
        {
            public DefaultOpcode Opcode { get => DefaultOpcode.CONSTANT; }

            public double Value { get; }

            public DefaultLoadConst(double value)
            {
                Value = value;
            }

            public override bool Equals(object obj)
            {
                return obj is DefaultLoadConst @const &&
                       Value == @const.Value;
            }

            public override int GetHashCode()
            {
                return -1937169414 + Value.GetHashCode();
            }
        }
    }
}
