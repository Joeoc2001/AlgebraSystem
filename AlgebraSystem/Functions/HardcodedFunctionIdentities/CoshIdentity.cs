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
            private static readonly IExpression _atomicExpression = 0.5 * (Expression.Pow(Expression.E, Expression.VarA) + Expression.Pow(Expression.E, -Expression.VarA));
            private static readonly int _hashSeed = -1733178947;
            private static readonly string _name = "cosh";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
