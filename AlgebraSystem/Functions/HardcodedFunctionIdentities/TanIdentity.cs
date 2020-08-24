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
            private static readonly IExpression atomicExpression = Expression.SinOf(Expression.VarA) / Expression.CosOf(Expression.VarA);
            private static readonly int hashSeed = 220126551;
            private static readonly string name = "tan";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
