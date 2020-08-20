using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;
using AlgebraSystem.Test;

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
        public void Multiplication_EvaluatesCorrectly([Range(-10, 10)] int a, [Range(-10, 10)] int b)
        {
            // ARANGE
            Expression expression = Constant.From(a) * Constant.From(b);

            // ACT
            float value = expression.GetDelegate(new VariableInputSet())();

            // ASSERT
            Assert.AreEqual(a * b, value);
        }

        [Test]
        public void Multiplication_Simplify_CollectsConstants()
        {
            // ARANGE

            // ACT
            Expression expression = Constant.From(54321) * Variable.Z * Constant.From(54321);
            Expression expected = Constant.From(((Rational)54321) * 54321) * Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_CollectsPowers()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Pow(Variable.Z, 2) * Expression.Pow(Variable.Z, Variable.Y) * Variable.Z;
            Expression expected = Expression.Pow(Variable.Z, 3 + Variable.Y);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_RemovesOnes()
        {
            // ARANGE

            // ACT
            Expression expression = 1 * 2 * Variable.Z * (Rational)0.5M;
            Expression expected = Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_CancelsVaraibles()
        {
            // ARANGE

            // ACT
            Expression expression = Variable.Z * Expression.Pow(Variable.Z, -1);
            Expression expected = Constant.From(1);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_CancelsVaraiblesTimesTwo()
        {
            // ARANGE

            // ACT
            Expression expression = Constant.From(2) * Variable.Z * Expression.Pow(Variable.Z, -1);
            Expression expected = Constant.From(2);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_CollatesMultiplication()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Multiply(new List<Expression>() { Expression.Multiply(new List<Expression>() { Variable.X, Variable.Y }), Variable.Z });
            Expression expected = Expression.Multiply(new List<Expression>() { Variable.X, Variable.Y, Variable.Z });

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_GoesToConstantZero()
        {
            // ARANGE

            // ACT
            Expression expression = 0 * 2 * Variable.Z * (Rational)0.5M;
            Expression expected = 0;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Multiplication_DoesNotSimplify_DOTS()
        {
            // ARANGE

            // ACT
            Expression expression = (Variable.X + 1) * (Variable.X - 1);
            Expression expected = Expression.Pow(Variable.X, 2) - 1;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_DoesNotExpandBraces()
        {
            // ARANGE

            // ACT
            Expression expression = (Variable.X + 1) * (Variable.X + 2);
            Expression expected = Expression.Pow(Variable.X, 2) + 3 * Variable.X + 2;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Multiplication_Simplify_DoesNotDistribute()
        {
            // ARANGE

            // ACT
            Expression expression = 3 * (Variable.X + 1) - 3;
            Expression expected = 3 * Variable.X;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Multiplication_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression expression = 3 * Variable.X;

            // ASSERT
            Assert.AreEqual(20, expression.GetOrderIndex());
        }

        [Test]
        public void Multiplication_PostMap_DoesntChangeOriginal()
        {
            // ARANGE
            Expression expression1 = Variable.X * 2;
            Expression expression2 = Variable.X * 2;

            // ACT
            expression2.PostMap(a => Variable.Y * 2);

            // ASSERT
            Assert.AreEqual(expression1, expression2);
        }

        [Test]
        public void Multiplication_PostMap_ReturnsAlternative()
        {
            // ARANGE
            Expression expression1 = Variable.X * 2;

            // ACT
            Expression expression2 = expression1.PostMap(a => Variable.Z);

            // ASSERT
            Assert.AreEqual(Variable.Z, expression2);
        }

        [Test]
        public void Multiplication_PostMap_PostMapsChildren()
        {
            // ARANGE
            Expression expression1 = Variable.X * Variable.Y;

            // ACT
            Expression expression2 = expression1.PostMap(a => a is Variable ? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(Variable.Z * Variable.Z, expression2);
        }

        [Test]
        public void Multiplication_PostMap_CanSkipSelf()
        {
            // ARANGE
            Expression expression1 = Variable.X * Variable.Y;
            ExpressionMapping mapping = new ExpressionMapping()
            {
                Map = a => Variable.Z,
                ShouldMapThis = a => !(a is Product)
            };

            // ACT
            Expression expression2 = expression1.PostMap(mapping);

            // ASSERT
            Assert.AreEqual(Variable.Z * Variable.Z, expression2);
        }

        [Test]
        public void Multiplication_PostMap_CanSkipChildren()
        {
            // ARANGE
            Expression expression1 = Variable.X * Variable.Y;
            ExpressionMapping mapping = new ExpressionMapping()
            {
                Map = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression expression2 = expression1.PostMap(mapping);

            // ASSERT
            Assert.AreEqual(Variable.X * Variable.Y, expression2);
        }

        [Test]
        public void Multiplication_PostMap_MapsChildrenFirst()
        {
            // ARANGE
            Expression expression1 = Variable.X * 2;

            // ACT
            Expression expression2 = expression1.PostMap(eq => eq is Constant ? eq : Constant.From(4));

            // ASSERT
            Assert.AreEqual(Constant.From(8), expression2);
        }

        [Test]
        public void Multiplication_PreMap_DoesntChangeOriginal()
        {
            // ARANGE
            Expression expression1 = Variable.X * 2;
            Expression expression2 = Variable.X * 2;

            // ACT
            expression2.PreMap(a => Variable.Y);

            // ASSERT
            Assert.AreEqual(expression1, expression2);
        }

        [Test]
        public void Multiplication_PreMap_ReturnsAlternative()
        {
            // ARANGE
            Expression expression1 = Variable.X * 2;

            // ACT
            Expression expression2 = expression1.PreMap(a => Variable.Z);

            // ASSERT
            Assert.AreEqual(Variable.Z, expression2);
        }

        [Test]
        public void Multiplication_PreMap_PostMapsChildren()
        {
            // ARANGE
            Expression expression1 = Variable.X * Variable.Y;

            // ACT
            Expression expression2 = expression1.PreMap(a => a is Variable ? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(Variable.Z * Variable.Z, expression2);
        }

        [Test]
        public void Multiplication_PreMap_CanSkipSelf()
        {
            // ARANGE
            Expression expression1 = Variable.X * Variable.Y;
            ExpressionMapping mapping = new ExpressionMapping()
            {
                Map = a => Variable.Z,
                ShouldMapThis = a => !(a is Product)
            };

            // ACT
            Expression expression2 = expression1.PreMap(mapping);

            // ASSERT
            Assert.AreEqual(Variable.Z * Variable.Z, expression2);
        }

        [Test]
        public void Multiplication_PreMap_CanSkipChildren()
        {
            // ARANGE
            Expression expression1 = Variable.X * Variable.Y;
            ExpressionMapping mapping = new ExpressionMapping()
            {
                Map = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression expression2 = expression1.PreMap(mapping);

            // ASSERT
            Assert.AreEqual(Variable.X * Variable.Y, expression2);
        }

        [Test]
        public void Multiplication_PreMap_MapsParentFirst()
        {
            // ARANGE
            Expression expression1 = Variable.X * 2;

            // ACT
            Expression expression2 = expression1.PreMap(eq => eq is Constant ? eq : Constant.From(4));

            // ASSERT
            Assert.AreEqual(Constant.From(4), expression2);
        }

        [Test]
        public void Multiplication_Simplify_DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 * 2;

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Multiplication_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 * 2;

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Multiplication_Simplify_DoesntUseMap()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 * 2;

            // ASSERT
            Assert.IsFalse(dummy1.MapChildrenCalled);
        }
    }
}
