using Algebra;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.ConstantTests
{
    class Atomicity
    {
        [Test]
        public void IsAtomic([ValueSource(typeof(InputValues), nameof(InputValues.expressions))] Expression expression)
        {
            // ARANGE

            // ACT
            bool isAtomic = expression.IsAtomic();

            // ASSERT
            Assert.IsTrue(isAtomic);
        }

        [Test]
        public void GetAtomic_ReferenceEquals([ValueSource(typeof(InputValues), nameof(InputValues.expressions))] Expression expression)
        {
            // ARANGE

            // ACT
            Expression atomicExpression = expression.GetAtomicExpression();

            // ASSERT
            Assert.IsTrue(ReferenceEquals(expression, atomicExpression));
        }
    }
}
