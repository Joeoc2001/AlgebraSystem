using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class CosIdentity
        {
            private static readonly Variable param = new Variable("a");
            private static readonly Expression atomicExpression = Expression.SinOf(param + Expression.PI / 2);
            private static readonly int hashSeed = 794723056;
            private static readonly string name = "cos";

            public static readonly MonadIdentity Instance = new MonadIdentity(name, param, hashSeed, atomicExpression);
        }
    }
}
