using Algebra;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.AdditionTests
{
    class Atomicity
    {
        [Test]
        public void IfIsAtomic_GetAtomic_ReferenceEquals([ValueSource(typeof(InputValues), nameof(InputValues.expressions))] Expression expression)
        {
            // ARANGE

            // ACT
            bool isAtomic = expression.IsAtomic();
            Expression atomicExpression = expression.GetAtomicExpression();

            // ASSERT
            if (isAtomic)
            {
                Assert.IsTrue(ReferenceEquals(expression, atomicExpression));
            }
            else
            {
                Assert.IsFalse(expression.Equals(atomicExpression));
            }
        }
    }
}
