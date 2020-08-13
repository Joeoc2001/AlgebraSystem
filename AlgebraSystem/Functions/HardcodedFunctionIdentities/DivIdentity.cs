using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Functions.HardcodedFunctionIdentities
{
    public static class DivIdentity
    {
        private static readonly Variable param1 = new Variable("a");
        private static readonly Variable param2 = new Variable("b");
        private static readonly Expression atomicExpression = param1 * Expression.Pow(param2, -1);
        private static readonly int hashSeed = -1042411579;
        private static readonly string name = "div";

        public static readonly DyadIdentity Instance = new DyadIdentity(name, param1, param2, hashSeed, atomicExpression, GetDelegate, GetDerivative);

        private static Expression GetDerivative(Expression parameter1, Expression parameter2, Variable wrt)
        {
            Expression dir1 = parameter1.GetDerivative(wrt);
            Expression dir2 = parameter2.GetDerivative(wrt);

            // Quotient Rule
            return ((parameter2 * dir1) - (parameter1 * dir2)) / Expression.Pow(parameter2, 2);
        }

        private static Expression.ExpressionDelegate GetDelegate(Expression.ExpressionDelegate parameter1, Expression.ExpressionDelegate parameter2)
        {
            return () => parameter1() / parameter2();
        }
    }
}
