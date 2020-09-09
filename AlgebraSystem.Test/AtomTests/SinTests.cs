using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;
using System;
using AlgebraTests;

namespace AtomTests
{
    public class SinTests
    {
        [Test]
        public void Sin_IsEqual_WhenSame()
        {
            // ARANGE
            Expression v1 = Expression.SinOf(Expression.VarX);
            Expression v2 = Expression.SinOf(Expression.VarX);

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
        public void Sin_EqualReturnFalse_WhenDifferent()
        {
            // ARANGE
            Expression v1 = Expression.SinOf(Expression.VarX);
            Expression v2 = Expression.SinOf(Expression.VarY);

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
        public void Sin_XDerivative_IsCorrect()
        {
            // ARANGE
            Expression value = Expression.SinOf(Expression.VarX);
            Expression expected = Expression.CosOf(Expression.VarX);

            // ACT
            Expression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Sin_XSquaredDerivative_IsCorrect()
        {
            // ARANGE
            Expression value = Expression.SinOf(Expression.Pow(Expression.VarX, 2));
            Expression expected = 2 * Expression.VarX * Expression.CosOf(Expression.Pow(Expression.VarX, 2));

            // ACT
            Expression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Sin_EvaluatesCorrectly([Range(-100, 100)] int v)
        {
            // ARANGE
            Expression expression = Expression.SinOf(v);

            // ACT
            double value = expression.EvaluateOnce();
            double expected = Math.Sin(v);

            // ASSERT
            Assert.That(value, Is.EqualTo(expected).Within(0.00001f));
        }

        [Test]
        public void Sin_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.SinOf(Expression.VarX);

            // ASSERT
            Assert.AreEqual(0, expression.GetOrderIndex());
        }

        [Test]
        public void Sin_GetHash_IsNotArgumentHash1()
        {
            // ARANGE

            // ACT
            Expression argument = Expression.VarY;
            Expression expression = Expression.SinOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sin_GetHash_IsNotArgumentHash2()
        {
            // ARANGE

            // ACT
            Expression argument = 2;
            Expression expression = Expression.SinOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sin_GetHash_IsNotArgumentHash3()
        {
            // ARANGE

            // ACT
            Expression argument = Expression.VarX + 1;
            Expression expression = Expression.SinOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sin_GetHash_IsNotSignHash()
        {
            // ARANGE

            // ACT
            Expression argument = Expression.VarX;
            Expression ln = Expression.SinOf(argument);
            Expression sign = Expression.SignOf(argument);
            int hash1 = ln.GetHashCode();
            int hash2 = sign.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sin_Simplify_DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = Expression.SinOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Sin_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = Expression.SinOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Sin_Simplify_DoesntUseEvaluate()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = Expression.SinOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.EvaluateCalled);
        }
    }
}
