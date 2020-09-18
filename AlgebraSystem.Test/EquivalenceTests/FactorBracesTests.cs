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
    public class FactorBracesTests
    {
        private static readonly EquivalencePath _factorBracesPath = EquivalencePaths.FactorBraces;

        private bool AreInSameClass(Expression start, Expression end)
        {
            EquivalenceClass equivalence = start.GetEquivalenceClass();
            return equivalence.IsInClass(end, equivalencies: new List<EquivalencePath>() { _factorBracesPath });
        }

        [Test]
        public void FactorBraces_EquivalenceClass_IsTrueOnSimpleCase()
        {
            // ARANGE
            Expression eq = (Expression.VarX * Expression.VarY) + (Expression.VarX * 2);
            Expression notExpected = Expression.VarX * (Expression.VarY + 2);

            // ACT
            bool contained = AreInSameClass(eq, notExpected);

            // ASSERT
            Assert.IsTrue(contained);
        }

        [Test]
        public void FactorBraces_onAddition_DoesNothing()
        {
            // ARANGE
            Expression eq = Expression.VarX + 1;

            // ACT
            List<Expression> actual = new List<Expression>(_factorBracesPath.GetAllFrom(eq));

            // ASSERT
            Assert.That(actual, Has.Count.EqualTo(0));
        }

        [Test]
        public void FactorBraces_EquivalenceClass_IsFalseWhenFactoringIsntEqual()
        {
            // ARANGE
            Expression eq = (Expression.VarX * Expression.VarY) + (Expression.VarX * 2);
            Expression notExpected = Expression.VarX * (Expression.VarY + 3);

            // ACT
            bool contained = AreInSameClass(eq, notExpected);

            // ASSERT
            Assert.IsFalse(contained);
        }
    }
}
