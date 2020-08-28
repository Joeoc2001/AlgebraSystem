using Algebra.Equivalence;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;

namespace EquivalenceTests
{
    public class ExpandBracesTests
    {
        private static readonly EquivalencePath ExpandBracesPath = EquivalencePaths.ExpandBraces;

        [Test]
        public void ExpandBraces_Expands_DOTS()
        {
            // ARANGE
            IExpression eq = (IExpression.X + 1) * (IExpression.X - 1);
            List<IExpression> expected = new List<IExpression>()
            {
                (IExpression.X * IExpression.X) - 1
            };

            // ACT
            List<IExpression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void ExpandBraces_Expands_Quadratic()
        {
            // ARANGE
            IExpression eq = (IExpression.X + 1) * (IExpression.X + 2);
            List<IExpression> expected = new List<IExpression>()
            {
                (IExpression.X * IExpression.X) + 3 * IExpression.X + 2
            };

            // ACT
            List<IExpression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void ExpandBraces_Expands_AllThreeOfCubic()
        {
            // ARANGE
            IExpression eq = (IExpression.X + 1) * (IExpression.X + 2) * (IExpression.X + 3);
            List<IExpression> expected = new List<IExpression>()
            {
                ((IExpression.X * IExpression.X) + 3 * IExpression.X + 2) * (IExpression.X + 3),
                ((IExpression.X * IExpression.X) + 4 * IExpression.X + 3) * (IExpression.X + 2),
                ((IExpression.X * IExpression.X) + 5 * IExpression.X + 6) * (IExpression.X + 1)
            };

            // ACT
            List<IExpression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void ExpandBraces_DoesntExpandAllOfCubicAtOnce()
        {
            // ARANGE
            IExpression eq = (IExpression.X + 1) * (IExpression.X + 2) * (IExpression.X + 3);
            IExpression nonexpected = IExpression.X * IExpression.X * IExpression.X
                + 6 * IExpression.X * IExpression.X
                + 11 * IExpression.X
                + 6;

            // ACT
            List<IExpression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.IsFalse(actual.Contains(nonexpected));
        }

        [Test]
        public void ExpandBraces_Expands_Nested()
        {
            // ARANGE
            IExpression eq = ((IExpression.X + 1) * (IExpression.X + 2) + 1) * (IExpression.X + 3);
            List<IExpression> expected = new List<IExpression>()
            {
                ((IExpression.X * IExpression.X) + 3 * IExpression.X + 3) * (IExpression.X + 3),
                (IExpression.X + 1) * (IExpression.X + 2) * IExpression.X + (IExpression.X + 1) * (IExpression.X + 2) * 3 + IExpression.X + 3
            };

            // ACT
            List<IExpression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void ExpandBraces_DoesntDistribute()
        {
            // ARANGE
            IExpression eq = (IExpression.X + 1) * 3;

            // ACT
            List<IExpression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Has.Count.EqualTo(0));
        }

        [Test]
        public void ExpandBraces_onAddition_DoesNothing()
        {
            // ARANGE
            IExpression eq = IExpression.X + 1;

            // ACT
            List<IExpression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Has.Count.EqualTo(0));
        }

        [Test]
        public void ExpandBraces_EquivalenceClass_ExpandsAllThreeOfCubic()
        {
            // ARANGE
            IExpression eq = (IExpression.X + 1) * (IExpression.X + 2) * (IExpression.X + 3);
            IExpression expected = IExpression.X * IExpression.X * IExpression.X
                + 6 * IExpression.X * IExpression.X
                + 11 * IExpression.X
                + 6;

            // ACT
            IEquivalenceClass equivalenceClass = eq.GetEquivalenceClass();
            bool contained = equivalenceClass.IsInClass(expected);

            // ASSERT
            Assert.IsTrue(contained);
        }

        [Test]
        public void ExpandBraces_EquivalenceClass_ExpandsTwoOfCubic()
        {
            // ARANGE
            IExpression eq = (IExpression.X + 1) * (IExpression.X + 2) * (IExpression.X + 3);
            IExpression expected = ((IExpression.X * IExpression.X) + 4 * IExpression.X + 3) * (IExpression.X + 2);

            // ACT
            IEquivalenceClass equivalenceClass = eq.GetEquivalenceClass();
            bool contained = equivalenceClass.IsInClass(expected);

            // ASSERT
            Assert.IsTrue(contained);
        }

        [Test]
        public void ExpandBraces_EquivalenceClass_IsFalseWhenExpansionIsntEqual()
        {
            // ARANGE
            IExpression eq = (IExpression.X + 1) * (IExpression.X + 2) * (IExpression.X + 3);
            IExpression notExpected = ((IExpression.X * IExpression.X) + 5 * IExpression.X + 3) * (IExpression.X + 2);

            // ACT
            IEquivalenceClass equivalenceClass = eq.GetEquivalenceClass();
            bool contained = equivalenceClass.IsInClass(notExpected);

            // ASSERT
            Assert.IsFalse(contained);
        }
    }
}
