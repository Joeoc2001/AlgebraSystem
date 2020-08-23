using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class AbsIdentity
        {
            private static readonly Variable param = new Variable("a");
            private static readonly Expression atomicExpression = param * Expression.SignOf(param);
            private static readonly int hashSeed = -2124003897;
            private static readonly string name = "abs";

            public static readonly MonadIdentity Instance = new MonadIdentity(name, param, hashSeed, atomicExpression);
        }
    }
}
