using Algebra;
using Algebra.PatternMatching;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraSystem.Test.EvaluatorsTests
{
    [Timeout(1000)]
    class PatternMatchingTests
    {
        [Test]
        public void AdditionStraightMatches([Values("X", "Y", "Z", "W", "V", "val", "t")] string variable1, [Values("X", "Y", "Z", "W", "V", "val", "t")] string variable2)
        {
            if (variable1 == variable2)
            {
                return;
            }

            // Arrange
            Expression expression = Expression.VariableFrom(variable1) + Expression.VariableFrom(variable2);
            Expression pattern = Expression.VarA;
            PatternMatchingResultSet expected = new PatternMatchingResultSet(new PatternMatchingResult("a", expression));

            // Act
            PatternMatchingResultSet resultSet = expression.Match(pattern);

            // Assert
            Assert.AreEqual(resultSet, expected);
        }
    }
}
