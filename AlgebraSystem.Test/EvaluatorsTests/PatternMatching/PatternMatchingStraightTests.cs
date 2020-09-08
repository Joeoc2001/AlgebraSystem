using Algebra;
using Algebra.PatternMatching;
using Libs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvaluatorsTests.PatternMatching
{
    class PatternMatchingStraightTests
    {
        [Test, TestCaseSource(typeof(StandardExpressions))]
        public void StraightMatches(Expression expression)
        {
            // Arrange
            Expression pattern = Expression.VarA;
            PatternMatchingResultSet expected = new PatternMatchingResultSet(new PatternMatchingResult("a", expression));

            // Act
            PatternMatchingResultSet resultSet = expression.Match(pattern);

            // Assert
            Assert.AreEqual(resultSet, expected);
        }
    }
}
