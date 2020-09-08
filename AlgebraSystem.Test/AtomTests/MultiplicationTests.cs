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
    public class MultiplicationTests
    {
        [Test]
        public void Multiplication_IsEqual_WhenSame()
        {
            // ARANGE
            Expression v1 = Expression.VarX * 2;
            Expression v2 = Expression.VarX * 2;

            // ACT

            // ASSERT
            Assert.IsTrue(v1.Equals(v2));
            Assert.IsTrue(v2.Equals(v1));
            Assert.IsTrue(v1.Equals((object)v2));
            Assert.IsTrue(v2.Equals((object)v1));
            Assert.IsTrue(v1 == v2);
            Assert.IsTrue(v2 == v1);
            Assert.IsFalse(v1 != v2);
            Assert.IsFalse(v2 != v1);
        }

        [Test]
        public void Multiplication_IsEqual_Commutative()
        {
            // ARANGE
            Expression v1 = Expression.VarX * 7;
            Expression v2 = 7 * Expression.VarX;

            // ACT

            // ASSERT
            Assert.IsTrue(v1.Equals(v2));
            Assert.IsTrue(v2.Equals(v1));
            Assert.IsTrue(v1.Equals((object)v2));
            Assert.IsTrue(v2.Equals((object)v1));
            Assert.IsTrue(v1 == v2);
            Assert.IsTrue(v2 == v1);
            Assert.IsFalse(v1 != v2);
            Assert.IsFalse(v2 != v1);
        }

        [Test]
        public void Multiplication_EqualReturnFalse_WhenDifferent()
        {
            // ARANGE
            Expression v1 = Expression.VarX * 1;
            Expression v2 = Expression.VarX * 2;

            // ACT

            // ASSERT
            Assert.IsFalse(v1.Equals(v2));
            Assert.IsFalse(v2.Equals(v1));
            Assert.IsFalse(v1.Equals((object)v2));
            Assert.IsFalse(v2.Equals((object)v1));
            Assert.IsFalse(v1 == v2);
            Assert.IsFalse(v2 == v1);
            Assert.IsTrue(v1 != v2);
            Assert.IsTrue(v2 != v1);
        }

        [Test]
        public void Multiplication_Derivative_IsTermsSum()
        {
            // ARANGE
            Expression value = Expression.VarX * Expression.VarY;
            Expression expected = 1 * Expression.VarY + Expression.VarX * 0;

            // ACT
            Expression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test, Pairwise]
        public void Multiplication_EvaluatesCorrectly([Range(-10, 10)] int a, [Range(-10, 10)] int b)
        {
            // ARANGE
            Expression expression = Expression.ConstantFrom(a) * Expression.ConstantFrom(b);

            // ACT
            float value = expression.EvaluateOnce(new VariableInputSet<float>());

            // ASSERT
            Assert.AreEqual(a * b, value);
        }

        [Test]
        public void Multiplication_Simplify_CollectsConstants()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(54321) * Expression.VarZ * Expression.ConstantFrom(54321);
            Expression expected = Expression.ConstantFrom(((Rational)54321) * 54321) * Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_CollectsPowers()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Pow(Expression.VarZ, 2) * Expression.Pow(Expression.VarZ, Expression.VarY) * Expression.VarZ;
            Expression expected = Expression.Pow(Expression.VarZ, 3 + Expression.VarY);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_RemovesOnes()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(1) * Expression.ConstantFrom(2) * Expression.VarZ * Expression.ConstantFrom(0.5);
            Expression expected = Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_CancelsVaraibles()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.VarZ * Expression.Pow(Expression.VarZ, -1);
            Expression expected = Expression.ConstantFrom(1);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_CancelsVaraiblesTimesTwo()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(2) * Expression.VarZ * Expression.Pow(Expression.VarZ, -1);
            Expression expected = Expression.ConstantFrom(2);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_CollatesMultiplication()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Multiply(new List<Expression>() { Expression.Multiply(new List<Expression>() { Expression.VarX, Expression.VarY }), Expression.VarZ });
            Expression expected = Expression.Multiply(new List<Expression>() { Expression.VarX, Expression.VarY, Expression.VarZ });

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_GoesToConstantZero()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.ConstantFrom(0) * Expression.ConstantFrom(2) * Expression.VarZ * Expression.ConstantFrom(0.5);
            Expression expected = 0;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_DoesNotSimplify_DOTS()
        {
            // ARANGE

            // ACT
            Expression expression = (Expression.VarX + 1) * (Expression.VarX - 1);
            Expression expected = Expression.Pow(Expression.VarX, 2) - 1;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_DoesNotExpandBraces()
        {
            // ARANGE

            // ACT
            Expression expression = (Expression.VarX + 1) * (Expression.VarX + 2);
            Expression expected = Expression.Pow(Expression.VarX, 2) + 3 * Expression.VarX + 2;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_DoesNotDistribute()
        {
            // ARANGE

            // ACT
            Expression expression = 3 * (Expression.VarX + 1) - 3;
            Expression expected = 3 * Expression.VarX;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Multiplication_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression expression = 3 * Expression.VarX;

            // ASSERT
            Assert.AreEqual(20, expression.GetOrderIndex());
        }
        [Test]
        public void Multiplication_Simplify_DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 * 2;

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Multiplication_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 * 2;

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Multiplication_Simplify_DoesntUseEvaluate()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 * 2;

            // ASSERT
            Assert.IsFalse(dummy1.EvaluateCalled);
        }
    }
}
