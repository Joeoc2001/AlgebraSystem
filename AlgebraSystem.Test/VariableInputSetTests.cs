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
            VariableInputSet set = new VariableInputSet();

            // ACT

            // ASSERT
            Assert.IsFalse(set.Contains(name));
        }

        [Test]
        public void VariableSet_SetVector2_SetsXY([Values("X", "Y", "Z", "W", "V", "val", "t")] string name)
        {
            // ARANGE
            VariableInputSet set = new VariableInputSet();
            set.Set(new Vector2(27f, -17f));

            // ACT

            // ASSERT
            switch (name)
            {
                case "X":
                    Assert.IsTrue(set.Contains(name));
                    Assert.AreEqual(27f, set[name].Value);
                    break;
                case "Y":
                    Assert.IsTrue(set.Contains(name));
                    Assert.AreEqual(-17f, set[name].Value);
                    break;
                default:
                    Assert.IsFalse(set.Contains(name));
                    break;
            }
        }

        [Test]
        public void VariableSet_SetVector3_SetsXYZ([Values("X", "Y", "Z", "W", "V", "val", "t")] string name)
        {
            // ARANGE
            VariableInputSet set = new VariableInputSet();
            set.Set(new Vector3(163f, 27f, -17f));

            // ACT

            // ASSERT
            switch (name)
            {
                case "X":
                    Assert.IsTrue(set.Contains(name));
                    Assert.AreEqual(163f, set[name].Value);
                    break;
                case "Y":
                    Assert.IsTrue(set.Contains(name));
                    Assert.AreEqual(27f, set[name].Value);
                    break;
                case "Z":
                    Assert.IsTrue(set.Contains(name));
                    Assert.AreEqual(-17f, set[name].Value);
                    break;
                default:
                    Assert.IsFalse(set.Contains(name));
                    break;
            }
        }

        [Test]
        public void VariableSet_SetVector4_SetsXYZW([Values("X", "Y", "Z", "W", "V", "val", "t")] string name)
        {
            // ARANGE
            VariableInputSet set = new VariableInputSet();
            set.Set(new Vector4(163f, -0.5f, 27f, -17f));

            // ACT

            // ASSERT
            switch (name)
            {
                case "X":
                    Assert.IsTrue(set.Contains(name));
                    Assert.AreEqual(163f, set[name].Value);
                    break;
                case "Y":
                    Assert.IsTrue(set.Contains(name));
                    Assert.AreEqual(-0.5f, set[name].Value);
                    break;
                case "Z":
                    Assert.IsTrue(set.Contains(name));
                    Assert.AreEqual(27f, set[name].Value);
                    break;
                case "W":
                    Assert.IsTrue(set.Contains(name));
                    Assert.AreEqual(-17f, set[name].Value);
                    break;
                default:
                    Assert.IsFalse(set.Contains(name));
                    break;
            }
        }

        [Test]
        public void VariableSet_EmptySets_AreEqual()
        {
            // ARANGE

            // ACT
            VariableInputSet set1 = new VariableInputSet();
            VariableInputSet set2 = new VariableInputSet();

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
            VariableInputSet set1 = new VariableInputSet();
            set1.Set(new Vector3(5, 2, 1));
            VariableInputSet set2 = new VariableInputSet();
            set2.Set(new Vector3(5, 2, 1.00001f));

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
            VariableInputSet set = new VariableInputSet()
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
