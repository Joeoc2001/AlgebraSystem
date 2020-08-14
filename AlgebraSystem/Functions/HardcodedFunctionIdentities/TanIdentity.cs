using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Functions.HardcodedFunctionIdentities
{
    public static class TanIdentity
    {
        private static readonly Variable param = new Variable("a");
        private static readonly Expression atomicExpression = Expression.SinOf(param) / Expression.CosOf(param);
        private static readonly int hashSeed = 220126551;
        private static readonly string name = "tan";

        public static readonly MonadIdentity Instance = new MonadIdentity(name, param, hashSeed, atomicExpression, GetDelegate, GetDerivative);

        private static Expression GetDerivative(Expression parameter, Variable wrt)
        {
            Expression dir = parameter.GetDerivative(wrt);

            return dir * Expression.Pow(Expression.CosOf(parameter), -2);
        }

        private static Expression.ExpressionDelegate GetDelegate(Expression.ExpressionDelegate parameter)
        {
            return () => (float)Math.Tan(parameter());
        }
    }
}
