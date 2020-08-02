using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Algebra;
using Algebra.Operations;
using Algebra.Parsing;

namespace OperationsTests
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
            Equation derivative = v1.GetDerivative(v1);

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
            Equation derivative = v1.GetDerivative(v2);

            // ASSERT
            Assert.AreEqual(Constant.From(0), derivative);
        }

        [Test]
        public void Variable_EvaluatesCorrectly([Values("X", "Y", "Z", "W", "V", "val", "t")] string name)
        {
            // ARANGE
            float expected = 167283;
            Variable v = new Variable(name);
            VariableInputSet inputSet = new VariableInputSet();
            inputSet.Set(name, expected);

            // ACT
            float value = v.GetExpression(inputSet)();

            // ASSERT
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void Variable_ThrowsIfNotPresent([Values("X", "Y", "Z", "W", "V", "val", "t")] string name)
        {
            // ARANGE
            float falseValue = -193742;
            Variable v = new Variable(name);
            VariableInputSet inputSet = new VariableInputSet();
            inputSet.Set("q", falseValue);

            // ACT

            // ASSERT
            Assert.That(() => v.GetExpression(inputSet), Throws.TypeOf<Variable.NotPresentException>());
        }

        [Test]
        public void Variable_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Equation equation = Variable.X;

            // ASSERT
            Assert.AreEqual(0, equation.GetOrderIndex());
        }

        [Test]
        public void Variable_Map_DoesntChangeOriginal()
        {
            // ARANGE
            Equation equation1 = Variable.X;
            Equation equation2 = Variable.X;

            // ACT
            equation2.Map(a => Variable.Y);

            // ASSERT
            Assert.AreEqual(equation1, equation2);
        }

        [Test]
        public void Variable_Map_ReturnsAlternative()
        {
            // ARANGE
            Equation equation1 = Variable.X;

            // ACT
            Equation equation2 = equation1.Map(a => Variable.Z);

            // ASSERT
            Assert.AreEqual(Variable.Z, equation2);
        }
    }
}
