using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Operations;
using Algebra.Parsing;

namespace OperationsTests
{
    public class SinTests
    {
        [Test]
        public void Sin_IsEqual_WhenSame()
        {
            // ARANGE
            Expression v1 = Expression.SinOf(Variable.X);
            Expression v2 = Expression.SinOf(Variable.X);

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
        public void Sin_EqualReturnFalse_WhenDifferent()
        {
            // ARANGE
            Expression v1 = Expression.SinOf(Variable.X);
            Expression v2 = Expression.SinOf(Variable.Y);

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
        public void Sin_XDerivative_IsCorrect()
        {
            // ARANGE
            Expression value = Expression.SinOf(Variable.X);
            Expression expected = Expression.CosOf(Variable.X);

            // ACT
            Expression derivative = value.GetDerivative(Variable.X);

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Sin_XSquaredDerivative_IsCorrect()
        {
            // ARANGE
            Expression value = Expression.SinOf(Expression.Pow(Variable.X, 2));
            Expression expected = 2 * Variable.X * Expression.CosOf(Expression.Pow(Variable.X, 2));

            // ACT
            Expression derivative = value.GetDerivative(Variable.X);

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void Sin_Evaluates0Correctly()
        {
            // ARANGE
            Expression equation = Expression.SinOf(0);

            // ACT
            float value = equation.GetDelegate(new VariableInputSet())();

            // ASSERT
            Assert.AreEqual(0, value);
        }

        [Test]
        public void Sin_DoesntSimplify_WhenConstantParameter()
        {
            // ARANGE

            // ACT
            Expression e = Expression.SinOf(10);

            // ASSERT
            Assert.IsFalse(e is Constant);
        }

        [Test]
        public void Sin_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression equation = Expression.SinOf(Variable.X);

            // ASSERT
            Assert.AreEqual(0, equation.GetOrderIndex());
        }

        [Test]
        public void Sin_GetHash_IsNotArgumentHash1()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.Y;
            Expression equation = Expression.SinOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = equation.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sin_GetHash_IsNotArgumentHash2()
        {
            // ARANGE

            // ACT
            Expression argument = 2;
            Expression equation = Expression.SinOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = equation.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sin_GetHash_IsNotArgumentHash3()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.X + 1;
            Expression equation = Expression.SinOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = equation.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sin_GetHash_IsNotSignHash()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.X;
            Expression ln = Expression.SinOf(argument);
            Expression sign = Expression.SignOf(argument);
            int hash1 = ln.GetHashCode();
            int hash2 = sign.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sin_Map_DoesntChangeOriginal()
        {
            // ARANGE
            Expression equation1 = Expression.SinOf(Variable.X);
            Expression equation2 = Expression.SinOf(Variable.X);

            // ACT
            equation2.Map(a => Expression.SinOf(Variable.Y));

            // ASSERT
            Assert.AreEqual(equation1, equation2);
        }

        [Test]
        public void Sin_Map_ReturnsAlternative()
        {
            // ARANGE
            Expression equation1 = Expression.SinOf(Variable.X);

            // ACT
            Expression equation2 = equation1.Map(a => Expression.SinOf(Variable.Y));

            // ASSERT
            Assert.AreEqual(Expression.SinOf(Variable.Y), equation2);
        }

        [Test]
        public void Sin_Map_MapsChildren()
        {
            // ARANGE
            Expression equation1 = Expression.SinOf(Variable.X);

            // ACT
            Expression equation2 = equation1.Map(a => a is Variable ? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(Expression.SinOf(Variable.Z), equation2);
        }

        [Test]
        public void Sin_Map_CanSkipSelf()
        {
            // ARANGE
            Expression equation1 = Expression.SinOf(Variable.X);
            EquationMapping mapping = new EquationMapping()
            {
                PostMap = a => Variable.Z,
                ShouldMapThis = a => !(a is Sin)
            };

            // ACT
            Expression equation2 = equation1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Expression.SinOf(Variable.Z), equation2);
        }

        [Test]
        public void Sin_Map_CanSkipChildren()
        {
            // ARANGE
            Expression equation1 = Expression.SinOf(Variable.X);
            EquationMapping mapping = new EquationMapping()
            {
                PostMap = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression equation2 = equation1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Expression.SinOf(Variable.X), equation2);
        }
    }
}
