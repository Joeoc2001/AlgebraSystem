﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;

namespace AtomTests
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
        public void Constant_Derivative_IsZero([Range(-100, 100, 10)] int v)
        {
            // ARANGE
            Constant value = Constant.From(v);

            // ACT
            Expression derivative = value.GetDerivative(Variable.X);

            // ASSERT
            Assert.AreEqual(Constant.From(0), derivative);
        }

        [Test]
        public void Constant_EvaluatesCorrectly([Range(-100, 100, 10)] int v)
        {
            // ARANGE
            Constant expression = Constant.From(v);

            // ACT
            float value = expression.GetDelegate(new VariableInputSet())();

            // ASSERT
            Assert.AreEqual(v, value);
        }

        [Test]
        public void Constant_ParsesCorrectly([Range(-100, 100, 10)] int v)
        {
            // ARANGE

            // ACT
            Constant expression = Constant.From(v);

            // ASSERT
            Assert.AreEqual((Rational)v, expression.GetValue());
        }

        [Test]
        public void Constant_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression expression = 1;

            // ASSERT
            Assert.AreEqual(0, expression.GetOrderIndex());
        }

        [Test]
        public void Constant_PostMap_DoesntChangeOriginal()
        {
            // ARANGE
            Expression expression1 = 5;
            Expression expression2 = 5;

            // ACT
            expression2.PostMap(a => 2);

            // ASSERT
            Assert.AreEqual(expression1, expression2);
        }

        [Test]
        public void Constant_PostMap_ReturnsAlternative()
        {
            // ARANGE
            Expression expression1 = 5;

            // ACT
            Expression expression2 = expression1.PostMap(a => 2);

            // ASSERT
            Assert.AreEqual((Expression)2, expression2);
        }

        [Test]
        public void Constant_PreMap_DoesntChangeOriginal()
        {
            // ARANGE
            Expression expression1 = 5;
            Expression expression2 = 5;

            // ACT
            expression2.PreMap(a => 2);

            // ASSERT
            Assert.AreEqual(expression1, expression2);
        }

        [Test]
        public void Constant_PreMap_ReturnsAlternative()
        {
            // ARANGE
            Expression expression1 = 5;

            // ACT
            Expression expression2 = expression1.PreMap(a => 2);

            // ASSERT
            Assert.AreEqual((Expression)2, expression2);
        }
    }
}
