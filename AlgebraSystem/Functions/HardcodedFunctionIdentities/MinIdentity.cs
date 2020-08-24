using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class MinIdentity
        {
            private static readonly IExpression atomicExpression = 0.5 * (Expression.VarA + Expression.VarB - Expression.Abs(Expression.VarA - Expression.VarB));
            private static readonly int hashSeed = 1435958181;
            private static readonly string name = "min";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
