using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Functions.HardcodedFunctionIdentities
{
    public static class MinIdentity
    {
        private static readonly Variable param1 = new Variable("a");
        private static readonly Variable param2 = new Variable("b");
        private static readonly Expression atomicExpression = 0.5 * (param1 + param2 - Expression.Abs(param1 - param2));
        private static readonly int hashSeed = 1435958181;

        public static readonly DyadIdentity Instance = new DyadIdentity(param1, param2, hashSeed, atomicExpression, GetDelegate, GetDerivative);

        private static Expression GetDerivative(Expression parameter1, Expression parameter2, Variable wrt)
        {
            Expression dir1 = parameter1.GetDerivative(wrt);
            Expression dir2 = parameter2.GetDerivative(wrt);
            Expression condition = param1 - param2;

            return Expression.SelectOn(dir1, dir2, condition);
        }

        private static Expression.ExpressionDelegate GetDelegate(Expression.ExpressionDelegate parameter1, Expression.ExpressionDelegate parameter2)
        {
            return () => Math.Min(parameter1(), parameter2());
        }
    }
}
