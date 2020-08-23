﻿using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions.HardcodedFunctionIdentities
    {
        internal static class TanhIdentity
        {
            private static readonly Variable param = new Variable("a");
            private static readonly IExpression atomicExpression = Expression.SinhOf(param) / Expression.CoshOf(param);
            private static readonly int hashSeed = -1881126278;
            private static readonly string name = "tanh";

            public static readonly MonadIdentity Instance = new MonadIdentity(name, param, hashSeed, atomicExpression);
        }
    }
}
