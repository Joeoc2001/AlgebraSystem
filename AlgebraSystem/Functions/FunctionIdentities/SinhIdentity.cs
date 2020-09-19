using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.FunctionIdentities
    {
        internal static class SinhIdentity
        {
            private static readonly Expression _atomicExpression = 0.5 * (Expression.Pow(Expression.E, Expression.VarA) - Expression.Pow(Expression.E, -Expression.VarA));
            private static readonly int _hashSeed = 1411437579;
            private static readonly string _name = "sinh";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
