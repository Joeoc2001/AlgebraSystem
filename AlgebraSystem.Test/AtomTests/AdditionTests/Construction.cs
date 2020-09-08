using Algebra;
using AlgebraTests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.AdditionTests
{
    class Construction
    {
        [Test]
        public void Addition_Construction_CollectsConstants([Range(-10, 10)] int a, [Range(-10, 10)] int b)
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(a) + Expression.VarZ + Expression.ConstantFrom(b);
            Expression expected = Expression.ConstantFrom(a + b) + Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Construction_CollectsCoefficients([Range(-10, 10)] int a, [Range(-10, 10)] int b)
        {
            // ARANGE

            // ACT
            Expression expression = (Expression.ConstantFrom(a) * Expression.VarZ) + (Expression.ConstantFrom(b) * Expression.VarZ);
            Expression expected = Expression.ConstantFrom(a + b) * Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Construction_RemovesZeros([Range(-10, 10)] int a, [Range(-10, 10)] int b)
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(0) + Expression.ConstantFrom(a) + Expression.ConstantFrom(b) + Expression.VarZ;
            Expression expected = Expression.ConstantFrom(a + b) + Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Construction_CancelsVaraibles([Range(-10, 10)] int a, [Range(-10, 10)] int b)
        {
            // ARANGE

            // ACT
            Expression expression = (b * Expression.VarZ) - (b * Expression.VarZ) + a;
            Expression expected = Expression.ConstantFrom(a);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Construction_RemovesVaraibleCoefficientIfOne([Range(-10, 10)] int a, [Range(-10, 10)] int b)
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(a) + (b * Expression.VarZ) - ((b - 1) * Expression.VarZ);
            Expression expected = Expression.ConstantFrom(a) + Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Construction_CollatesAdditions()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Add(new List<Expression>() { Expression.Add(new List<Expression>() { Expression.VarX, Expression.VarY }), Expression.VarZ });
            Expression expected = Expression.Add(new List<Expression>() { Expression.VarX, Expression.VarY, Expression.VarZ });

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Construction_DoesNotFactorise()
        {
            // ARANGE

            // ACT
            Expression expression = (Expression.Pow(Expression.VarX, 2) + (3 * Expression.VarX) + 2) / (Expression.VarX + 1);
            Expression expected = Expression.VarX + 2;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Addition_Construction_DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Addition_Construction_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Addition_Construction_DoesntUseEvaluate()
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
