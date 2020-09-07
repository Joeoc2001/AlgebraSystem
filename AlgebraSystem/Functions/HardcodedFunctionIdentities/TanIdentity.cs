using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class TanIdentity
        {
            private static readonly Expression _atomicExpression = Expression.SinOf(Expression.VarA) / Expression.CosOf(Expression.VarA);
            private static readonly int _hashSeed = 220126551;
            private static readonly string _name = "tan";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
