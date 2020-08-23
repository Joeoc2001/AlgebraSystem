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
    public class SignTests
    {
        [Test]
        public void Sign_Simplifies_WhenConstantParameter()
        {
            // ARANGE
            IExpression c = IExpression.ConstantFrom(1);

            // ACT
            IExpression e = IExpression.SignOf(10);

            // ASSERT
            Assert.AreEqual(c, e);
        }

        [Test]
        public void Sign_ReturnsZero_WhenZero()
        {
            // ARANGE
            IExpression c = IExpression.ConstantFrom(0);

            // ACT
            IExpression e = IExpression.SignOf(0);

            // ASSERT
            Assert.AreEqual(c, e);
        }

        [Test]
        public void Sign_EvaluatesToZero_WhenZero()
        {
            // ARANGE

            // ACT
            float e = IExpression.SignOf(IExpression.X).EvaluateOnce(0);

            // ASSERT
            Assert.AreEqual(0, e);
        }

        [Test]
        public void Sign_EvaluatesToOne_WhenPositive()
        {
            // ARANGE

            // ACT
            float e = IExpression.SignOf(IExpression.X).EvaluateOnce(145);

            // ASSERT
            Assert.AreEqual(1, e);
        }

        [Test]
        public void Sign_EvaluatesToMinusOne_WhenNegative()
        {
            // ARANGE

            // ACT
            float e = IExpression.SignOf(IExpression.X).EvaluateOnce(-14335);

            // ASSERT
            Assert.AreEqual(-1, e);
        }

        [Test]
        public void Addition_EvaluatesCorrectlyFor([Range(-1000, 1000, 10)] int a)
        {
            // ARANGE
            IExpression expression = IExpression.SignOf(a);

            // ACT
            float value = expression.EvaluateOnce(new VariableInputSet<float>());
            int expected = Math.Sign(a);

            // ASSERT
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void Sign_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.SignOf(IExpression.X);

            // ASSERT
            Assert.AreEqual(0, expression.GetOrderIndex());
        }

        [Test]
        public void Sign_GetHash_IsNotArgumentHash1()
        {
            // ARANGE

            // ACT
            IExpression argument = IExpression.Y;
            IExpression expression = IExpression.SignOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sign_GetHash_IsNotArgumentHash2()
        {
            // ARANGE

            // ACT
            IExpression argument = 2;
            IExpression expression = IExpression.SignOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sign_GetHash_IsNotArgumentHash3()
        {
            // ARANGE

            // ACT
            IExpression argument = IExpression.X + 1;
            IExpression expression = IExpression.SignOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sign_Simplify_DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = IExpression.SignOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Sign_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = IExpression.SignOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Sign_Simplify_DoesntUseMap()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = IExpression.SignOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.MapChildrenCalled);
        }
    }
}
