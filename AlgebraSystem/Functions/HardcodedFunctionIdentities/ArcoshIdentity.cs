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
            private static readonly IExpression atomicExpression = Expression.LnOf(Expression.VarA + Expression.Sqrt(Expression.VarA * Expression.VarA - 1));
            private static readonly int hashSeed = 615814358;
            private static readonly string name = "arcosh";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
