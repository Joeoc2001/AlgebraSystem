using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Functions.HardcodedFunctionIdentities
{
    public static class AbsIdentity
    {
        private static readonly Variable param = new Variable("a");
        private static readonly Expression atomicExpression = param * Expression.SignOf(param);
        private static readonly int hashSeed = -2124003897;

        public static readonly MonadIdentity Instance = new MonadIdentity(param, hashSeed, atomicExpression, GetDelegate, GetDerivative);

        private static Expression GetDerivative(Expression parameter, Variable wrt)
        {
            Expression dir = parameter.GetDerivative(wrt);

            return dir * Expression.SignOf(parameter);
        }

        private static Expression.ExpressionDelegate GetDelegate(Expression.ExpressionDelegate parameter1)
        {
            return () => Math.Abs(parameter1());
        }
    }
}
