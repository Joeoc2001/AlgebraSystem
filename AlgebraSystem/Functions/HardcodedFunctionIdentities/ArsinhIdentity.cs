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
            private static readonly IExpression atomicExpression = Expression.LnOf(Expression.VarA + Expression.Sqrt(Expression.VarA * Expression.VarA + 1));
            private static readonly int hashSeed = -1795032240;
            private static readonly string name = "arsinh";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
