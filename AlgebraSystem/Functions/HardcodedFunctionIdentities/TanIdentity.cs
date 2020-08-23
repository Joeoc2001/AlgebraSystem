using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class TanIdentity
        {
            private static readonly Variable param = new Variable("a");
            private static readonly IExpression atomicExpression = Expression.SinOf(param) / Expression.CosOf(param);
            private static readonly int hashSeed = 220126551;
            private static readonly string name = "tan";

            public static readonly MonadIdentity Instance = new MonadIdentity(name, param, hashSeed, atomicExpression);
        }
    }
}
