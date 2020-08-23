using Algebra;
using Algebra.Atoms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AlgebraSystem.Test.AtomTests.AdditionTests
{
    class Evaluation
    {
        [Test]
        public void XPlusY([Range(-10, 10)] int a, [Range(-10, 10)] int b)
        {
            // ARANGE
            IExpression expression = IExpression.X + IExpression.Y;

            // ACT
            float value = expression.EvaluateOnce(new Vector2(a, b));

            // ASSERT
            Assert.AreEqual(a + b, value);
        }

        [Test]
        public void XPlus1([Range(-10, 10)] int a)
        {
            // ARANGE
            IExpression expression = IExpression.X + 1;

            // ACT
            float value = expression.EvaluateOnce(a);

            // ASSERT
            Assert.AreEqual(a + 1, value);
        }

        [Test]
        public void XPlus100([Range(-10, 10)] int a)
        {
            // ARANGE
            IExpression expression = IExpression.X + 100;

            // ACT
            float value = expression.EvaluateOnce(a);

            // ASSERT
            Assert.AreEqual(a + 100, value);
        }
    }
}
