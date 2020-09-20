using Algebra;
using Algebra.Atoms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AtomTests.AdditionTests
{
    class AdditionEvaluation
    {
        [Test]
        public void XPlusY([Range(-1, 1)] int a, [Range(-1, 1)] int b)
        {
            // ARANGE
            Expression expression = Expression.VarX + Expression.VarY;

            // ACT
            double value = expression.EvaluateOnce(new Vector2(a, b));

            // ASSERT
            Assert.AreEqual(a + b, value);
        }

        [Test]
        public void XPlus1([Range(-1, 1)] int a)
        {
            // ARANGE
            Expression expression = Expression.VarX + 1;

            // ACT
            double value = expression.EvaluateOnce(a);

            // ASSERT
            Assert.AreEqual(a + 1, value);
        }

        [Test]
        public void XPlus100([Range(-1, 1)] int a)
        {
            // ARANGE
            Expression expression = Expression.VarX + 100;

            // ACT
            double value = expression.EvaluateOnce(a);

            // ASSERT
            Assert.AreEqual(a + 100, value);
        }

        [Test]
        public void Empty()
        {
            // ARANGE
            Expression expression = Expression.Add(new List<Expression>());

            // ACT
            double value = expression.EvaluateOnce();

            // ASSERT
            Assert.AreEqual(0, value);
        }
    }
}
