using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Functions
{
    public abstract class FunctionFactory
    {
        public static readonly Dictionary<string, FunctionFactory> DefaultFunctions = new Dictionary<string, FunctionFactory>()
        {

        };

        public abstract Equation CreateEquation(IList<Equation> nodes);
    }
}
