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
            Variable v1 = new Variable(name);
            Variable v2 = new Variable(name);

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
            Variable v1 = new Variable(name1);
            Variable v2 = new Variable(name2);

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
            Variable v1 = new Variable(name);

            // ACT
            Expression derivative = v1.GetDerivative(v1);

            // ASSERT
            Assert.AreEqual(Constant.From(1), derivative);
        }

        [Test]
        public void Variable_Derivative_IsZero_WRTOthers([Values("X", "Y", "Z", "W", "V", "val", "t")] string name1, [Values("X", "Y", "Z", "W", "V", "val", "t")] string name2)
        {
            if (name1.Equals(name2))
            {
                return;
            }

            // ARANGE
            Variable v1 = new Variable(name1);
            Variable v2 = new Variable(name2);

            // ACT
            Expression derivative = v1.GetDerivative(v2);

            // ASSERT
            Assert.AreEqual(Constant.From(0), derivative);
        }

        [Test]
        public void Variable_EvaluatesCorrectly([Values("X", "Y", "Z", "W", "V", "val", "t")] string name, [Range(-100, 100)] int expected)
        {
            // ARANGE
            Variable v = new Variable(name);
            VariableInputSet inputSet = new VariableInputSet();
            inputSet.Set(name, expected);

            // ACT
            float value = v.GetDelegate(inputSet)();

            // ASSERT
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void Variable_ThrowsIfNotPresent([Values("X", "Y", "Z", "W", "V", "val", "t")] string name, [Range(-10, 10)] int falseValue)
        {
            // ARANGE
            Variable v = new Variable(name);
            VariableInputSet inputSet = new VariableInputSet();
            inputSet.Set("q", falseValue);

            // ACT

            // ASSERT
            Assert.That(() => v.GetDelegate(inputSet), Throws.TypeOf<Variable.NotPresentException>());
        }

        [Test]
        public void Variable_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression expression = Variable.X;

            // ASSERT
            Assert.AreEqual(0, expression.GetOrderIndex());
        }

        [Test]
        public void Variable_Map_DoesntChangeOriginal()
        {
            // ARANGE
            Expression expression1 = Variable.X;
            Expression expression2 = Variable.X;

            // ACT
            expression2.Map(a => Variable.Y);

            // ASSERT
            Assert.AreEqual(expression1, expression2);
        }

        [Test]
        public void Variable_Map_ReturnsAlternative()
        {
            // ARANGE
            Expression expression1 = Variable.X;

            // ACT
            Expression expression2 = expression1.Map(a => Variable.Z);

            // ASSERT
            Assert.AreEqual(Variable.Z, expression2);
        }
    }
}
