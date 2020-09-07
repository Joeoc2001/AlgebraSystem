using Algebra.Equivalence;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;

namespace EquivalenceTests
{
    [Timeout(1000)]
    public class ExpandBracesTests
    {
        private static readonly EquivalencePath _expandBracesPath = EquivalencePaths.ExpandBraces;

        private bool AreInSameClass(Expression start, Expression end)
        {
            EquivalenceClass equivalence = start.GetEquivalenceClass();
            return equivalence.IsInClass(end, -1, new List<EquivalencePath>() { _expandBracesPath });
        }


        [Test]
        public void ExpandBraces_Expands_DOTS()
        {
            // ARANGE
            Expression eq = (Expression.VarX + 1) * (Expression.VarX - 1);
            Expression expected = (Expression.VarX * Expression.VarX) - 1;

            // ACT
            bool contained = AreInSameClass(eq, expected);

            // ASSERT
            Assert.IsTrue(contained);
        }

        [Test]
        public void ExpandBraces_Expands_Quadratic()
        {
            // ARANGE
            Expression eq = (Expression.VarX + 1) * (Expression.VarX + 2);
            Expression expected = (Expression.VarX * Expression.VarX) + 3 * Expression.VarX + 2;

            // ACT
            bool contained = AreInSameClass(eq, expected);

            // ASSERT
            Assert.IsTrue(contained);
        }

        [Test]
        public void ExpandBraces_Expands_AllThreePairsOfCubic([Range(0, 2)] int index)
        {
            // ARANGE
            Expression eq = (Expression.VarX + 1) * (Expression.VarX + 2) * (Expression.VarX + 3);
            List<Expression> expected = new List<Expression>()
            {
                ((Expression.VarX * Expression.VarX) + 3 * Expression.VarX + 2) * (Expression.VarX + 3),
                ((Expression.VarX * Expression.VarX) + 4 * Expression.VarX + 3) * (Expression.VarX + 2),
                ((Expression.VarX * Expression.VarX) + 5 * Expression.VarX + 6) * (Expression.VarX + 1)
            };

            // ACT
            bool contained = AreInSameClass(eq, expected[index]);

            // ASSERT
            Assert.IsTrue(contained);
        }

        [Test]
        public void ExpandBraces_Expands_AllOfCubic()
        {
            // ARANGE
            Expression eq = (Expression.VarX + 1) * (Expression.VarX + 2) * (Expression.VarX + 3);
            Expression expected = Expression.VarX * Expression.VarX * Expression.VarX
                + 6 * Expression.VarX * Expression.VarX
                + 11 * Expression.VarX
                + 6;

            // ACT
            bool contained = AreInSameClass(eq, expected);

            // ASSERT
            Assert.IsTrue(contained);
        }

        [Test]
        public void ExpandBraces_Expands_Nested([Range(0, 1)] int index)
        {
            // ARANGE
            Expression eq = ((Expression.VarX + 1) * (Expression.VarX + 2) + 1) * (Expression.VarX + 3);
            List<Expression> expected = new List<Expression>()
            {
                ((Expression.VarX * Expression.VarX) + 3 * Expression.VarX + 3) * (Expression.VarX + 3),
                (Expression.VarX + 1) * (Expression.VarX + 2) * Expression.VarX + (Expression.VarX + 1) * (Expression.VarX + 2) * 3 + Expression.VarX + 3
            };

            // ACT
            bool contained = AreInSameClass(eq, expected[index]);

            // ASSERT
            Assert.IsTrue(contained);
        }

        [Test]
        public void ExpandBraces_Distributes()
        {
            // ARANGE
            Expression eq = (Expression.VarX + 1) * 3;
            Expression expected = Expression.VarX * 3 + 3;

            // ACT
            bool contained = AreInSameClass(eq, expected);

            // ASSERT
            Assert.IsTrue(contained);
        }

        [Test]
        public void ExpandBraces_onAddition_DoesNothing()
        {
            // ARANGE
            Expression eq = Expression.VarX + 1;

            // ACT
            List<Expression> actual = new List<Expression>(_expandBracesPath.GetAllFrom(eq));

            // ASSERT
            Assert.That(actual, Has.Count.EqualTo(0));
        }

        [Test]
        public void ExpandBraces_EquivalenceClass_IsFalseWhenExpansionIsntEqual()
        {
            // ARANGE
            Expression eq = (Expression.VarX + 1) * (Expression.VarX + 2) * (Expression.VarX + 3);
            Expression notExpected = ((Expression.VarX * Expression.VarX) + 5 * Expression.VarX + 3) * (Expression.VarX + 2);

            // ACT
            bool contained = AreInSameClass(eq, notExpected);

            // ASSERT
            Assert.IsFalse(contained);
        }
    }
}
