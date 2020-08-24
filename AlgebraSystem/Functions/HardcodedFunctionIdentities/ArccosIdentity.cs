using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class ArccosIdentity
        {
            private static readonly IExpression atomicExpression = (Expression.PI / 2) - Expression.ArcsinOf(Expression.VarA);
            private static readonly int hashSeed = -1731053180;
            private static readonly string name = "arccos";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
