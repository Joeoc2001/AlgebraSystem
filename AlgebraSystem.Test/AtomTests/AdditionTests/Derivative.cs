using Algebra;
using Algebra.Atoms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraSystem.Test.AtomTests.AdditionTests
{
    class Derivative
    {
        [Test]
        public void IsValidFor_XPlusYPlusZ_WRT([Values("x", "y", "z")] string varName)
        {
            // ARANGE
            IExpression value = Expression.VarX + Expression.VarY + Expression.VarZ;
            IExpression expected = Expression.ConstantFrom(1);

            // ACT
            IExpression derivative = value.GetDerivative(varName);

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void IsValidFor_XPlusY_WRTz()
        {
            // ARANGE
            IExpression value = Expression.VarX + Expression.VarY;
            IExpression expected = Expression.ConstantFrom(0);

            // ACT
            IExpression derivative = value.GetDerivative("z");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void IsValidFor_XPlus1_WRTx()
        {
            // ARANGE
            IExpression value = Expression.VarX + 1;
            IExpression expected = Expression.ConstantFrom(1);

            // ACT
            IExpression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }
    }
}
