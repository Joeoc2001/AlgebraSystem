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
            private static readonly Variable argParam = new Variable("arg");
            private static readonly Variable baseParam = new Variable("base");
            private static readonly Expression atomicExpression = Expression.LnOf(argParam) / Expression.LnOf(baseParam);
            private static readonly int hashSeed = 1441075845;
            private static readonly string name = "log";

            public static readonly DyadIdentity Instance = new DyadIdentity(name, argParam, baseParam, hashSeed, atomicExpression);
        }
    }
}
