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
        private static readonly EquivalencePath _expandBracesPath = EquivalencePaths.ExpandBraces;

        [Test]
        public void ExpandBraces_Expands_DOTS()
        {
            // ARANGE
            IExpression eq = (Expression.VarX + 1) * (Expression.VarX - 1);
            List<IExpression> expected = new List<IExpression>()
            {
                (Expression.VarX * Expression.VarX) - 1
            };

            // ACT
            List<IExpression> actual = new List<IExpression>(_expandBracesPath.GetAllFrom(eq));

            // ASSERT
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void ExpandBraces_Expands_Quadratic()
        {
            // ARANGE
            IExpression eq = (Expression.VarX + 1) * (Expression.VarX + 2);
            List<IExpression> expected = new List<IExpression>()
            {
                (Expression.VarX * Expression.VarX) + 3 * Expression.VarX + 2
            };

            // ACT
            List<IExpression> actual = new List<IExpression>(_expandBracesPath.GetAllFrom(eq));

            // ASSERT
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void ExpandBraces_Expands_AllThreeOfCubic()
        {
            // ARANGE
            IExpression eq = (Expression.VarX + 1) * (Expression.VarX + 2) * (Expression.VarX + 3);
            List<IExpression> expected = new List<IExpression>()
            {
                ((Expression.VarX * Expression.VarX) + 3 * Expression.VarX + 2) * (Expression.VarX + 3),
                ((Expression.VarX * Expression.VarX) + 4 * Expression.VarX + 3) * (Expression.VarX + 2),
                ((Expression.VarX * Expression.VarX) + 5 * Expression.VarX + 6) * (Expression.VarX + 1)
            };

            // ACT
            List<IExpression> actual = new List<IExpression>(_expandBracesPath.GetAllFrom(eq));

            // ASSERT
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void ExpandBraces_DoesntExpandAllOfCubicAtOnce()
        {
            // ARANGE
            IExpression eq = (Expression.VarX + 1) * (Expression.VarX + 2) * (Expression.VarX + 3);
            IExpression nonexpected = Expression.VarX * Expression.VarX * Expression.VarX
                + 6 * Expression.VarX * Expression.VarX
                + 11 * Expression.VarX
                + 6;

            // ACT
            List<IExpression> actual = new List<IExpression>(_expandBracesPath.GetAllFrom(eq));

            // ASSERT
            Assert.IsFalse(actual.Contains(nonexpected));
        }

        [Test]
        public void ExpandBraces_Expands_Nested()
        {
            // ARANGE
            IExpression eq = ((Expression.VarX + 1) * (Expression.VarX + 2) + 1) * (Expression.VarX + 3);
            List<IExpression> expected = new List<IExpression>()
            {
                ((Expression.VarX * Expression.VarX) + 3 * Expression.VarX + 3) * (Expression.VarX + 3),
                (Expression.VarX + 1) * (Expression.VarX + 2) * Expression.VarX + (Expression.VarX + 1) * (Expression.VarX + 2) * 3 + Expression.VarX + 3
            };

            // ACT
            List<IExpression> actual = new List<IExpression>(_expandBracesPath.GetAllFrom(eq));

            // ASSERT
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void ExpandBraces_DoesntDistribute()
        {
            // ARANGE
            IExpression eq = (Expression.VarX + 1) * 3;

            // ACT
            List<IExpression> actual = new List<IExpression>(_expandBracesPath.GetAllFrom(eq));

            // ASSERT
            Assert.That(actual, Has.Count.EqualTo(0));
        }

        [Test]
        public void ExpandBraces_onAddition_DoesNothing()
        {
            // ARANGE
            IExpression eq = Expression.VarX + 1;

            // ACT
            List<IExpression> actual = new List<IExpression>(_expandBracesPath.GetAllFrom(eq));

            // ASSERT
            Assert.That(actual, Has.Count.EqualTo(0));
        }

        [Test]
        public void ExpandBraces_EquivalenceClass_ExpandsAllThreeOfCubic()
        {
            // ARANGE
            IExpression eq = (Expression.VarX + 1) * (Expression.VarX + 2) * (Expression.VarX + 3);
            IExpression expected = Expression.VarX * Expression.VarX * Expression.VarX
                + 6 * Expression.VarX * Expression.VarX
                + 11 * Expression.VarX
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
            IExpression eq = (Expression.VarX + 1) * (Expression.VarX + 2) * (Expression.VarX + 3);
            IExpression expected = ((Expression.VarX * Expression.VarX) + 4 * Expression.VarX + 3) * (Expression.VarX + 2);

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
            IExpression eq = (Expression.VarX + 1) * (Expression.VarX + 2) * (Expression.VarX + 3);
            IExpression notExpected = ((Expression.VarX * Expression.VarX) + 5 * Expression.VarX + 3) * (Expression.VarX + 2);

            // ACT
            IEquivalenceClass equivalenceClass = eq.GetEquivalenceClass();
            bool contained = equivalenceClass.IsInClass(notExpected);

            // ASSERT
            Assert.IsFalse(contained);
        }
    }
}
