using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.FunctionIdentities
    {
        internal static class LogIdentity
        {
            private static readonly Expression _atomicExpression = Expression.LnOf(Expression.VarA) / Expression.LnOf(Expression.VarB);
            private static readonly int _hashSeed = 1441075845;
            private static readonly string _name = "log";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
