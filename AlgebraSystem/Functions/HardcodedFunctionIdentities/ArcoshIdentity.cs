using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class ArcoshIdentity
        {
            private static readonly Variable param = new Variable("a");
            private static readonly IExpression atomicExpression = Expression.LnOf(param + Expression.Sqrt(param * param - 1));
            private static readonly int hashSeed = 615814358;
            private static readonly string name = "arcosh";

            public static readonly MonadIdentity Instance = new MonadIdentity(name, param, hashSeed, atomicExpression);
        }
    }
}
