using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{

    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class DivIdentity
        {
            private static readonly IExpression atomicExpression = Expression.VarA * Expression.Pow(Expression.VarB, Expression.MinusOne);
            private static readonly int hashSeed = -1042411579;
            private static readonly string name = "div";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}