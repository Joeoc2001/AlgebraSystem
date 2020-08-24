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
            private static readonly IExpression atomicExpression = Expression.LnOf((Expression.VarA + 1) / (Expression.VarA - 1)) / 2;
            private static readonly int hashSeed = 1823411889;
            private static readonly string name = "artanh";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
