using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Functions.HardcodedFunctionIdentities
{
    public static class LogIdentity
    {
        private static readonly Variable argParam = new Variable("arg");
        private static readonly Variable baseParam = new Variable("base");
        private static readonly Expression atomicExpression = Expression.LnOf(argParam) / Expression.LnOf(baseParam);
        private static readonly int hashSeed = 1441075845;
        private static readonly string name = "log";

        public static readonly DyadIdentity Instance = new DyadIdentity(name, argParam, baseParam, hashSeed, atomicExpression, GetDelegate, GetDerivative);

        private static Expression GetDerivative(Expression argExp, Expression baseExp, Variable wrt)
        {
            Expression argDir = argExp.GetDerivative(wrt);
            Expression baseDir = baseExp.GetDerivative(wrt);

            // d/dx(log(a(x), b(x))) = ((log(a(x)) b'(x))/b(x) - (a'(x) log(b(x)))/a(x))/(log^2(a(x)))
            return Expression.Pow(Expression.LnOf(baseExp), -2) * ((argDir / argExp) * Expression.LnOf(baseExp) - (baseDir / baseExp) * Expression.LnOf(argExp));
        }

        private static Expression.ExpressionDelegate GetDelegate(Expression.ExpressionDelegate argDelegate, Expression.ExpressionDelegate baseDelegate)
        {
            return () => (float)Math.Log(argDelegate(), baseDelegate());
        }
    }
}
