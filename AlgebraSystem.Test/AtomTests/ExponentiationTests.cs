using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;
using System;

namespace AtomTests
{
    public class ExponentiationTests
    {
        [Test]
        public void Exponentiation_IsEqual_WhenSame()
        {
            // ARANGE
            Expression v1 = Expression.Pow(Variable.X, 2);
            Expression v2 = Expression.Pow(Variable.X, 2);

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
            Expression v1 = Expression.Pow(Variable.X, 6);
            Expression v2 = Expression.Pow(6, Variable.X);

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
            Expression value = Expression.Pow(Variable.X, 1);
            Expression expected = 1;

            // ACT
            Expression derivative = value.GetDerivative(Variable.X);

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Exponentiation_ConstantPowerDerivative_IsCorrect()
        {
            // ARANGE
            Expression value = Expression.Pow(Variable.X, 5);
            Expression expected = 5 * Expression.Pow(Variable.X, 4);

            // ACT
            Expression derivative = value.GetDerivative(Variable.X);

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Exponentiation_ConstantBaseDerivative_IsCorrect()
        {
            // ARANGE
            Expression value = Expression.Pow(5, Variable.X);
            Expression expected = Expression.LnOf(5) * Expression.Pow(5, Variable.X);

            // ACT
            Expression derivative = value.GetDerivative(Variable.X);

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Exponentiation_BothVariableDerivative_IsCorrect()
        {
            // ARANGE
            Expression value = Expression.Pow(Variable.X, Variable.X);
            Expression expected = (1 + Expression.LnOf(Variable.X)) * Expression.Pow(Variable.X, Variable.X);

            // ACT
            Expression derivative = value.GetDerivative(Variable.X);

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Exponentiation_EvaluatesCorrectly([Range(-10, 10)] int a, [Range(0, 10)] int b)
        {
            // ARANGE
            Expression expression = Expression.Pow(a, b);

            // ACT
            float value = expression.GetDelegate(new VariableInputSet())();

            // ASSERT
            Assert.AreEqual((float)Math.Pow(a, b), value);
        }

        [Test]
        public void Exponentiation_Simplify_CollapsesConstants()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Pow(5, Constant.From(2));
            Expression expected = Constant.From(25);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Exponentiation_Simplify_RemovesPowersOf1()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Pow(Variable.Z, 1);
            Expression expected = Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Exponentiation_Simplify_RemovesPowersOf0()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Pow(Variable.Y, 0);
            Expression expected = 1;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Exponentiation_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Pow(Variable.Y, 3);

            // ASSERT
            Assert.AreEqual(10, expression.GetOrderIndex());
        }

        [Test]
        public void Exponentiation_GetHash_IsNotCommutative1()
        {
            // ARANGE

            // ACT
            Expression expression1 = Expression.Pow(Variable.Y, 3);
            Expression expression2 = Expression.Pow(3, Variable.Y);
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
            Expression expression1 = Expression.Pow(Variable.Y, Variable.X);
            Expression expression2 = Expression.Pow(Variable.X, Variable.Y);
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
            Expression expression1 = Expression.Pow(Variable.Y, Variable.X + 1);
            Expression expression2 = Expression.Pow(Variable.X + 1, Variable.Y);
            int hash1 = expression1.GetHashCode();
            int hash2 = expression2.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Exponentiation_Map_DoesntChangeOriginal()
        {
            // ARANGE
            Expression expression1 = Expression.Pow(Variable.X, 2);
            Expression expression2 = Expression.Pow(Variable.X, 2);

            // ACT
            expression2.Map(a => Expression.Pow(Variable.Y, 4));

            // ASSERT
            Assert.AreEqual(expression1, expression2);
        }

        [Test]
        public void Exponentiation_Map_ReturnsAlternative()
        {
            // ARANGE
            Expression expression1 = Expression.Pow(Variable.X, 2);

            // ACT
            Expression expression2 = expression1.Map(a => Expression.Pow(Variable.Y, 4));

            // ASSERT
            Assert.AreEqual(Expression.Pow(Variable.Y, 4), expression2);
        }

        [Test]
        public void Exponentiation_Map_MapsChildren()
        {
            // ARANGE
            Expression expression1 = Expression.Pow(Variable.X, 5);

            // ACT
            Expression expression2 = expression1.Map(a => a is Variable ? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(Expression.Pow(Variable.Z, 5), expression2);
        }

        [Test]
        public void Exponentiation_Map_CanSkipSelf()
        {
            // ARANGE
            Expression expression1 = Expression.Pow(Variable.X, Variable.Y);
            ExpressionMapping mapping = new ExpressionMapping()
            {
                PostMap = a => Variable.Z,
                ShouldMapThis = a => !(a is Exponent)
            };

            // ACT
            Expression expression2 = expression1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Expression.Pow(Variable.Z, Variable.Z), expression2);
        }

        [Test]
        public void Exponentiation_Map_CanSkipChildren()
        {
            // ARANGE
            Expression expression1 = Expression.Pow(Variable.X, 5);
            ExpressionMapping mapping = new ExpressionMapping()
            {
                PostMap = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression expression2 = expression1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Expression.Pow(Variable.X, 5), expression2);
        }
    }
}
