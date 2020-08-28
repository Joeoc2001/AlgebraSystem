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
    public class LogTests
    {

        [Test]
        public void Log_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            IExpression expression = Expression.LnOf(Expression.VarX);

            // ASSERT
            Assert.AreEqual(0, expression.GetOrderIndex());
        }

        [Test]
        public void Log_EvaluatesCorrectly([Range(1, 100)] int v)
        {
            // ARANGE
            IExpression expression = Expression.LnOf(v);

            // ACT
            float value = expression.EvaluateOnce(new VariableInputSet<float>());
            double expected = Math.Log(v);

            // ASSERT
            Assert.That(value, Is.EqualTo(expected).Within(0.00001f));
        }

        [Test]
        public void Log_GetHash_IsNotArgumentHash1()
        {
            // ARANGE

            // ACT
            IExpression argument = Expression.VarY;
            IExpression expression = Expression.LnOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Log_GetHash_IsNotArgumentHash2()
        {
            // ARANGE

            // ACT
            Expression argument = 2;
            IExpression expression = Expression.LnOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Log_GetHash_IsNotArgumentHash3()
        {
            // ARANGE

            // ACT
            IExpression argument = Expression.VarX + 1;
            IExpression expression = Expression.LnOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Log_GetHash_IsNotSignHash()
        {
            // ARANGE

            // ACT
            IExpression argument = Expression.VarX;
            IExpression ln = Expression.LnOf(argument);
            IExpression sign = Expression.SignOf(argument);
            int hash1 = ln.GetHashCode();
            int hash2 = sign.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Log_Simplify_DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = Expression.LnOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Log_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = Expression.LnOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Log_Simplify_DoesntUseEvaluate()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = Expression.LnOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.EvaluateCalled);
        }
    }
}
