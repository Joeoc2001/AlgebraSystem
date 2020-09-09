using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public class VariableInput<T>
    {
        public T Value { get; set; }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ -754487501;
        }
    }
}
