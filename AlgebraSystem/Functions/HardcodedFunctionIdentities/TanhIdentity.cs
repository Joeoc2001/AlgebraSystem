using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class TanhIdentity
        {
            private static readonly IExpression _atomicExpression = Expression.SinhOf(Expression.VarA) / Expression.CoshOf(Expression.VarA);
            private static readonly int _hashSeed = -1881126278;
            private static readonly string _name = "tanh";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
