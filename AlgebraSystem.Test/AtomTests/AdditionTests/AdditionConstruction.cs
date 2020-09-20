using Algebra;
using AlgebraTests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.AdditionTests
{
    class AdditionConstruction
    {
        [Test]
        public void CollectsConstants([Range(-1, 1)] int a, [Range(-1, 1)] int b)
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(a) + Expression.VarZ + Expression.ConstantFrom(b);
            Expression expected = Expression.ConstantFrom(a + b) + Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void CollectsCoefficients([Range(-1, 1)] int a, [Range(-1, 1)] int b)
        {
            // ARANGE

            // ACT
            Expression expression = (Expression.ConstantFrom(a) * Expression.VarZ) + (Expression.ConstantFrom(b) * Expression.VarZ);
            Expression expected = Expression.ConstantFrom(a + b) * Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void RemovesZeros([Range(-1, 1)] int a, [Range(-1, 1)] int b)
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(0) + Expression.ConstantFrom(a) + Expression.ConstantFrom(b) + Expression.VarZ;
            Expression expected = Expression.ConstantFrom(a + b) + Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void CancelsVaraibles([Range(-1, 1)] int a, [Range(-1, 1)] int b)
        {
            // ARANGE

            // ACT
            Expression expression = (b * Expression.VarZ) - (b * Expression.VarZ) + a;
            Expression expected = Expression.ConstantFrom(a);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void RemovesVaraibleCoefficientIfOne([Range(-1, 1)] int a, [Range(-1, 1)] int b)
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(a) + (b * Expression.VarZ) - ((b - 1) * Expression.VarZ);
            Expression expected = Expression.ConstantFrom(a) + Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void CollatesAdditions()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Add(new List<Expression>() { Expression.Add(new List<Expression>() { Expression.VarX, Expression.VarY }), Expression.VarZ });
            Expression expected = Expression.Add(new List<Expression>() { Expression.VarX, Expression.VarY, Expression.VarZ });

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void DoesNotFactorise()
        {
            // ARANGE

            // ACT
            Expression expression = (Expression.Pow(Expression.VarX, 2) + (3 * Expression.VarX) + 2) / (Expression.VarX + 1);
            Expression expected = Expression.VarX + 2;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void DoesntUseEvaluate()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.EvaluateCalled);
        }
    }
}
