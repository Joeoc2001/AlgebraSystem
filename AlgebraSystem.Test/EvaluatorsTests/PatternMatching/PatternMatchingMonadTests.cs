using Algebra;
using Algebra.PatternMatching;
using AlgebraSystem.Test.Libs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraSystem.Test.EvaluatorsTests.PatternMatching
{
    class PatternMatchingMonadTests
    {
        private static readonly Expression[] arguments = new Expression[]
        {
            Expression.VarA,
            Expression.VarB * 5,
            3 * Expression.VarX - 10
        };

        [Test]
        public void Matches([Values] MonadBuilder.Monad monad, [ValueSource(nameof(arguments))] Expression argument)
        {
            // Arrange
            Expression expression = MonadBuilder.Build(argument, monad);
            Expression pattern = MonadBuilder.Build(Expression.VarA, monad);
            PatternMatchingResultSet expected = new PatternMatchingResultSet(new PatternMatchingResult("a", argument));

            // Act
            PatternMatchingResultSet resultSet = expression.Match(pattern);

            // Assert
            Assert.AreEqual(resultSet, expected);
        }

        [Test]
        public void DoesNotMatch([Values] MonadBuilder.Monad monad1, [Values] MonadBuilder.Monad monad2, [ValueSource(nameof(arguments))] Expression argument)
        {
            if (monad1 == monad2)
            {
                return;
            }

            // Arrange
            Expression expression = MonadBuilder.Build(argument, monad1);
            Expression pattern = MonadBuilder.Build(Expression.VarA, monad2);
            PatternMatchingResultSet expected = PatternMatchingResultSet.None;

            // Act
            PatternMatchingResultSet resultSet = expression.Match(pattern);

            // Assert
            Assert.AreEqual(resultSet, expected);
            Assert.IsTrue(resultSet.IsNone);
        }
    }
}
