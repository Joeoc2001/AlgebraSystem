﻿using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Functions.HardcodedFunctionIdentities
{
    public static class SinhIdentity
    {
        private static readonly Variable param = new Variable("a");
        private static readonly Expression atomicExpression = 0.5 * (Expression.Pow(Constant.E, param) - Expression.Pow(Constant.E, -param));
        private static readonly int hashSeed = 1411437579;
        private static readonly string name = "sinh";

        public static readonly MonadIdentity Instance = new MonadIdentity(name, param, hashSeed, atomicExpression, GetDelegate, GetDerivative);

        private static Expression GetDerivative(Expression parameter, Variable wrt)
        {
            Expression dir = parameter.GetDerivative(wrt);

            return dir * Expression.CoshOf(parameter);
        }

        private static Expression.ExpressionDelegate GetDelegate(Expression.ExpressionDelegate parameter)
        {
            return () => (float)Math.Sinh(parameter());
        }
    }
}
