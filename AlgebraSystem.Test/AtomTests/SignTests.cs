﻿using System.Collections;
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
    public class SignTests
    {
        [Test]
        public void Sign_Simplifies_WhenConstantParameter()
        {
            // ARANGE
            Expression c = Expression.ConstantFrom(1);

            // ACT
            Expression e = Expression.SignOf(10);

            // ASSERT
            Assert.AreEqual(c, e);
        }

        [Test]
        public void Sign_ReturnsZero_WhenZero()
        {
            // ARANGE
            Expression c = Expression.ConstantFrom(0);

            // ACT
            Expression e = Expression.SignOf(0);

            // ASSERT
            Assert.AreEqual(c, e);
        }

        [Test]
        public void Sign_EvaluatesToZero_WhenZero()
        {
            // ARANGE

            // ACT
            double e = Expression.SignOf(Expression.VarX).EvaluateOnce(0);

            // ASSERT
            Assert.AreEqual(0, e);
        }

        [Test]
        public void Sign_EvaluatesToOne_WhenPositive()
        {
            // ARANGE

            // ACT
            double e = Expression.SignOf(Expression.VarX).EvaluateOnce(145);

            // ASSERT
            Assert.AreEqual(1, e);
        }

        [Test]
        public void Sign_EvaluatesToMinusOne_WhenNegative()
        {
            // ARANGE

            // ACT
            double e = Expression.SignOf(Expression.VarX).EvaluateOnce(-14335);

            // ASSERT
            Assert.AreEqual(-1, e);
        }

        [Test]
        public void Addition_EvaluatesCorrectlyFor([Range(-1000, 1000, 10)] int a)
        {
            // ARANGE
            Expression expression = Expression.SignOf(a);

            // ACT
            double value = expression.EvaluateOnce();
            int expected = Math.Sign(a);

            // ASSERT
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void Sign_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.SignOf(Expression.VarX);

            // ASSERT
            Assert.AreEqual(0, expression.GetOrderIndex());
        }

        [Test]
        public void Sign_GetHash_IsNotArgumentHash1()
        {
            // ARANGE

            // ACT
            Expression argument = Expression.VarY;
            Expression expression = Expression.SignOf(argument);
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
            Expression argument = 2;
            Expression expression = Expression.SignOf(argument);
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
            Expression argument = Expression.VarX + 1;
            Expression expression = Expression.SignOf(argument);
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
            Expression _ = Expression.SignOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Sign_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = Expression.SignOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Sign_Simplify_DoesntUseEvaluate()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = Expression.SignOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.EvaluateCalled);
        }
    }
}
