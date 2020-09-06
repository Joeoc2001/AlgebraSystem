﻿using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class MaxIdentity
        {
            private static readonly Expression _atomicExpression = 0.5 * (Expression.VarA + Expression.VarB + Expression.AbsOf(Expression.VarA - Expression.VarB));
            private static readonly int _hashSeed = -1121236537;
            private static readonly string _name = "max";

            public static readonly FunctionIdentity Instance = new FunctionIdentity(_name, _hashSeed, _atomicExpression);
        }
    }
}
