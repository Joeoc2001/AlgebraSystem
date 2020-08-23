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
        public void Addition_Simplify_CollectsConstants()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.ConstantFrom(54321) + IExpression.Z + IExpression.ConstantFrom(54321);
            IExpression expected = IExpression.ConstantFrom(54321 + 54321) + IExpression.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_CollectsCoefficients()
        {
            // ARANGE

            // ACT
            IExpression expression = (IExpression.ConstantFrom(54321) * IExpression.Z) + (IExpression.ConstantFrom(54321) * IExpression.Z) + IExpression.Z;
            IExpression expected = IExpression.ConstantFrom(54321 + 54321 + 1) * IExpression.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_RemovesZeros()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.ConstantFrom(0) + IExpression.ConstantFrom(1) + IExpression.ConstantFrom(-1) + IExpression.Z;
            IExpression expected = IExpression.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_CancelsVaraibles()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.Z - IExpression.Z;
            IExpression expected = IExpression.ConstantFrom(0);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_CancelsVaraiblesPlusOne()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.ConstantFrom(1) + IExpression.Z - IExpression.Z;
            IExpression expected = IExpression.ConstantFrom(1);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_RemovesVaraibleCoefficientIfOne()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.ConstantFrom(1) + 2 * IExpression.Z - IExpression.Z;
            IExpression expected = IExpression.ConstantFrom(1) + IExpression.Z;

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_CollatesAdditions()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.Add(new List<IExpression>() { IExpression.Add(new List<IExpression>() { IExpression.X, IExpression.Y }), IExpression.Z });
            IExpression expected = IExpression.Add(new List<IExpression>() { IExpression.X, IExpression.Y, IExpression.Z });

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void Addition_Simplify_DoesNotFactorise()
        {
            // ARANGE

            // ACT
            IExpression expression = (IExpression.Pow(IExpression.X, 2) + (3 * IExpression.X) + 2) / (IExpression.X + 1);
            IExpression expected = IExpression.X + 2;

            // ASSERT
            Assert.AreNotEqual(expected, expression);
        }

        [Test]
        public void Addition_GetOrderIndex_Is30()
        {
            // ARANGE

            // ACT
            IExpression expression = IExpression.X + 1;

            // ASSERT
            Assert.AreEqual(30, expression.GetOrderIndex());
        }

        [Test]
        public void Addition_Simplify_DoesntUseAtomicForm()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.GenAtomicExpressionCalled);
        }

        [Test]
        public void Addition_Simplify_DoesntUseToString()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.ToStringCalled);
        }

        [Test]
        public void Addition_Simplify_DoesntUseMap()
        {
            // ARANGE
            DummyExpression dummy1 = new DummyExpression();

            // ACT
            IExpression _ = dummy1 + 1;

            // ASSERT
            Assert.IsFalse(dummy1.MapChildrenCalled);
        }
    }
}
