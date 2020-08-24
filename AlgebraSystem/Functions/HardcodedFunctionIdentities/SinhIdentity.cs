using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class SinhIdentity
        {
            private static readonly IExpression atomicExpression = 0.5 * (Expression.Pow(Expression.E, Expression.VarA) - Expression.Pow(Expression.E, -Expression.VarA));
            private static readonly int hashSeed = 1411437579;
            private static readonly string name = "sinh";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
