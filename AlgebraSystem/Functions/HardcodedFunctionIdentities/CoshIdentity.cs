using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class CoshIdentity
        {
            private static readonly IExpression atomicExpression = 0.5 * (Expression.Pow(Expression.E, Expression.VarA) + Expression.Pow(Expression.E, -Expression.VarA));
            private static readonly int hashSeed = -1733178947;
            private static readonly string name = "cosh";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
