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
            Expression value = Expression.VarX + Expression.VarY + Expression.VarZ;
            Expression expected = Expression.ConstantFrom(1);

            // ACT
            Expression derivative = value.GetDerivative(varName);

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void IsValidFor_XPlusY_WRTz()
        {
            // ARANGE
            Expression value = Expression.VarX + Expression.VarY;
            Expression expected = Expression.ConstantFrom(0);

            // ACT
            Expression derivative = value.GetDerivative("z");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }

        [Test]
        public void IsValidFor_XPlus1_WRTx()
        {
            // ARANGE
            Expression value = Expression.VarX + 1;
            Expression expected = Expression.ConstantFrom(1);

            // ACT
            Expression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(expected, derivative);
        }
    }
}
