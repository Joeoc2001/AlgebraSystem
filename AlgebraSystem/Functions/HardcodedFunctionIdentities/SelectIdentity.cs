using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class SelectIdentity
        {
            private static readonly Variable param1 = new Variable("lt");
            private static readonly Variable param2 = new Variable("gt");
            private static readonly Variable param3 = new Variable("condition");
            private static readonly Expression atomicExpression = 0.5 * (param1 + param2 + (param2 - param1) * Expression.SignOf(param3));
            private static readonly int hashSeed = 739870216;
            private static readonly string name = "select";

            public static readonly TryadIdentity Instance = new TryadIdentity(name, param1, param2, param3, hashSeed, atomicExpression);
        }
    }
}
