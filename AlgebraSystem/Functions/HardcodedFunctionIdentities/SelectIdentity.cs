using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class SelectIdentity
        {
            private static readonly IExpression atomicExpression = 0.5 * (Expression.VarA + Expression.VarB + (Expression.VarB - Expression.VarA) * Expression.SignOf(Expression.VarC));
            private static readonly int hashSeed = 739870216;
            private static readonly string name = "select";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
