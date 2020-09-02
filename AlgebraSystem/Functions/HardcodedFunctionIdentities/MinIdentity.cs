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
            private static readonly Expression _atomicExpression = 0.5 * (Expression.VarA + Expression.VarB - Expression.Abs(Expression.VarA - Expression.VarB));
            private static readonly int _hashSeed = 1435958181;
            private static readonly string _name = "min";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
