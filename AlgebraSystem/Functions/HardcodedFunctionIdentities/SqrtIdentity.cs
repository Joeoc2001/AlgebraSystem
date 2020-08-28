using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class SqrtIdentity
        {
            private static readonly IExpression _atomicExpression = Expression.Pow(Expression.VarA, Expression.ConstantFrom(0.5));
            private static readonly int _hashSeed = -1139274514;
            private static readonly string _name = "sqrt";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
