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
            private static readonly IExpression atomicExpression = Expression.SinOf(Expression.VarA + Expression.PI / 2);
            private static readonly int hashSeed = 794723056;
            private static readonly string name = "cos";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
