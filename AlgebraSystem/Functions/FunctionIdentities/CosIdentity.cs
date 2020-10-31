using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.FunctionIdentities
    {
        internal static class CosIdentity
        {
            private static readonly Expression _atomicExpression = Expression.SinOf(Expression.VarA + Expression.PI / 2);
            private static readonly int _hashSeed = 794723056;
            private static readonly string _name = "cos";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression, new List<string> { "a" });
        }
    }
}
