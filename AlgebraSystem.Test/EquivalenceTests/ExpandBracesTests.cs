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
        private static readonly EquivalencePath ExpandBracesPath = EquivalencePaths.EXPAND_BRACES;

        [Test]
        public void ExpandBraces_Expands_DOTS()
        {
            // ARANGE
            Expression eq = (Expression.X + 1) * (Expression.X - 1);
            List<Expression> expected = new List<Expression>()
            {
                (Expression.X * Expression.X) - 1
            };

            // ACT
            List<Expression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void ExpandBraces_Expands_Quadratic()
        {
            // ARANGE
            Expression eq = (Expression.X + 1) * (Expression.X + 2);
            List<Expression> expected = new List<Expression>()
            {
                (Expression.X * Expression.X) + 3 * Expression.X + 2
            };

            // ACT
            List<Expression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void ExpandBraces_Expands_AllThreeOfCubic()
        {
            // ARANGE
            Expression eq = (Expression.X + 1) * (Expression.X + 2) * (Expression.X + 3);
            List<Expression> expected = new List<Expression>()
            {
                ((Expression.X * Expression.X) + 3 * Expression.X + 2) * (Expression.X + 3),
                ((Expression.X * Expression.X) + 4 * Expression.X + 3) * (Expression.X + 2),
                ((Expression.X * Expression.X) + 5 * Expression.X + 6) * (Expression.X + 1)
            };

            // ACT
            List<Expression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void ExpandBraces_DoesntExpandAllOfCubicAtOnce()
        {
            // ARANGE
            Expression eq = (Expression.X + 1) * (Expression.X + 2) * (Expression.X + 3);
            Expression nonexpected = Expression.X * Expression.X * Expression.X
                + 6 * Expression.X * Expression.X
                + 11 * Expression.X
                + 6;

            // ACT
            List<Expression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.IsFalse(actual.Contains(nonexpected));
        }

        [Test]
        public void ExpandBraces_Expands_Nested()
        {
            // ARANGE
            Expression eq = ((Expression.X + 1) * (Expression.X + 2) + 1) * (Expression.X + 3);
            List<Expression> expected = new List<Expression>()
            {
                ((Expression.X * Expression.X) + 3 * Expression.X + 3) * (Expression.X + 3),
                (Expression.X + 1) * (Expression.X + 2) * Expression.X + (Expression.X + 1) * (Expression.X + 2) * 3 + Expression.X + 3
            };

            // ACT
            List<Expression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void ExpandBraces_DoesntDistribute()
        {
            // ARANGE
            Expression eq = (Expression.X + 1) * 3;

            // ACT
            List<Expression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Has.Count.EqualTo(0));
        }

        [Test]
        public void ExpandBraces_onAddition_DoesNothing()
        {
            // ARANGE
            Expression eq = Expression.X + 1;

            // ACT
            List<Expression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Has.Count.EqualTo(0));
        }

        [Test]
        public void ExpandBraces_EquivalenceClass_ExpandsAllThreeOfCubic()
        {
            // ARANGE
            Expression eq = (Expression.X + 1) * (Expression.X + 2) * (Expression.X + 3);
            Expression expected = Expression.X * Expression.X * Expression.X
                + 6 * Expression.X * Expression.X
                + 11 * Expression.X
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
            Expression eq = (Expression.X + 1) * (Expression.X + 2) * (Expression.X + 3);
            Expression expected = ((Expression.X * Expression.X) + 4 * Expression.X + 3) * (Expression.X + 2);

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
            Expression eq = (Expression.X + 1) * (Expression.X + 2) * (Expression.X + 3);
            Expression notExpected = ((Expression.X * Expression.X) + 5 * Expression.X + 3) * (Expression.X + 2);

            // ACT
            IEquivalenceClass equivalenceClass = eq.GetEquivalenceClass();
            bool contained = equivalenceClass.IsInClass(notExpected);

            // ASSERT
            Assert.IsFalse(contained);
        }
    }
}
