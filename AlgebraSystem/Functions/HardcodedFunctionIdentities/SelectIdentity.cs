using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Algebra.Functions.HardcodedFunctionIdentities
{
    public static class SelectIdentity
    {
        private static readonly Variable param1 = new Variable("lt");
        private static readonly Variable param2 = new Variable("gt");
        private static readonly Variable param3 = new Variable("condition");
        private static readonly Expression atomicExpression = 0.5 * (param1 + param2 + (param2 - param1) * Expression.SignOf(param3));
        private static readonly int hashSeed = 739870216;
        private static readonly string name = "select";

        public static readonly TryadIdentity Instance = new TryadIdentity(name, param1, param2, param3, hashSeed, atomicExpression, GetDelegate, GetDerivative);

        private static Expression GetDerivative(Expression parameter1, Expression parameter2, Expression parameter3, Variable wrt)
        {
            Expression dir1 = parameter1.GetDerivative(wrt);
            Expression dir2 = parameter2.GetDerivative(wrt);

            return Instance.CreateExpression(dir1, dir2, parameter3);
        }

        private static Expression.ExpressionDelegate GetDelegate(Expression.ExpressionDelegate delLT, Expression.ExpressionDelegate delGT, Expression.ExpressionDelegate condition)
        {
            return () =>
            {
                float eval = condition();
                if (eval < 0)
                {
                    return delLT();
                }
                else if (eval > 0)
                {
                    return delGT();
                }
                else
                {
                    return (delLT() + delGT()) / 2;
                }
            };
        }
    }
}
