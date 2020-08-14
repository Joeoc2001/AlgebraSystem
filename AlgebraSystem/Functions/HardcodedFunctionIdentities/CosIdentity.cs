using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Functions.HardcodedFunctionIdentities
{
    public static class CosIdentity
    {
        private static readonly Variable param = new Variable("a");
        private static readonly Expression atomicExpression = Expression.SinOf(param + Constant.PI / 2);
        private static readonly int hashSeed = 794723056;
        private static readonly string name = "cos";

        public static readonly MonadIdentity Instance = new MonadIdentity(name, param, hashSeed, atomicExpression, GetDelegate, GetDerivative);

        private static Expression GetDerivative(Expression parameter, Variable wrt)
        {
            Expression dir = parameter.GetDerivative(wrt);

            return - dir * Expression.SinOf(parameter);
        }

        private static Expression.ExpressionDelegate GetDelegate(Expression.ExpressionDelegate parameter)
        {
            return () => (float)Math.Cos(parameter());
        }
    }
}
