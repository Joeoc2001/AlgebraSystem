using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{

    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class DivIdentity
        {
            private static readonly Variable param1 = new Variable("a");
            private static readonly Variable param2 = new Variable("b");
            private static readonly IExpression atomicExpression = param1 * Expression.Pow(param2, Expression.MinusOne);
            private static readonly int hashSeed = -1042411579;
            private static readonly string name = "div";

            public static readonly DyadIdentity Instance = new DyadIdentity(name, param1, param2, hashSeed, atomicExpression);
        }
    }
}