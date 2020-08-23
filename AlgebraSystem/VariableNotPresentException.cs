using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public class VariableNotPresentException : ArgumentException
    {
        public VariableNotPresentException(string message) : base(message)
        {
        }
    }
}
