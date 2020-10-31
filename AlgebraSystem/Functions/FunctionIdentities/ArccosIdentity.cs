using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.FunctionIdentities
    {
        internal static class ArccosIdentity
        {
            private static readonly Expression _atomicExpression = (Expression.PI / 2) - Expression.ArcsinOf(Expression.VarA);
            private static readonly int _hashSeed = -1731053180;
            private static readonly string _name = "arccos";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression, new List<string> { "a" });
        }
    }
}
