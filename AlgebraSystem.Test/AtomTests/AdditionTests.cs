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
        public void Addition_EvaluatesCorrectly([Range(-10, 10)] int a, [Range(-10, 10)] int b)
        {
            // ARANGE
            Expression expression = Constant.From(a) + Constant.From(b);

            // ACT
            float value = expression.GetDelegate(new VariableInputSet())();

            // ASSERT
            Assert.AreEqual(a + b, value);
        }

        [Test]
        public void Addition_Simplify_CollectsConstants()
        {
            // ARANGE

            // ACT
            Expression expression = Constant.From(54321) + Variable.Z + Constant.From(54321);
            Expression expected = Constant.From(54321 + 54321) + Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_CollectsCoefficients()
        {
            // ARANGE

            // ACT
            Expression expression = (Constant.From(54321) * Variable.Z) + (Constant.From(54321) * Variable.Z) + Variable.Z;
            Expression expected = Constant.From(54321 + 54321 + 1) * Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_RemovesZeros()
        {
            // ARANGE

            // ACT
            Expression expression = Constant.From(0) + Constant.From(1) + Constant.From(-1) + Variable.Z;
            Expression expected = Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_CancelsVaraibles()
        {
            // ARANGE

            // ACT
            Expression expression = Variable.Z - Variable.Z;
            Expression expected = Constant.From(0);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_CancelsVaraiblesPlusOne()
        {
            // ARANGE

            // ACT
            Expression expression = Constant.From(1) + Variable.Z - Variable.Z;
            Expression expected = Constant.From(1);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_RemovesVaraibleCoefficientIfOne()
        {
            // ARANGE

            // ACT
            Expression expression = Constant.From(1) + 2 * Variable.Z - Variable.Z;
            Expression expected = Constant.From(1) + Variable.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_CollatesAdditions()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.Add(new List<Expression>() { Expression.Add(new List<Expression>() { Variable.X, Variable.Y }), Variable.Z });
            Expression expected = Expression.Add(new List<Expression>() { Variable.X, Variable.Y, Variable.Z });

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_DoesNotFactorise()
        {
            // ARANGE

            // ACT
            Expression expression = (Expression.Pow(Variable.X, 2) + (3 * Variable.X) + 2) / (Variable.X + 1);
            Expression expected = Variable.X + 2;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Addition_GetOrderIndex_Is30()
        {
            // ARANGE

            // ACT
            Expression expression = Variable.X + 1;

            // ASSERT
            Assert.AreEqual(30, expression.GetOrderIndex());
        }

        [Test]
        public void Addition_PostMap_DoesntChangeOriginal()
        {
            // ARANGE
            Expression expression1 = Variable.X + 1;
            Expression expression2 = Variable.X + 1;

            // ACT
            expression2.PostMap(a => Variable.Y + 2);

            // ASSERT
            Assert.AreEqual(expression1, expression2);
        }

        [Test]
        public void Addition_PostMap_ReturnsAlternative()
        {
            // ARANGE
            Expression expression1 = Variable.X + 1;

            // ACT
            Expression expression2 = expression1.PostMap(a => Variable.Z);

            // ASSERT
            Assert.AreEqual(Variable.Z, expression2);
        }

        [Test]
        public void Addition_PostMap_MapsChildren()
        {
            // ARANGE
            Expression expression1 = Variable.X + Variable.Y;

            // ACT
            Expression expression2 = expression1.PostMap(a => a is Variable ? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(2 * Variable.Z, expression2);
        }

        [Test]
        public void Addition_PostMap_CanSkipSelf()
        {
            // ARANGE
            Expression expression1 = Variable.X + 1;
            ExpressionMapping mapping = new ExpressionMapping()
            {
                Map = a => Variable.Z,
                ShouldMapThis = a => !(a is Sum)
            };

            // ACT
            Expression expression2 = expression1.PostMap(mapping);

            // ASSERT
            Assert.AreEqual(2 * Variable.Z, expression2);
        }

        [Test]
        public void Addition_PostMap_CanSkipChildren()
        {
            // ARANGE
            Expression expression1 = Variable.X + 1;
            ExpressionMapping mapping = new ExpressionMapping()
            {
                Map = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression expression2 = expression1.PostMap(mapping);

            // ASSERT
            Assert.AreEqual(Variable.X + 1, expression2);
        }

        [Test]
        public void Addition_PostMap_MapsChildrenFirst()
        {
            // ARANGE
            Expression expression1 = Variable.X + 1;

            // ACT
            bool first = true;
            Expression expression2 = expression1.PostMap(eq => 
            { 
                first = false;
                return first ? Variable.X : eq;
            });

            // ASSERT
            Assert.AreEqual(Constant.From(12), expression2);
        }

        [Test]
        public void Addition_PreMap_DoesntChangeOriginal()
        {
            // ARANGE
            Expression expression1 = Variable.X + 1;
            Expression expression2 = Variable.X + 1;

            // ACT
            expression2.PreMap(a => Variable.Y + 2);

            // ASSERT
            Assert.AreEqual(expression1, expression2);
        }

        [Test]
        public void Addition_PreMap_ReturnsAlternative()
        {
            // ARANGE
            Expression expression1 = Variable.X + 1;

            // ACT
            Expression expression2 = expression1.PreMap(a => Variable.Z);

            // ASSERT
            Assert.AreEqual(Variable.Z, expression2);
        }

        [Test]
        public void Addition_PreMap_MapsChildren()
        {
            // ARANGE
            Expression expression1 = Variable.X + Variable.Y;

            // ACT
            Expression expression2 = expression1.PreMap(a => a is Variable ? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(2 * Variable.Z, expression2);
        }

        [Test]
        public void Addition_PreMap_CanSkipSelf()
        {
            // ARANGE
            Expression expression1 = Variable.X + 1;
            ExpressionMapping mapping = new ExpressionMapping()
            {
                Map = a => Variable.Z,
                ShouldMapThis = a => !(a is Sum)
            };

            // ACT
            Expression expression2 = expression1.PreMap(mapping);

            // ASSERT
            Assert.AreEqual(2 * Variable.Z, expression2);
        }

        [Test]
        public void Addition_PreMap_CanSkipChildren()
        {
            // ARANGE
            Expression expression1 = Variable.X + 1;
            ExpressionMapping mapping = new ExpressionMapping()
            {
                Map = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression expression2 = expression1.PreMap(mapping);

            // ASSERT
            Assert.AreEqual(Variable.X + 1, expression2);
        }

        [Test]
        public void Addition_PreMap_MapsParentFirst()
        {
            // ARANGE
            Expression expression1 = Variable.X + 1;

            // ACT
            Expression expression2 = expression1.PreMap(eq => eq is Sum s ? Expression.Multiply(s.Arguments) : 2);

            // ASSERT
            Assert.AreEqual(Variable.X, expression2);
        }

        [Test]
        public void Addition_Simplify_DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Addition_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Addition_Simplify_DoesntUseMap()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            Expression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.MapChildrenCalled);
        }
    }
}
