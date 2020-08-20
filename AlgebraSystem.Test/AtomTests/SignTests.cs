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
        public void Addition_EvaluatesCorrectlyFor([Range(-1000, 1000, 10)] int a)
        {
            // ARANGE
            Expression expression = Expression.SignOf(a);

            // ACT
            float value = expression.GetDelegate(new VariableInputSet())();
            int expected = Math.Sign(a);

            // ASSERT
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void Sign_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.SignOf(Variable.X);

            // ASSERT
            Assert.AreEqual(0, expression.GetOrderIndex());
        }

        [Test]
        public void Sign_GetHash_IsNotArgumentHash1()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.Y;
            Expression expression = Expression.SignOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sign_GetHash_IsNotArgumentHash2()
        {
            // ARANGE

            // ACT
            Expression argument = 2;
            Expression expression = Expression.SignOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sign_GetHash_IsNotArgumentHash3()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.X + 1;
            Expression expression = Expression.SignOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Sign_Map_DoesntChangeOriginal()
        {
            // ARANGE
            Expression expression1 = Expression.SignOf(Variable.X);
            Expression expression2 = Expression.SignOf(Variable.X);

            // ACT
            expression2.PostMap(a => Expression.SignOf(Variable.Y));

            // ASSERT
            Assert.AreEqual(expression1, expression2);
        }

        [Test]
        public void Sign_Map_ReturnsAlternative()
        {
            // ARANGE
            Expression expression1 = Expression.SignOf(Variable.X);

            // ACT
            Expression expression2 = expression1.PostMap(a => Expression.SignOf(Variable.Y));

            // ASSERT
            Assert.AreEqual(Expression.SignOf(Variable.Y), expression2);
        }

        [Test]
        public void Sign_Map_MapsChildren()
        {
            // ARANGE
            Expression expression1 = Expression.SignOf(Variable.X);

            // ACT
            Expression expression2 = expression1.PostMap(a => a is Variable ? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(Expression.SignOf(Variable.Z), expression2);
        }

        [Test]
        public void Sign_Map_CanSkipSelf()
        {
            // ARANGE
            Expression expression1 = Expression.SignOf(Variable.X);
            ExpressionMapping mapping = new ExpressionMapping()
            {
                Map = a => Variable.Z,
                ShouldMapThis = a => !(a is Sign)
            };

            // ACT
            Expression expression2 = expression1.PostMap(mapping);

            // ASSERT
            Assert.AreEqual(Expression.SignOf(Variable.Z), expression2);
        }

        [Test]
        public void Sign_Map_CanSkipChildren()
        {
            // ARANGE
            Expression expression1 = Expression.SignOf(Variable.X);
            ExpressionMapping mapping = new ExpressionMapping()
            {
                Map = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression expression2 = expression1.PostMap(mapping);

            // ASSERT
            Assert.AreEqual(Expression.SignOf(Variable.X), expression2);
        }

        [Test]
        public void Sign_Simplify_DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = Expression.SignOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Sign_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = Expression.SignOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Sign_Simplify_DoesntUseMap()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = Expression.SignOf(dummy1);

            // ASSERT
            Assert.IsFalse(dummy1.MapChildrenCalled);
        }
    }
}
