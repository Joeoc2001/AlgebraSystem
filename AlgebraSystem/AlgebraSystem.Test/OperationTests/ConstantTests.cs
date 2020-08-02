﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Operations;
using Algebra.Parsing;

namespace OperationsTests
{
    public class ConstantTests
    {
        [Test]
        public void Constant_Zero_IsSelfEqual()
        {
            // ARANGE
            Constant zero1 = 0;
            Constant zero2 = 0;

            // ACT

            // ASSERT
            Assert.IsTrue(zero1.Equals(zero2));
            Assert.IsTrue(zero2.Equals(zero1));
            Assert.IsTrue(zero1.Equals((object)zero2));
            Assert.IsTrue(zero2.Equals((object)zero1));
            Assert.IsTrue(zero1 == zero2);
            Assert.IsTrue(zero2 == zero1);
            Assert.IsFalse(zero1 != zero2);
            Assert.IsFalse(zero2 != zero1);
        }

        [Test]
        public void Constant_ZeroAndOne_AreNotEqual()
        {
            // ARANGE
            Constant zero = 0;
            Constant one = 1;

            // ACT

            // ASSERT
            Assert.IsFalse(zero.Equals(one));
            Assert.IsFalse(one.Equals(zero));
            Assert.IsFalse(zero.Equals((object)one));
            Assert.IsFalse(one.Equals((object)zero));
            Assert.IsFalse(zero == one);
            Assert.IsFalse(one == zero);
            Assert.IsTrue(zero != one);
            Assert.IsTrue(one != zero);
        }

        [Test]
        public void Constant_Derivative_IsZero()
        {
            // ARANGE
            Constant value = Constant.From(54321);

            // ACT
            Equation derivative = value.GetDerivative(Variable.X);

            // ASSERT
            Assert.AreEqual(Constant.From(0), derivative);
        }

        [Test]
        public void Constant_EvaluatesCorrectly()
        {
            // ARANGE
            Constant equation = Constant.From(54321);

            // ACT
            float value = equation.GetExpression()(new VariableSet());

            // ASSERT
            Assert.AreEqual(54321, value);
        }

        [Test]
        public void Constant_ParsesCorrectly()
        {
            // ARANGE

            // ACT
            Constant equation = Constant.From(54321);

            // ASSERT
            Assert.AreEqual((Rational)54321, equation.GetValue());
        }

        [Test]
        public void Constant_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Equation equation = 1;

            // ASSERT
            Assert.AreEqual(0, equation.GetOrderIndex());
        }

        [Test]
        public void Constant_Map_DoesntChangeOriginal()
        {
            // ARANGE
            Equation equation1 = 5;
            Equation equation2 = 5;

            // ACT
            equation2.Map(a => 2);

            // ASSERT
            Assert.AreEqual(equation1, equation2);
        }

        [Test]
        public void Constant_Map_ReturnsAlternative()
        {
            // ARANGE
            Equation equation1 = 5;

            // ACT
            Equation equation2 = equation1.Map(a => 2);

            // ASSERT
            Assert.AreEqual((Equation)2, equation2);
        }
    }
}
