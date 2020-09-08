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
    public class ExponentiationTests
    {
        [Test]
        public void Exponentiation_IsEqual_WhenSame()
        {
            // ARANGE
            Expression v1 = Expression.Pow(Expression.VarX, 2);
            Expression v2 = Expression.Pow(Expression.VarX, 2);

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
            Expression v1 = Expression.Pow(Expression.VarX, 6);
            Expression v2 = Expression.Pow(6, Expression.VarX);

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
            Expression value = Expression.Pow(Expression.VarX, 1);
            Expression expected = 1;

            // ACT
            Expression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Exponentiation_ConstantPowerDerivative_IsCorrect()
        {
            // ARANGE
            Expression value = Expression.Pow(Expression.VarX, 5);
            Expression expected = 5 * Expression.Pow(Expression.VarX, 4);

            // ACT
            Expression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Exponentiation_ConstantBaseDerivative_IsCorrect()
        {
            // ARANGE
            Expression value = Expression.Pow(5, Expression.VarX);
            Expression expected = Expression.LnOf(5) * Expression.Pow(5, Expression.VarX);

            // ACT
            Expression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Exponentiation_BothVariableDerivative_IsAtomicallyCorrect()
        {
            // ARANGE
            Expression value = Expression.Pow(Expression.VarX, Expression.VarX);
            Expression expected = (1 + Expression.LnOf(Expression.VarX)) * Expression.Pow(Expression.VarX, Expression.VarX);

            // ACT
            Expression derivative = value.GetDerivative("x");
            Expression atomicDerivative = derivative.GetAtomicExpression();

            // ASSERT
            Assert.AreEqual(expected, atomicDerivative);
        }

        [Test, Pairwise]
        public void Exponentiation_EvaluatesCorrectly([Range(-8, 8)] int a, [Range(0, 8)] int b)
        {
            // ARANGE
            Expression expression = Expression.Pow(a, b);

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
            Expression expression = Expression.Pow(5, Expression.ConstantFrom(2));
            Expression expected = Expression.ConstantFrom(25);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Exponentiation_Simplify_RemovesPowersOf1()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Pow(Expression.VarZ, 1);
            Expression expected = Expression.VarZ;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Exponentiation_Simplify_RemovesPowersOf0()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Pow(Expression.VarY, 0);
            Expression expected = 1;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Exponentiation_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Pow(Expression.VarY, 3);

            // ASSERT
            Assert.AreEqual(10, expression.GetOrderIndex());
        }

        [Test]
        public void Exponentiation_GetHash_IsNotCommutative1()
        {
            // ARANGE

            // ACT
            Expression expression1 = Expression.Pow(Expression.VarY, 3);
            Expression expression2 = Expression.Pow(3, Expression.VarY);
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
            Expression expression1 = Expression.Pow(Expression.VarY, Expression.VarX);
            Expression expression2 = Expression.Pow(Expression.VarX, Expression.VarY);
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
            Expression expression1 = Expression.Pow(Expression.VarY, Expression.VarX + 1);
            Expression expression2 = Expression.Pow(Expression.VarX + 1, Expression.VarY);
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
            Expression _ = Expression.Pow(dummy1, 2);

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Exponentiation_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = Expression.Pow(dummy1, 2);

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Exponentiation_Simplify_DoesntUseEvaluate()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = Expression.Pow(dummy1, 2);

            // ASSERT
            Assert.IsFalse(dummy1.EvaluateCalled);
        }
    }
}
