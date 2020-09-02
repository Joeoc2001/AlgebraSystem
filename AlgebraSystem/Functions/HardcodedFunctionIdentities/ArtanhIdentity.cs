using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class ArtanhIdentity
        {
            private static readonly Expression _atomicExpression = Expression.LnOf((Expression.VarA + 1) / (Expression.VarA - 1)) / 2;
            private static readonly int _hashSeed = 1823411889;
            private static readonly string _name = "artanh";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
