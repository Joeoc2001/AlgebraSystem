using System.Collections;
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
            Expression zero1 = 0;
            Expression zero2 = 0;

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
            Expression zero = 0;
            Expression one = 1;

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
            IExpression value = Expression.ConstantFrom(v);

            // ACT
            IExpression derivative = value.GetDerivative("x");

            // ASSERT
            Assert.AreEqual(Expression.ConstantFrom(0), derivative);
        }

        [Test]
        public void Constant_EvaluatesCorrectly([Range(-100, 100, 10)] int v)
        {
            // ARANGE
            IExpression expression = Expression.ConstantFrom(v);

            // ACT
            float value = expression.EvaluateOnce(new VariableInputSet<float>());

            // ASSERT
            Assert.AreEqual(v, value);
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
    }
}
