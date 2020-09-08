using Algebra;
using Algebra.Atoms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.ConstantTests
{
    class Derivative
    {
        [Test]
        public void Is0_WRT([Values("x", "y", "z")] string varName, [ValueSource(typeof(InputValues), nameof(InputValues.expressions))] Expression expression)
        {
            // ARANGE
            Expression expected = Expression.ConstantFrom(0);

            // ACT
            Expression derivative = expression.GetDerivative(varName);

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }
    }
}
