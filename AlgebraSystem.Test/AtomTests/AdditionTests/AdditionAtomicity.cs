using Algebra;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.AdditionTests
{
    class AdditionAtomicity
    {
        [Test]
        public void IfIsAtomic_GetAtomic_ReferenceEquals([ValueSource(typeof(AdditionInputValues), nameof(AdditionInputValues.atomicExpressions))] Expression expression)
        {
            // ARANGE

            // ACT
            Expression atomicExpression = expression.GetAtomicExpression();

            // ASSERT
            Assert.IsTrue(ReferenceEquals(expression, atomicExpression));
        }

        [Test]
        public void IfIsAtomic_IsAtomicIsTrue([ValueSource(typeof(AdditionInputValues), nameof(AdditionInputValues.atomicExpressions))] Expression expression)
        {
            // ARANGE

            // ACT
            bool isAtomic = expression.IsAtomic();

            // ASSERT
            Assert.IsTrue(isAtomic);
        }

        [Test]
        public void IfIsNotAtomic_GetAtomic_ReferenceEquals([ValueSource(typeof(AdditionInputValues), nameof(AdditionInputValues.nonAtomicExpressions))] Expression expression)
        {
            // ARANGE

            // ACT
            Expression atomicExpression = expression.GetAtomicExpression();

            // ASSERT
            Assert.IsFalse(expression.Equals(atomicExpression));
        }

        [Test]
        public void IfIsNotAtomic_IsAtomicIsFalse([ValueSource(typeof(AdditionInputValues), nameof(AdditionInputValues.nonAtomicExpressions))] Expression expression)
        {
            // ARANGE

            // ACT
            bool isAtomic = expression.IsAtomic();

            // ASSERT
            Assert.IsFalse(isAtomic);
        }
    }
}
