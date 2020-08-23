using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class MinIdentity
        {
            private static readonly Variable param1 = new Variable("a");
            private static readonly Variable param2 = new Variable("b");
            private static readonly Expression atomicExpression = 0.5 * (param1 + param2 - Expression.Abs(param1 - param2));
            private static readonly int hashSeed = 1435958181;
            private static readonly string name = "min";

            public static readonly DyadIdentity Instance = new DyadIdentity(name, param1, param2, hashSeed, atomicExpression);
        }
    }
}
