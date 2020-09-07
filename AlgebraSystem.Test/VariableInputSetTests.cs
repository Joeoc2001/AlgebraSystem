using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;
using System.Numerics;

namespace AlgebraTests
{
    public class VariableInputSetTests
    {
        [Test]
        public void VariableSet_EmptyConstructor_HasNoVariables([Values("X", "Y", "Z", "W", "V", "val", "t")] string name)
        {
            // ARANGE
            VariableInputSet<float> set = new VariableInputSet<float>();

            // ACT

            // ASSERT
            Assert.IsFalse(set.Contains(name));
        }

        [Test]
        public void VariableSet_EmptySets_AreEqual()
        {
            // ARANGE

            // ACT
            VariableInputSet<float> set1 = new VariableInputSet<float>();
            VariableInputSet<float> set2 = new VariableInputSet<float>();

            // ASSERT
            Assert.IsTrue(set1.Equals(set2));
            Assert.IsTrue(set2.Equals(set1));
            Assert.IsTrue(set1.Equals((object)set2));
            Assert.IsTrue(set2.Equals((object)set1));
            Assert.IsTrue(set1 == set2);
            Assert.IsTrue(set2 == set1);
            Assert.IsFalse(set1 != set2);
            Assert.IsFalse(set2 != set1);
        }

        [Test]
        public void VariableSet_DifferentSets_AreDifferent()
        {
            // ARANGE

            // ACT
            VariableInputSet<float> set1 = new VariableInputSet<float>();
            set1.Set("x", 1);
            VariableInputSet<float> set2 = new VariableInputSet<float>();
            set2.Set("x", 1.00001f);

            // ASSERT
            Assert.IsFalse(set1.Equals(set2));
            Assert.IsFalse(set2.Equals(set1));
            Assert.IsFalse(set1.Equals((object)set2));
            Assert.IsFalse(set2.Equals((object)set1));
            Assert.IsFalse(set1 == set2);
            Assert.IsFalse(set2 == set1);
            Assert.IsTrue(set1 != set2);
            Assert.IsTrue(set2 != set1);
        }

        [Test]
        public void VariableSet_CollectionInitilizationNotation_SetsValuesTo0()
        {
            // ARANGE
            VariableInputSet<float> set = new VariableInputSet<float>()
            {
                "X",
                "Y"
            };

            // ACT

            // ASSERT
            Assert.IsTrue(set.Contains("X"));
            Assert.IsTrue(set.Contains("Y"));
            Assert.AreEqual(0, set["X"].Value);
            Assert.AreEqual(0, set["Y"].Value);
        }
    }
}
