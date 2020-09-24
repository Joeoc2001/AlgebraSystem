using Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtomTests.AdditionTests
{
    internal static class AdditionInputValues
    {
        public static List<Expression> atomicExpressions = new List<Expression>()
        {
            Expression.VarX + 1,
            Expression.VarX + Expression.PI,
            Expression.VarX + Expression.VarY,
            Expression.VarX + Expression.VarY + Expression.E,
            (3 * Expression.VarZ) + 100,
            Expression.LnOf(Expression.VarX) + Expression.VarA,
            1 - Expression.Pow(Expression.SinOf(200), 2),
        };

        public static List<Expression> nonAtomicExpressions = new List<Expression>()
        {
            Expression.LogOf(Expression.VarX, 10) + 10,
            Expression.Max(Expression.VarA, Expression.PI) - Expression.Min(Expression.VarA, Expression.PI),
        };

        public static List<Expression> boundaryExpressions = new List<Expression>()
        {
            Expression.Add(new List<Expression>()) // Empty sum
        };

        public static List<Expression> expressions = new List<Expression>(atomicExpressions.Union(nonAtomicExpressions));

        public static List<Expression> allExpressions = new List<Expression>(expressions.Union(boundaryExpressions));
    }
}
