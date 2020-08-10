using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Operations;
using Algebra.Parsing;

namespace OperationsTests
{
    public class SignTests
    {
        [Test]
        public void Sign_Simplifies_WhenConstantParameter()
        {
            // ARANGE
            Constant c = Constant.ONE;

            // ACT
            Expression e = Expression.SignOf(10);

            // ASSERT
            Assert.IsTrue(e is Constant);
            Assert.AreEqual(c, e);
        }

        [Test]
        public void Sign_ReturnsZero_WhenZero()
        {
            // ARANGE
            Constant c = Constant.ZERO;

            // ACT
            Expression e = Expression.SignOf(0);

            // ASSERT
            Assert.IsTrue(e is Constant);
            Assert.AreEqual(c, e);
        }

        [Test]
        public void Sign_EvaluatesToZero_WhenZero()
        {
            // ARANGE

            // ACT
            float e = Expression.SignOf(Variable.X).GetDelegate(new VariableInputSet(0))();

            // ASSERT
            Assert.AreEqual(0, e);
        }

        [Test]
        public void Sign_EvaluatesToOne_WhenPositive()
        {
            // ARANGE

            // ACT
            float e = Expression.SignOf(Variable.X).GetDelegate(new VariableInputSet(145))();

            // ASSERT
            Assert.AreEqual(1, e);
        }

        [Test]
        public void Sign_EvaluatesToMinusOne_WhenNegative()
        {
            // ARANGE

            // ACT
            float e = Expression.SignOf(Variable.X).GetDelegate(new VariableInputSet(-14335))();

            // ASSERT
            Assert.AreEqual(-1, e);
        }

        [Test]
        public void Sign_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression equation = Expression.SignOf(Variable.X);

            // ASSERT
            Assert.AreEqual(0, equation.GetOrderIndex());
        }

        [Test]
        public void Sign_GetHash_IsNotArgumentHash1()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.Y;
            Expression equation = Expression.SignOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = equation.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sign_GetHash_IsNotArgumentHash2()
        {
            // ARANGE

            // ACT
            Expression argument = 2;
            Expression equation = Expression.SignOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = equation.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sign_GetHash_IsNotArgumentHash3()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.X + 1;
            Expression equation = Expression.SignOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = equation.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sign_Map_DoesntChangeOriginal()
        {
            // ARANGE
            Expression equation1 = Expression.SignOf(Variable.X);
            Expression equation2 = Expression.SignOf(Variable.X);

            // ACT
            equation2.Map(a => Expression.SignOf(Variable.Y));

            // ASSERT
            Assert.AreEqual(equation1, equation2);
        }

        [Test]
        public void Sign_Map_ReturnsAlternative()
        {
            // ARANGE
            Expression equation1 = Expression.SignOf(Variable.X);

            // ACT
            Expression equation2 = equation1.Map(a => Expression.SignOf(Variable.Y));

            // ASSERT
            Assert.AreEqual(Expression.SignOf(Variable.Y), equation2);
        }

        [Test]
        public void Sign_Map_MapsChildren()
        {
            // ARANGE
            Expression equation1 = Expression.SignOf(Variable.X);

            // ACT
            Expression equation2 = equation1.Map(a => a is Variable ? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(Expression.SignOf(Variable.Z), equation2);
        }

        [Test]
        public void Sign_Map_CanSkipSelf()
        {
            // ARANGE
            Expression equation1 = Expression.SignOf(Variable.X);
            EquationMapping mapping = new EquationMapping()
            {
                PostMap = a => Variable.Z,
                ShouldMapThis = a => !(a is Sign)
            };

            // ACT
            Expression equation2 = equation1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Expression.SignOf(Variable.Z), equation2);
        }

        [Test]
        public void Sign_Map_CanSkipChildren()
        {
            // ARANGE
            Expression equation1 = Expression.SignOf(Variable.X);
            EquationMapping mapping = new EquationMapping()
            {
                PostMap = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression equation2 = equation1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Expression.SignOf(Variable.X), equation2);
        }
    }
}
