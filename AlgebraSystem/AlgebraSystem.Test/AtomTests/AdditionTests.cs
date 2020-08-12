using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;

namespace AtomTests
{
    public class AdditionTests
    {
        [Test]
        public void Addition_IsEqual_WhenSame()
        {
            // ARANGE
            Expression v1 = Variable.X + 1;
            Expression v2 = Variable.X + 1;

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
        public void Addition_IsEqual_Commutative()
        {
            // ARANGE
            Expression v1 = Variable.X + 1;
            Expression v2 = 1 + Variable.X;

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
        public void Addition_EqualReturnFalse_WhenDifferent()
        {
            // ARANGE
            Expression v1 = Variable.X + 1;
            Expression v2 = Variable.X + 2;

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
        public void Addition_Derivative_IsDerivativeSum()
        {
            // ARANGE
            Expression value = Variable.X + Variable.Y;
            Expression expected = Constant.From(1) + Constant.From(0);

            // ACT
            Expression derivative = value.GetDerivative(Variable.X);

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Addition_EvaluatesCorrectly()
        {
            // ARANGE
            Expression equation = Constant.From(54321) + Constant.From(7);

            // ACT
            float value = equation.GetDelegate(new VariableInputSet())();

            // ASSERT
            Assert.AreEqual(54321 + 7, value);
        }

        [Test]
        public void Addition_Simplify_CollectsConstants()
        {
            // ARANGE

            // ACT
            Expression equation = Constant.From(54321) + Variable.Z + Constant.From(54321);
            Expression expected = Constant.From(54321 + 54321) + Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Addition_Simplify_CollectsCoefficients()
        {
            // ARANGE

            // ACT
            Expression equation = (Constant.From(54321) * Variable.Z) + (Constant.From(54321) * Variable.Z) + Variable.Z;
            Expression expected = Constant.From(54321 + 54321 + 1) * Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Addition_Simplify_RemovesZeros()
        {
            // ARANGE

            // ACT
            Expression equation = Constant.From(0) + Constant.From(1) + Constant.From(-1) + Variable.Z;
            Expression expected = Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Addition_Simplify_CancelsVaraibles()
        {
            // ARANGE

            // ACT
            Expression equation = Variable.Z - Variable.Z;
            Expression expected = Constant.From(0);

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Addition_Simplify_CancelsVaraiblesPlusOne()
        {
            // ARANGE

            // ACT
            Expression equation = Constant.From(1) + Variable.Z - Variable.Z;
            Expression expected = Constant.From(1);

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Addition_Simplify_RemovesVaraibleCoefficientIfOne()
        {
            // ARANGE

            // ACT
            Expression equation = Constant.From(1) + 2 * Variable.Z - Variable.Z;
            Expression expected = Constant.From(1) + Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Addition_Simplify_CollatesAdditions()
        {
            // ARANGE

            // ACT
            Expression equation = Expression.Add(new List<Expression>() { Expression.Add(new List<Expression>() { Variable.X, Variable.Y }), Variable.Z });
            Expression expected = Expression.Add(new List<Expression>() { Variable.X, Variable.Y, Variable.Z });

            // ASSERT
            Assert.AreEqual(expected, equation);
        }

        [Test]
        public void Addition_Simplify_DoesNotFactorise()
        {
            // ARANGE

            // ACT
            Expression equation = (Expression.Pow(Variable.X, 2) + (3 * Variable.X) + 2) / (Variable.X + 1);
            Expression expected = Variable.X + 2;

            // ASSERT
            Assert.AreNotEqual(expected, equation);
        }

        [Test]
        public void Addition_GetOrderIndex_Is30()
        {
            // ARANGE

            // ACT
            Expression equation = Variable.X + 1;

            // ASSERT
            Assert.AreEqual(30, equation.GetOrderIndex());
        }

        [Test]
        public void Addition_Map_DoesntChangeOriginal()
        {
            // ARANGE
            Expression equation1 = Variable.X + 1;
            Expression equation2 = Variable.X + 1;

            // ACT
            equation2.Map(a => Variable.Y + 2);

            // ASSERT
            Assert.AreEqual(equation1, equation2);
        }

        [Test]
        public void Addition_Map_ReturnsAlternative()
        {
            // ARANGE
            Expression equation1 = Variable.X + 1;

            // ACT
            Expression equation2 = equation1.Map(a => Variable.Z);

            // ASSERT
            Assert.AreEqual(Variable.Z, equation2);
        }

        [Test]
        public void Addition_Map_MapsChildren()
        {
            // ARANGE
            Expression equation1 = Variable.X + Variable.Y;

            // ACT
            Expression equation2 = equation1.Map(a => a is Variable? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(2 * Variable.Z, equation2);
        }

        [Test]
        public void Addition_Map_CanSkipSelf()
        {
            // ARANGE
            Expression equation1 = Variable.X + 1;
            EquationMapping mapping = new EquationMapping()
            {
                PostMap = a => Variable.Z,
                ShouldMapThis = a => !(a is Sum)
            };

            // ACT
            Expression equation2 = equation1.Map(mapping);

            // ASSERT
            Assert.AreEqual(2 * Variable.Z, equation2);
        }

        [Test]
        public void Addition_Map_CanSkipChildren()
        {
            // ARANGE
            Expression equation1 = Variable.X + 1;
            EquationMapping mapping = new EquationMapping()
            {
                PostMap = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression equation2 = equation1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Variable.X + 1, equation2);
        }
    }
}
