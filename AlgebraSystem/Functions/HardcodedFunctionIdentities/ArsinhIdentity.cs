using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class ArsinhIdentity
        {
            private static readonly Variable param = new Variable("a");
            private static readonly IExpression atomicExpression = Expression.LnOf(param + Expression.Sqrt(param * param + 1));
            private static readonly int hashSeed = -1795032240;
            private static readonly string name = "arsinh";

            public static readonly MonadIdentity Instance = new MonadIdentity(name, param, hashSeed, atomicExpression);
        }
    }
}
