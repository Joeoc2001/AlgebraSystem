using Algebra;
using Algebra.PatternMatching;
using AlgebraSystem.Test.Libs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraSystem.Test.EvaluatorsTests.Replacement
{
    class MonadReplacementTests
    {
        private static readonly Expression[] argumentsInjection = new Expression[]
        {
            Expression.VarA,
            5,
            -10
        };

        [Test, Pairwise]
        public void MonadSingleInjection([Values] MonadBuilder.Monad monad, [ValueSource(nameof(argumentsInjection))] Expression argument)
        {
            // Arrange
            Expression expression = argument;
            Expression pattern = Expression.VarA;
            Expression replacement = MonadBuilder.Build(Expression.VarA, monad);
            Expression expected = MonadBuilder.Build(argument, monad);
            ReplaceEvaluator replaceEvaluator = new ReplaceEvaluator(pattern, replacement);

            // Act
            HashSet<Expression> resultSet = new HashSet<Expression>(expression.Evaluate(replaceEvaluator));

            // Assert
            Assert.That(resultSet, Has.Count.EqualTo(1));
            Assert.That(resultSet, Contains.Item(expected));
        }

        private static readonly Expression[] argumentsRemoval = new Expression[]
        {
            Expression.VarA,
            Expression.VarZ,
            Expression.VarB * 5,
            3 * Expression.VarX - 10
        };

        [Test, Pairwise]
        public void MonadSingleRemoval([Values] MonadBuilder.Monad monad, [ValueSource(nameof(argumentsRemoval))] Expression argument)
        {
            // Arrange
            Expression expression = MonadBuilder.Build(argument, monad);
            Expression pattern = MonadBuilder.Build(Expression.VarA, monad);
            Expression replacement = Expression.VarA;
            Expression expected = argument;
            ReplaceEvaluator replaceEvaluator = new ReplaceEvaluator(pattern, replacement);

            // Act
            HashSet<Expression> resultSet = new HashSet<Expression>(expression.Evaluate(replaceEvaluator));

            // Assert
            Assert.That(resultSet, Has.Count.EqualTo(1));
            Assert.That(resultSet, Contains.Item(expected));
        }

        [Test, Pairwise]
        public void NestedMonadInnerRemoval([Values] MonadBuilder.Monad outerMonad, [Values] MonadBuilder.Monad innerMonad, [ValueSource(nameof(argumentsRemoval))] Expression argument)
        {
            if (outerMonad == innerMonad)
            {
                return;
            }

            // Arrange
            Expression innerExpression = MonadBuilder.Build(argument, innerMonad);
            Expression expression = MonadBuilder.Build(innerExpression, outerMonad);
            Expression pattern = MonadBuilder.Build(Expression.VarA, innerMonad);
            Expression replacement = Expression.VarA;
            Expression expected = MonadBuilder.Build(argument, outerMonad);
            ReplaceEvaluator replaceEvaluator = new ReplaceEvaluator(pattern, replacement);

            // Act
            HashSet<Expression> resultSet = new HashSet<Expression>(expression.Evaluate(replaceEvaluator));

            // Assert
            Assert.That(resultSet, Has.Count.EqualTo(1));
            Assert.That(resultSet, Contains.Item(expected));
        }

        private static readonly Expression[] argumentsReplacement = new Expression[]
        {
            Expression.VarA,
            Expression.VarZ,
            Expression.VarB * 5,
            3 * Expression.VarX - 10
        };

        [Test, Pairwise]
        public void MonadSingleReplacement([Values] MonadBuilder.Monad monadBefore, [Values] MonadBuilder.Monad monadAfter, [ValueSource(nameof(argumentsReplacement))] Expression argument)
        {
            // Arrange
            Expression expression = MonadBuilder.Build(argument, monadBefore);
            Expression pattern = MonadBuilder.Build(Expression.VarA, monadBefore);
            Expression replacement = MonadBuilder.Build(Expression.VarA, monadAfter);
            Expression expected = MonadBuilder.Build(argument, monadAfter);
            ReplaceEvaluator replaceEvaluator = new ReplaceEvaluator(pattern, replacement);

            // Act
            HashSet<Expression> resultSet = new HashSet<Expression>(expression.Evaluate(replaceEvaluator));

            // Assert
            Assert.That(resultSet, Has.Count.EqualTo(1));
            Assert.That(resultSet, Contains.Item(expected));
        }

        [Test, Pairwise]
        public void NestedMonadInnerReplacement([Values] MonadBuilder.Monad outerMonad, 
            [Values] MonadBuilder.Monad monadBefore,
            [Values] MonadBuilder.Monad monadAfter,
            [ValueSource(nameof(argumentsReplacement))] Expression argument)
        {
            if (outerMonad == monadBefore)
            {
                return;
            }

            // Arrange
            Expression expressionInner = MonadBuilder.Build(argument, monadBefore);
            Expression expression = MonadBuilder.Build(expressionInner, outerMonad);
            Expression pattern = MonadBuilder.Build(Expression.VarA, monadBefore);
            Expression replacement = MonadBuilder.Build(Expression.VarA, monadAfter);
            Expression expectedInner = MonadBuilder.Build(argument, monadAfter);
            Expression expected = MonadBuilder.Build(expectedInner, outerMonad);
            ReplaceEvaluator replaceEvaluator = new ReplaceEvaluator(pattern, replacement);

            // Act
            HashSet<Expression> resultSet = new HashSet<Expression>(expression.Evaluate(replaceEvaluator));

            // Assert
            Assert.That(resultSet, Has.Count.EqualTo(1));
            Assert.That(resultSet, Contains.Item(expected));
        }
    }
}
