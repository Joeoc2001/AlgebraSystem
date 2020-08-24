using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class LogIdentity
        {
            private static readonly IExpression atomicExpression = Expression.LnOf(Expression.VarA) / Expression.LnOf(Expression.VarB);
            private static readonly int hashSeed = 1441075845;
            private static readonly string name = "log";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(name, hashSeed, atomicExpression);
        }
    }
}
