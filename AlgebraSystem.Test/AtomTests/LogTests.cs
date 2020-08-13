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
    public class LogTests
    {
        [Test]
        public void Log_DoesntSimplify_WhenConstantParameter()
        {
            // ARANGE

            // ACT
            Expression e = Expression.LnOf(10);

            // ASSERT
            Assert.IsFalse(e is Constant);
        }

        [Test]
        public void Log_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.LnOf(Variable.X);

            // ASSERT
            Assert.AreEqual(0, expression.GetOrderIndex());
        }

        [Test]
        public void Log_EvaluatesCorrectly([Range(1, 100)] int v)
        {
            // ARANGE
            Expression expression = Expression.LnOf(v);

            // ACT
            float value = expression.GetDelegate(new VariableInputSet())();
            double expected = Math.Log(v);

            // ASSERT
            Assert.That(value, Is.EqualTo(expected).Within(0.00001f));
        }

        [Test]
        public void Log_GetHash_IsNotArgumentHash1()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.Y;
            Expression expression = Expression.LnOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Log_GetHash_IsNotArgumentHash2()
        {
            // ARANGE

            // ACT
            Expression argument = 2;
            Expression expression = Expression.LnOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Log_GetHash_IsNotArgumentHash3()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.X + 1;
            Expression expression = Expression.LnOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = expression.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Log_GetHash_IsNotSignHash()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.X;
            Expression ln = Expression.LnOf(argument);
            Expression sign = Expression.SignOf(argument);
            int hash1 = ln.GetHashCode();
            int hash2 = sign.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Log_Map_DoesntChangeOriginal()
        {
            // ARANGE
            Expression expression1 = Expression.LnOf(Variable.X);
            Expression expression2 = Expression.LnOf(Variable.X);

            // ACT
            expression2.Map(a => Expression.LnOf(Variable.Y));

            // ASSERT
            Assert.AreEqual(expression1, expression2);
        }

        [Test]
        public void Log_Map_ReturnsAlternative()
        {
            // ARANGE
            Expression expression1 = Expression.LnOf(Variable.X);

            // ACT
            Expression expression2 = expression1.Map(a => Expression.LnOf(Variable.Y));

            // ASSERT
            Assert.AreEqual(Expression.LnOf(Variable.Y), expression2);
        }

        [Test]
        public void Log_Map_MapsChildren()
        {
            // ARANGE
            Expression expression1 = Expression.LnOf(Variable.X);

            // ACT
            Expression expression2 = expression1.Map(a => a is Variable ? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(Expression.LnOf(Variable.Z), expression2);
        }

        [Test]
        public void Log_Map_CanSkipSelf()
        {
            // ARANGE
            Expression expression1 = Expression.LnOf(Variable.X);
            ExpressionMapping mapping = new ExpressionMapping()
            {
                PostMap = a => Variable.Z,
                ShouldMapThis = a => !(a is Ln)
            };

            // ACT
            Expression expression2 = expression1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Expression.LnOf(Variable.Z), expression2);
        }

        [Test]
        public void Log_Map_CanSkipChildren()
        {
            // ARANGE
            Expression expression1 = Expression.LnOf(Variable.X);
            ExpressionMapping mapping = new ExpressionMapping()
            {
                PostMap = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression expression2 = expression1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Expression.LnOf(Variable.X), expression2);
        }
    }
}
