using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.FunctionIdentities
    {
        /// <summary>
        /// Returns A if C < 0, B if C > 0, (A + B) / 2 if C = 0
        /// </summary>
        internal static class SelectIdentity
        {
            private static readonly Expression _indicator = 0.5 * (Expression.SignOf(Expression.VarC) + 1);
            private static readonly Expression _atomicExpression = _indicator * Expression.VarB + (1 - _indicator) * Expression.VarA;
            private static readonly int _hashSeed = 739870216;
            private static readonly string _name = "select";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression, new List<string> { "a", "b", "c" });
        }
    }
}
