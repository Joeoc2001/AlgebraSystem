using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class MaxIdentity
        {
            private static readonly Variable param1 = new Variable("a");
            private static readonly Variable param2 = new Variable("b");
            private static readonly IExpression atomicExpression = 0.5 * (param1 + param2 + Expression.Abs(param1 - param2));
            private static readonly int hashSeed = -1121236537;
            private static readonly string name = "max";

            public static readonly DyadIdentity Instance = new DyadIdentity(name, param1, param2, hashSeed, atomicExpression);
        }
    }
}
