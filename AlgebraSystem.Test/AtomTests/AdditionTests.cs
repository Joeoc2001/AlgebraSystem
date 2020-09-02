using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;
using AlgebraSystem.Test;

namespace AtomTests
{
    public class AdditionTests
    {

        [Test]
        public void Addition_Simplify_CollectsConstants()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(54321) + Expression.VarZ + Expression.ConstantFrom(54321);
            Expression expected = Expression.ConstantFrom(54321 + 54321) + Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_CollectsCoefficients()
        {
            // ARANGE

            // ACT
            Expression expression = (Expression.ConstantFrom(54321) * Expression.VarZ) + (Expression.ConstantFrom(54321) * Expression.VarZ) + Expression.VarZ;
            Expression expected = Expression.ConstantFrom(54321 + 54321 + 1) * Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_RemovesZeros()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(0) + Expression.ConstantFrom(1) + Expression.ConstantFrom(-1) + Expression.VarZ;
            Expression expected = Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_CancelsVaraibles()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.VarZ - Expression.VarZ;
            Expression expected = Expression.ConstantFrom(0);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_CancelsVaraiblesPlusOne()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(1) + Expression.VarZ - Expression.VarZ;
            Expression expected = Expression.ConstantFrom(1);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_RemovesVaraibleCoefficientIfOne()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(1) + 2 * Expression.VarZ - Expression.VarZ;
            Expression expected = Expression.ConstantFrom(1) + Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_CollatesAdditions()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Add(new List<Expression>() { Expression.Add(new List<Expression>() { Expression.VarX, Expression.VarY }), Expression.VarZ });
            Expression expected = Expression.Add(new List<Expression>() { Expression.VarX, Expression.VarY, Expression.VarZ });

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_DoesNotFactorise()
        {
            // ARANGE

            // ACT
            Expression expression = (Expression.Pow(Expression.VarX, 2) + (3 * Expression.VarX) + 2) / (Expression.VarX + 1);
            Expression expected = Expression.VarX + 2;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Addition_GetOrderIndex_Is30()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.VarX + 1;

            // ASSERT
            Assert.AreEqual(30, expression.GetOrderIndex());
        }

        [Test]
        public void Addition_Simplify_DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Addition_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Addition_Simplify_DoesntUseEvaluate()
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
