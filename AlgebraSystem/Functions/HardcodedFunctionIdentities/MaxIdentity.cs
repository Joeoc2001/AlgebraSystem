using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class MaxIdentity
        {
            private static readonly IExpression atomicExpression = 0.5 * (Expression.VarA + Expression.VarB + Expression.Abs(Expression.VarA - Expression.VarB));
            private static readonly int hashSeed = -1121236537;
            private static readonly string name = "max";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
