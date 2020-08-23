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
            IExpression expression = IExpression.LnOf(IExpression.X);

            // ASSERT
            Assert.AreEqual(0, expression.GetOrderIndex());
        }

        [Test]
        public void Log_EvaluatesCorrectly([Range(1, 100)] int v)
        {
            // ARANGE
            IExpression expression = IExpression.LnOf(v);

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
            IExpression argument = IExpression.Y;
            IExpression expression = IExpression.LnOf(argument);
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
            IExpression argument = 2;
            IExpression expression = IExpression.LnOf(argument);
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
            IExpression argument = IExpression.X + 1;
            IExpression expression = IExpression.LnOf(argument);
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
            IExpression argument = IExpression.X;
            IExpression ln = IExpression.LnOf(argument);
            IExpression sign = IExpression.SignOf(argument);
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
            IExpression _ = IExpression.LnOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Log_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = IExpression.LnOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Log_Simplify_DoesntUseMap()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = IExpression.LnOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.MapChildrenCalled);
        }
    }
}
