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
            private static readonly IExpression atomicExpression = Expression.VarA * Expression.SignOf(Expression.VarA);
            private static readonly int hashSeed = -2124003897;
            private static readonly string name = "abs";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
