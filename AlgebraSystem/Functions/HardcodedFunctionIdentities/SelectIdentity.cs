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
            private static readonly Expression _atomicExpression = 0.5 * (Expression.VarA + Expression.VarB + (Expression.VarB - Expression.VarA) * Expression.SignOf(Expression.VarC));
            private static readonly int _hashSeed = 739870216;
            private static readonly string _name = "select";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
