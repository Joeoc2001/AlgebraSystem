using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;
using System;
using AlgebraSystem.Test;

namespace AtomTests
{
    public class SinTests
    {
        [Test]
        public void Sin_IsEqual_WhenSame()
        {
            // ARANGE
            IExpression v1 = IExpression.SinOf(IExpression.X);
            IExpression v2 = IExpression.SinOf(IExpression.X);

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
            IExpression v1 = IExpression.SinOf(IExpression.X);
            IExpression v2 = IExpression.SinOf(IExpression.Y);

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
            IExpression value = IExpression.SinOf(IExpression.X);
            IExpression expected = IExpression.CosOf(IExpression.X);

            // ACT
            IExpression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Sin_XSquaredDerivative_IsCorrect()
        {
            // ARANGE
            IExpression value = IExpression.SinOf(IExpression.Pow(IExpression.X, 2));
            IExpression expected = 2 * IExpression.X * IExpression.CosOf(IExpression.Pow(IExpression.X, 2));

            // ACT
            IExpression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Sin_EvaluatesCorrectly([Range(-100, 100)] int v)
        {
            // ARANGE
            IExpression expression = IExpression.SinOf(v);

            // ACT
            float value = expression.EvaluateOnce(new VariableInputSet<float>());
            double expected = Math.Sin(v);

            // ASSERT
            Assert.That(value, Is.EqualTo(expected).Within(0.00001f));
        }

        [Test]
        public void Sin_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.SinOf(IExpression.X);

            // ASSERT
            Assert.AreEqual(0, expression.GetOrderIndex());
        }

        [Test]
        public void Sin_GetHash_IsNotArgumentHash1()
        {
            // ARANGE

            // ACT
            IExpression argument = IExpression.Y;
            IExpression expression = IExpression.SinOf(argument);
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
            IExpression argument = 2;
            IExpression expression = IExpression.SinOf(argument);
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
            IExpression argument = IExpression.X + 1;
            IExpression expression = IExpression.SinOf(argument);
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
            IExpression argument = IExpression.X;
            IExpression ln = IExpression.SinOf(argument);
            IExpression sign = IExpression.SignOf(argument);
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
            IExpression _ = IExpression.SinOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Sin_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = IExpression.SinOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Sin_Simplify_DoesntUseMap()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = IExpression.SinOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.MapChildrenCalled);
        }
    }
}
