using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Functions.HardcodedFunctionIdentities
{
    public static class TanhIdentity
    {
        private static readonly Variable param = new Variable("a");
        private static readonly Expression atomicExpression = Expression.SinhOf(param) / Expression.CoshOf(param);
        private static readonly int hashSeed = -1881126278;

        public static readonly MonadIdentity Instance = new MonadIdentity(param, hashSeed, atomicExpression, GetDelegate, GetDerivative);

        private static Expression GetDerivative(Expression parameter, Variable wrt)
        {
            Expression dir = parameter.GetDerivative(wrt);

            return dir * (1 - Expression.Pow(Expression.TanhOf(parameter), 2));
        }

        private static Expression.ExpressionDelegate GetDelegate(Expression.ExpressionDelegate parameter)
        {
            return () => (float)Math.Tanh(parameter());
        }
    }
}
