using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class CoshIdentity
        {
            private static readonly Variable param = new Variable("a");
            private static readonly IExpression atomicExpression = 0.5 * (Expression.Pow(Expression.E, param) + Expression.Pow(Expression.E, -param));
            private static readonly int hashSeed = -1733178947;
            private static readonly string name = "cosh";

            public static readonly MonadIdentity Instance = new MonadIdentity(name, param, hashSeed, atomicExpression);
        }
    }
}
