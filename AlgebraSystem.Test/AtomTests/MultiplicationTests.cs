using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;

namespace AtomTests
{
    public class MultiplicationTests
    {
        [Test]
        public void Multiplication_IsEqual_WhenSame()
        {
            // ARANGE
            Expression v1 = Variable.X * 2;
            Expression v2 = Variable.X * 2;

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
            Expression v1 = Variable.X * 7;
            Expression v2 = 7 * Variable.X;

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
            Expression v1 = Variable.X * 1;
            Expression v2 = Variable.X * 2;

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
            Expression value = Variable.X * Variable.Y;
            Expression expected = 1 * Variable.Y + Variable.X * 0;

            // ACT
            Expression derivative = value.GetDerivative(Variable.X);

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Multiplication_EvaluatesCorrectly()
        {
            // ARANGE
            Expression equation = Constant.From(54321) * Constant.From(7);

            // ACT
            float value = equation.GetDelegate(new VariableInputSet())();

            // ASSERT
            Assert.AreEqual(54321 * 7, value);
        }

        [Test]
        public void Multiplication_Simplify_CollectsConstants()
        {
            // ARANGE

            // ACT
            Expression equation = Constant.From(54321) * Variable.Z * Constant.From(54321);
            Expression expected = Constant.From(((Rational)54321) * 54321) * Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Multiplication_Simplify_CollectsPowers()
        {
            // ARANGE

            // ACT
            Expression equation = Expression.Pow(Variable.Z, 2) * Expression.Pow(Variable.Z, Variable.Y) * Variable.Z;
            Expression expected = Expression.Pow(Variable.Z, 3 + Variable.Y);

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Multiplication_Simplify_RemovesOnes()
        {
            // ARANGE

            // ACT
            Expression equation = 1 * 2 * Variable.Z * (Rational)0.5M;
            Expression expected = Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Multiplication_Simplify_CancelsVaraibles()
        {
            // ARANGE

            // ACT
            Expression equation = Variable.Z / Variable.Z;
            Expression expected = Constant.From(1);

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Multiplication_Simplify_CancelsVaraiblesTimesTwo()
        {
            // ARANGE

            // ACT
            Expression equation = Constant.From(2) * Variable.Z / Variable.Z;
            Expression expected = Constant.From(2);

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Multiplication_Simplify_CollatesMultiplication()
        {
            // ARANGE

            // ACT
            Expression equation = Expression.Multiply(new List<Expression>() { Expression.Multiply(new List<Expression>() { Variable.X, Variable.Y }), Variable.Z });
            Expression expected = Expression.Multiply(new List<Expression>() { Variable.X, Variable.Y, Variable.Z });

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Multiplication_Simplify_GoesToConstantZero()
        {
            // ARANGE

            // ACT
            Expression equation = 0 * 2 * Variable.Z * (Rational)0.5M;
            Expression expected = 0;

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Multiplication_DoesNotSimplify_DOTS()
        {
            // ARANGE

            // ACT
            Expression equation = (Variable.X + 1) * (Variable.X - 1);
            Expression expected = Expression.Pow(Variable.X, 2) - 1;

            // ASSERT
            Assert.AreNotEqual(expected, equation);
        }

        [Test]
        public void Multiplication_Simplify_DoesNotExpandBraces()
        {
            // ARANGE

            // ACT
            Expression equation = (Variable.X + 1) * (Variable.X + 2);
            Expression expected = Expression.Pow(Variable.X, 2) + 3 * Variable.X + 2;

            // ASSERT
            Assert.AreNotEqual(expected, equation);
        }

        [Test]
        public void Multiplication_Simplify_DoesNotDistribute()
        {
            // ARANGE

            // ACT
            Expression equation = 3 * (Variable.X + 1) - 3;
            Expression expected = 3 * Variable.X;

            // ASSERT
            Assert.AreNotEqual(expected, equation);
        }

        [Test]
        public void Multiplication_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression equation = 3 * Variable.X;

            // ASSERT
            Assert.AreEqual(20, equation.GetOrderIndex());
        }

        [Test]
        public void Multiplication_Map_DoesntChangeOriginal()
        {
            // ARANGE
            Expression equation1 = Variable.X * 2;
            Expression equation2 = Variable.X * 2;

            // ACT
            equation2.Map(a => Variable.Y * 2);

            // ASSERT
            Assert.AreEqual(equation1, equation2);
        }

        [Test]
        public void Multiplication_Map_ReturnsAlternative()
        {
            // ARANGE
            Expression equation1 = Variable.X * 2;

            // ACT
            Expression equation2 = equation1.Map(a => Variable.Z);

            // ASSERT
            Assert.AreEqual(Variable.Z, equation2);
        }

        [Test]
        public void Multiplication_Map_MapsChildren()
        {
            // ARANGE
            Expression equation1 = Variable.X * Variable.Y;

            // ACT
            Expression equation2 = equation1.Map(a => a is Variable ? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(Variable.Z * Variable.Z, equation2);
        }

        [Test]
        public void Multiplication_Map_CanSkipSelf()
        {
            // ARANGE
            Expression equation1 = Variable.X * Variable.Y;
            EquationMapping mapping = new EquationMapping()
            {
                PostMap = a => Variable.Z,
                ShouldMapThis = a => !(a is Product)
            };

            // ACT
            Expression equation2 = equation1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Variable.Z * Variable.Z, equation2);
        }

        [Test]
        public void Multiplication_Map_CanSkipChildren()
        {
            // ARANGE
            Expression equation1 = Variable.X * Variable.Y;
            EquationMapping mapping = new EquationMapping()
            {
                PostMap = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression equation2 = equation1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Variable.X * Variable.Y, equation2);
        }
    }
}
