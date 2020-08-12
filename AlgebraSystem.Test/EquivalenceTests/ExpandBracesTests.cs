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
            Expression eq = (Variable.X + 1) * (Variable.X - 1);
            List<Expression> expected = new List<Expression>()
            {
                (Variable.X * Variable.X) - 1
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
            Expression eq = (Variable.X + 1) * (Variable.X + 2);
            List<Expression> expected = new List<Expression>()
            {
                (Variable.X * Variable.X) + 3 * Variable.X + 2
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
            Expression eq = (Variable.X + 1) * (Variable.X + 2) * (Variable.X + 3);
            List<Expression> expected = new List<Expression>()
            {
                ((Variable.X * Variable.X) + 3 * Variable.X + 2) * (Variable.X + 3),
                ((Variable.X * Variable.X) + 4 * Variable.X + 3) * (Variable.X + 2),
                ((Variable.X * Variable.X) + 5 * Variable.X + 6) * (Variable.X + 1)
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
            Expression eq = (Variable.X + 1) * (Variable.X + 2) * (Variable.X + 3);
            Expression nonexpected = Variable.X * Variable.X * Variable.X
                + 6 * Variable.X * Variable.X
                + 11 * Variable.X
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
            Expression eq = ((Variable.X + 1) * (Variable.X + 2) + 1) * (Variable.X + 3);
            List<Expression> expected = new List<Expression>()
            {
                ((Variable.X * Variable.X) + 3 * Variable.X + 3) * (Variable.X + 3),
                (Variable.X + 1) * (Variable.X + 2) * Variable.X + (Variable.X + 1) * (Variable.X + 2) * 3 + Variable.X + 3
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
            Expression eq = (Variable.X + 1) * 3;

            // ACT
            List<Expression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Has.Count.EqualTo(0));
        }

        [Test]
        public void ExpandBraces_onAddition_DoesNothing()
        {
            // ARANGE
            Expression eq = Variable.X + 1;

            // ACT
            List<Expression> actual = ExpandBracesPath(eq);

            // ASSERT
            Assert.That(actual, Has.Count.EqualTo(0));
        }
    }
}
