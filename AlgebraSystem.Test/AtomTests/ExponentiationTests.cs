using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;

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
        public void Exponentiation_EvaluatesCorrectly()
        {
            // ARANGE
            Expression equation = Expression.Pow(2, Constant.From(7));

            // ACT
            float value = equation.GetDelegate(new VariableInputSet())();

            // ASSERT
            Assert.AreEqual(128.0f, value);
        }

        [Test]
        public void Exponentiation_Simplify_CollapsesConstants()
        {
            // ARANGE

            // ACT
            Expression equation = Expression.Pow(5, Constant.From(2));
            Expression expected = Constant.From(25);

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Exponentiation_Simplify_RemovesPowersOf1()
        {
            // ARANGE

            // ACT
            Expression equation = Expression.Pow(Variable.Z, 1);
            Expression expected = Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Exponentiation_Simplify_RemovesPowersOf0()
        {
            // ARANGE

            // ACT
            Expression equation = Expression.Pow(Variable.Y, 0);
            Expression expected = 1;

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Exponentiation_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression equation = Expression.Pow(Variable.Y, 3);

            // ASSERT
            Assert.AreEqual(10, equation.GetOrderIndex());
        }

        [Test]
        public void Exponentiation_GetHash_IsNotCommutative1()
        {
            // ARANGE

            // ACT
            Expression equation1 = Expression.Pow(Variable.Y, 3);
            Expression equation2 = Expression.Pow(3, Variable.Y);
            int hash1 = equation1.GetHashCode();
            int hash2 = equation2.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Exponentiation_GetHash_IsNotCommutative2()
        {
            // ARANGE

            // ACT
            Expression equation1 = Expression.Pow(Variable.Y, Variable.X);
            Expression equation2 = Expression.Pow(Variable.X, Variable.Y);
            int hash1 = equation1.GetHashCode();
            int hash2 = equation2.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Exponentiation_GetHash_IsNotCommutative3()
        {
            // ARANGE

            // ACT
            Expression equation1 = Expression.Pow(Variable.Y, Variable.X + 1);
            Expression equation2 = Expression.Pow(Variable.X + 1, Variable.Y);
            int hash1 = equation1.GetHashCode();
            int hash2 = equation2.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Exponentiation_Map_DoesntChangeOriginal()
        {
            // ARANGE
            Expression equation1 = Expression.Pow(Variable.X, 2);
            Expression equation2 = Expression.Pow(Variable.X, 2);

            // ACT
            equation2.Map(a => Expression.Pow(Variable.Y, 4));

            // ASSERT
            Assert.AreEqual(equation1, equation2);
        }

        [Test]
        public void Exponentiation_Map_ReturnsAlternative()
        {
            // ARANGE
            Expression equation1 = Expression.Pow(Variable.X, 2);

            // ACT
            Expression equation2 = equation1.Map(a => Expression.Pow(Variable.Y, 4));

            // ASSERT
            Assert.AreEqual(Expression.Pow(Variable.Y, 4), equation2);
        }

        [Test]
        public void Exponentiation_Map_MapsChildren()
        {
            // ARANGE
            Expression equation1 = Expression.Pow(Variable.X, 5);

            // ACT
            Expression equation2 = equation1.Map(a => a is Variable ? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(Expression.Pow(Variable.Z, 5), equation2);
        }

        [Test]
        public void Exponentiation_Map_CanSkipSelf()
        {
            // ARANGE
            Expression equation1 = Expression.Pow(Variable.X, Variable.Y);
            EquationMapping mapping = new EquationMapping()
            {
                PostMap = a => Variable.Z,
                ShouldMapThis = a => !(a is Exponent)
            };

            // ACT
            Expression equation2 = equation1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Expression.Pow(Variable.Z, Variable.Z), equation2);
        }

        [Test]
        public void Exponentiation_Map_CanSkipChildren()
        {
            // ARANGE
            Expression equation1 = Expression.Pow(Variable.X, 5);
            EquationMapping mapping = new EquationMapping()
            {
                PostMap = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression equation2 = equation1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Expression.Pow(Variable.X, 5), equation2);
        }
    }
}
