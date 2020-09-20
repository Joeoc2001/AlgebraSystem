using Algebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.ConstantTests
{
    static class InputValues
    {
        public static Expression[] expressions = new Expression[]
        {
            0,
            1,
            -1,
            0.5f,
            new Rationals.Rational(101, 200),
            int.MaxValue,
            int.MinValue,
            new Rationals.Rational(int.MinValue, int.MaxValue),
            Expression.PI,
            Expression.E
        };
    }
}
