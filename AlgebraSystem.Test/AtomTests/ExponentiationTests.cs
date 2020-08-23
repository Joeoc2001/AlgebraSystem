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
    public class ExponentiationTests
    {
        [Test]
        public void Exponentiation_IsEqual_WhenSame()
        {
            // ARANGE
            IExpression v1 = IExpression.Pow(IExpression.X, 2);
            IExpression v2 = IExpression.Pow(IExpression.X, 2);

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
        public void Exponentiation_EqualReturnFalse_WhenDifferent()
        {
            // ARANGE
            IExpression v1 = IExpression.Pow(IExpression.X, 6);
            IExpression v2 = IExpression.Pow(6, IExpression.X);

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
        public void Exponentiation_1stPowerDerivative_IsCorrect()
        {
            // ARANGE
            IExpression value = IExpression.Pow(IExpression.X, 1);
            IExpression expected = 1;

            // ACT
            IExpression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Exponentiation_ConstantPowerDerivative_IsCorrect()
        {
            // ARANGE
            IExpression value = IExpression.Pow(IExpression.X, 5);
            IExpression expected = 5 * IExpression.Pow(IExpression.X, 4);

            // ACT
            IExpression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Exponentiation_ConstantBaseDerivative_IsCorrect()
        {
            // ARANGE
            IExpression value = IExpression.Pow(5, IExpression.X);
            IExpression expected = IExpression.LnOf(5) * IExpression.Pow(5, IExpression.X);

            // ACT
            IExpression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Exponentiation_BothVariableDerivative_IsAtomicallyCorrect()
        {
            // ARANGE
            IExpression value = IExpression.Pow(IExpression.X, IExpression.X);
            IExpression expected = (1 + IExpression.LnOf(IExpression.X)) * IExpression.Pow(IExpression.X, IExpression.X);

            // ACT
            IExpression derivative = value.GetDerivative("x");
            IExpression atomicDerivative = derivative.GetAtomicExpression();

            // ASSERT
            Assert.AreEqual(expected, atomicDerivative);
        }

        [Test]
        public void Exponentiation_EvaluatesCorrectly([Range(-10, 10)] int a, [Range(0, 10)] int b)
        {
            // ARANGE
            IExpression expression = IExpression.Pow(a, b);

            // ACT
            float value = expression.EvaluateOnce(new VariableInputSet<float>());

            // ASSERT
            Assert.AreEqual((float)Math.Pow(a, b), value);
        }

        [Test]
        public void Exponentiation_Simplify_CollapsesConstants()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.Pow(5, IExpression.ConstantFrom(2));
            IExpression expected = IExpression.ConstantFrom(25);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Exponentiation_Simplify_RemovesPowersOf1()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.Pow(IExpression.Z, 1);
            IExpression expected = IExpression.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Exponentiation_Simplify_RemovesPowersOf0()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.Pow(IExpression.Y, 0);
            IExpression expected = 1;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Exponentiation_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.Pow(IExpression.Y, 3);

            // ASSERT
            Assert.AreEqual(10, expression.GetOrderIndex());
        }

        [Test]
        public void Exponentiation_GetHash_IsNotCommutative1()
        {
            // ARANGE

            // ACT
            IExpression expression1 = IExpression.Pow(IExpression.Y, 3);
            IExpression expression2 = IExpression.Pow(3, IExpression.Y);
            int hash1 = expression1.GetHashCode();
            int hash2 = expression2.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Exponentiation_GetHash_IsNotCommutative2()
        {
            // ARANGE

            // ACT
            IExpression expression1 = IExpression.Pow(IExpression.Y, IExpression.X);
            IExpression expression2 = IExpression.Pow(IExpression.X, IExpression.Y);
            int hash1 = expression1.GetHashCode();
            int hash2 = expression2.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Exponentiation_GetHash_IsNotCommutative3()
        {
            // ARANGE

            // ACT
            IExpression expression1 = IExpression.Pow(IExpression.Y, IExpression.X + 1);
            IExpression expression2 = IExpression.Pow(IExpression.X + 1, IExpression.Y);
            int hash1 = expression1.GetHashCode();
            int hash2 = expression2.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Exponentiation_Simplify_DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = IExpression.Pow(dummy1, 2);

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Exponentiation_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = IExpression.Pow(dummy1, 2);

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Exponentiation_Simplify_DoesntUseMap()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = IExpression.Pow(dummy1, 2);

            // ASSERT
            Assert.IsFalse(dummy1.MapChildrenCalled);
        }
    }
}
