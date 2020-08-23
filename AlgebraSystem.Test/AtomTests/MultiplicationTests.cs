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
            IExpression v1 = IExpression.X * 2;
            IExpression v2 = IExpression.X * 2;

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
            IExpression v1 = IExpression.X * 7;
            IExpression v2 = 7 * IExpression.X;

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
            IExpression v1 = IExpression.X * 1;
            IExpression v2 = IExpression.X * 2;

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
            IExpression value = IExpression.X * IExpression.Y;
            IExpression expected = 1 * IExpression.Y + IExpression.X * 0;

            // ACT
            IExpression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Multiplication_EvaluatesCorrectly([Range(-10, 10)] int a, [Range(-10, 10)] int b)
        {
            // ARANGE
            IExpression expression = IExpression.ConstantFrom(a) * IExpression.ConstantFrom(b);

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
            IExpression expression = IExpression.ConstantFrom(54321) * IExpression.Z * IExpression.ConstantFrom(54321);
            IExpression expected = IExpression.ConstantFrom(((Rational)54321) * 54321) * IExpression.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_CollectsPowers()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.Pow(IExpression.Z, 2) * IExpression.Pow(IExpression.Z, IExpression.Y) * IExpression.Z;
            IExpression expected = IExpression.Pow(IExpression.Z, 3 + IExpression.Y);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_RemovesOnes()
        {
            // ARANGE

            // ACT
            IExpression expression = 1 * 2 * IExpression.Z * (Rational)0.5M;
            IExpression expected = IExpression.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_CancelsVaraibles()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.Z * IExpression.Pow(IExpression.Z, -1);
            IExpression expected = IExpression.ConstantFrom(1);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_CancelsVaraiblesTimesTwo()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.ConstantFrom(2) * IExpression.Z * IExpression.Pow(IExpression.Z, -1);
            IExpression expected = IExpression.ConstantFrom(2);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_CollatesMultiplication()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.Multiply(new List<IExpression>() { IExpression.Multiply(new List<IExpression>() { IExpression.X, IExpression.Y }), IExpression.Z });
            IExpression expected = IExpression.Multiply(new List<IExpression>() { IExpression.X, IExpression.Y, IExpression.Z });

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_GoesToConstantZero()
        {
            // ARANGE

            // ACT
            IExpression expression = 0 * 2 * IExpression.Z * (Rational)0.5M;
            IExpression expected = 0;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_DoesNotSimplify_DOTS()
        {
            // ARANGE

            // ACT
            IExpression expression = (IExpression.X + 1) * (IExpression.X - 1);
            IExpression expected = IExpression.Pow(IExpression.X, 2) - 1;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_DoesNotExpandBraces()
        {
            // ARANGE

            // ACT
            IExpression expression = (IExpression.X + 1) * (IExpression.X + 2);
            IExpression expected = IExpression.Pow(IExpression.X, 2) + 3 * IExpression.X + 2;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_DoesNotDistribute()
        {
            // ARANGE

            // ACT
            IExpression expression = 3 * (IExpression.X + 1) - 3;
            IExpression expected = 3 * IExpression.X;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Multiplication_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            IExpression expression = 3 * IExpression.X;

            // ASSERT
            Assert.AreEqual(20, expression.GetOrderIndex());
        }
        [Test]
        public void Multiplication_Simplify_DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = dummy1 * 2;

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Multiplication_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = dummy1 * 2;

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Multiplication_Simplify_DoesntUseMap()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = dummy1 * 2;

            // ASSERT
            Assert.IsFalse(dummy1.MapChildrenCalled);
        }
    }
}
