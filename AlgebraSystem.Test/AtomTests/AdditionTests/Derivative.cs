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
            IExpression value = IExpression.X + IExpression.Y + IExpression.Z;
            IExpression expected = IExpression.ConstantFrom(1);

            // ACT
            IExpression derivative = value.GetDerivative(varName);

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void IsValidFor_XPlusY_WRTz()
        {
            // ARANGE
            IExpression value = IExpression.X + IExpression.Y;
            IExpression expected = IExpression.ConstantFrom(0);

            // ACT
            IExpression derivative = value.GetDerivative("z");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void IsValidFor_XPlus1_WRTx()
        {
            // ARANGE
            IExpression value = IExpression.X + 1;
            IExpression expected = IExpression.ConstantFrom(1);

            // ACT
            IExpression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }
    }
}
