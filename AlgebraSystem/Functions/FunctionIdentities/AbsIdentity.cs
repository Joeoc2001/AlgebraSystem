using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.FunctionIdentities
    {
        internal static class AbsIdentity
        {
            private static readonly Expression _atomicExpression = Expression.VarA * Expression.SignOf(Expression.VarA);
            private static readonly int _hashSeed = -2124003897;
            private static readonly string _name = "abs";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression, new List<string> { "a" });
        }
    }
}
