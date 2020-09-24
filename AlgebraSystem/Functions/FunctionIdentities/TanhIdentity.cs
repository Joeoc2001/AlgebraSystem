using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.FunctionIdentities
    {
        internal static class TanhIdentity
        {
            private static readonly Expression _atomicExpression = 1 - 2 / (Expression.Pow(Expression.E, (2 * Expression.VarA)) + 1);
            private static readonly int _hashSeed = -1881126278;
            private static readonly string _name = "tanh";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
