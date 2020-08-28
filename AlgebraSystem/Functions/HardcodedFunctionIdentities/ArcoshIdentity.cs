using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class ArcoshIdentity
        {
            private static readonly IExpression _atomicExpression = Expression.LnOf(Expression.VarA + Expression.Sqrt(Expression.VarA * Expression.VarA - 1));
            private static readonly int _hashSeed = 615814358;
            private static readonly string _name = "arcosh";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
