using Algebra;
using Algebra.PatternMatching;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraSystem.Test.EvaluatorsTests.Replacement.SpecificCases
{
    class PowerSplits
    {
        static readonly Expression _expression = Expression.From("x ^ 3.5");
        static readonly Expression _pattern = Expression.From("a ^ 2");
        static readonly Expression _replacement = Expression.From("3");
        static readonly ReplaceEvaluator _replaceEvaluator = new ReplaceEvaluator(_pattern, _replacement);

        [Test]
        public void TestThat_Replacement_CanSplitPower()
        {
            // Arrange

            // Act
            List<Expression> resultSet = new List<Expression>(_expression.Evaluate(_replaceEvaluator));

            // Assert
            Assert.That(resultSet, Contains.Item(Expression.From("3 * (x ^ 1.5)")));
        }

        [Test]
        public void TestThat_Replacement_CorrectCountOfSplitPower()
        {
            // Arrange

            // Act
            List<Expression> resultSet = new List<Expression>(_expression.Evaluate(_replaceEvaluator));

            // Assert
            Assert.That(resultSet, Has.Count.EqualTo(1));
        }
    }
}
