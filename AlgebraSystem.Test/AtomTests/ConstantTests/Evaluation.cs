using Algebra;
using Algebra.Atoms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AtomTests.ConstantTests
{
    class Evaluation
    {
        [Test]
        public void Evaluates([Range(-100, 100, 10)] int a)
        {
            // ARANGE
            Expression expression = Expression.ConstantFrom(a);

            // ACT
            double value = expression.EvaluateOnce();

            // ASSERT
            Assert.AreEqual(a, value);
        }
    }
}
