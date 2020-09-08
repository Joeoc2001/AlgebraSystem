using Algebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.AdditionTests
{
    static class InputValues
    {
        public static Expression[] expressions = new Expression[]
        {
            Expression.VarX + 1,
            Expression.VarX + Expression.VarY,
            (3 * Expression.VarZ) + 100,
            Expression.LnOf(Expression.VarX) + Expression.VarA,
            1 - Expression.Pow(Expression.SinOf(200), 2),
        };
    }
}
