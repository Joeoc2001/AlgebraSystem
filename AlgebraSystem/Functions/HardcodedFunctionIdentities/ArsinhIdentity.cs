using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class ArsinhIdentity
        {
            private static readonly IExpression _atomicExpression = Expression.LnOf(Expression.VarA + Expression.Sqrt(Expression.VarA * Expression.VarA + 1));
            private static readonly int _hashSeed = -1795032240;
            private static readonly string _name = "arsinh";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
