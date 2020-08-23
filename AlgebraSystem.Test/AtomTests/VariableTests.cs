using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;

namespace AtomTests
{
    public class VariableTests
    {
        [Test]
        public void Variable_IsSelfEqual([Values("X", "Y", "Z", "W", "V", "val", "t")] string name)
        {
            // ARANGE
            Expression v1 = Expression.VariableFrom(name);
            Expression v2 = Expression.VariableFrom(name);

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
        public void Variable_IsOtherNotEqual([Values("X", "Y", "Z", "W", "V", "val", "t")] string name1, [Values("X", "Y", "Z", "W", "V", "val", "t")] string name2)
        {
            if (name1.Equals(name2))
            {
                return;
            }

            // ARANGE
            Expression v1 = Expression.VariableFrom(name1);
            Expression v2 = Expression.VariableFrom(name2);

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
        public void Variable_Derivative_IsOne_WRTSelf([Values("X", "Y", "Z", "W", "V", "val", "t")] string name)
        {
            // ARANGE
            Expression v1 = Expression.VariableFrom(name);

            // ACT
            Expression derivative = v1.GetDerivative(name);

            // ASSERT
            Assert.AreEqual(Expression.ConstantFrom(1), derivative);
        }

        [Test]
        public void Variable_Derivative_IsZero_WRTOthers([Values("X", "Y", "Z", "W", "V", "val", "t")] string name1, [Values("X", "Y", "Z", "W", "V", "val", "t")] string name2)
        {
            if (name1.Equals(name2))
            {
                return;
            }

            // ARANGE
            Expression v1 = Expression.VariableFrom(name1);

            // ACT
            Expression derivative = v1.GetDerivative(name2);

            // ASSERT
            Assert.AreEqual(Expression.ConstantFrom(0), derivative);
        }

        [Test]
        public void Variable_EvaluatesCorrectly([Values("X", "Y", "Z", "W", "V", "val", "t")] string name, [Range(-100, 100)] int expected)
        {
            // ARANGE
            Expression v = Expression.VariableFrom(name);
            VariableInputSet<float> inputSet = new VariableInputSet<float>();
            inputSet.Set(name, expected);

            // ACT
            float value = v.EvaluateOnce(inputSet);

            // ASSERT
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void Variable_ThrowsIfNotPresent([Values("X", "Y", "Z", "W", "V", "val", "t")] string name, [Range(-10, 10)] int falseValue)
        {
            // ARANGE
            Expression v = Expression.VariableFrom(name);
            VariableInputSet<float> inputSet = new VariableInputSet<float>();
            inputSet.Set("q", falseValue);

            // ACT

            // ASSERT
            Assert.That(() => v.EvaluateOnce(inputSet), Throws.TypeOf<VariableNotPresentException>());
        }

        [Test]
        public void Variable_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression expression = Expression.X;

            // ASSERT
            Assert.AreEqual(0, expression.GetOrderIndex());
        }
    }
}
