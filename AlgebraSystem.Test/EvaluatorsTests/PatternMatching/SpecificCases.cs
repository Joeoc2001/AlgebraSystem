using Algebra;
using Algebra.PatternMatching;
using Libs;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EvaluatorsTests.PatternMatching
{
    class SpecificCases
    {
        [Test]
        public void IsCommutative([Values(DyadBuilder.Dyad.Addition, DyadBuilder.Dyad.Multiplication)] DyadBuilder.Dyad dyad, [Range(0, 1)] int index)
        {
            // Arrange
            Expression expression = DyadBuilder.Build(Expression.VarX, Expression.VarY, dyad);
            Expression pattern = DyadBuilder.Build(Expression.VarA, Expression.VarB, dyad);
            PatternMatchingResult[] results = new PatternMatchingResult[]
            {
                new  PatternMatchingResult(
                    ("a", Expression.VarX),
                    ("b", Expression.VarY)
                ),
                new PatternMatchingResult(
                    ("b", Expression.VarX),
                    ("a", Expression.VarY)
                )
            };

            // Act
            PatternMatchingResultSet resultSet = expression.Match(pattern);

            // Assert
            Assert.That(resultSet, Contains.Item(results[index]));
        }

        [Test]
        public void IsAdditionAssociative3To2([Range(0, 3)] int index)
        {
            // Arrange
            Expression expression = Expression.VarX + Expression.VarY + Expression.VarZ;
            Expression pattern = Expression.VarA + Expression.VarB;
            PatternMatchingResult[] results = new PatternMatchingResult[]
            {
                new  PatternMatchingResult(
                    ("a", Expression.VarX + Expression.VarY),
                    ("b", Expression.VarZ)
                ),
                new PatternMatchingResult(
                    ("a", Expression.VarX + Expression.VarZ),
                    ("b", Expression.VarY)
                ),
                new PatternMatchingResult(
                    ("a", Expression.VarY + Expression.VarZ),
                    ("b", Expression.VarX)
                ),
                new PatternMatchingResult(
                    ("a", Expression.VarY),
                    ("b", Expression.VarX + Expression.VarZ)
                )
            };

            // Act
            PatternMatchingResultSet resultSet = expression.Match(pattern);

            // Assert
            Assert.That(resultSet, Contains.Item(results[index]));
        }

        [Test]
        public void IsAdditionAssociative4To2([Range(0, 3)] int index)
        {
            // Arrange
            Expression expression = Expression.VarX + Expression.VarY + Expression.VarZ + 100;
            Expression pattern = Expression.VarA + Expression.VarB;
            PatternMatchingResult[] results = new PatternMatchingResult[]
            {
                new  PatternMatchingResult(
                    ("a", Expression.VarX + Expression.VarY + Expression.VarZ),
                    ("b", 100)
                ),
                new PatternMatchingResult(
                    ("a", Expression.VarX + Expression.VarZ),
                    ("b", Expression.VarY + 100)
                ),
                new PatternMatchingResult(
                    ("a", Expression.VarY + Expression.VarZ),
                    ("b", Expression.VarX + 100)
                ),
                new PatternMatchingResult(
                    ("a", Expression.VarY),
                    ("b", Expression.VarX + Expression.VarZ + 100)
                )
            };

            // Act
            PatternMatchingResultSet resultSet = expression.Match(pattern);

            // Assert
            Assert.That(resultSet, Contains.Item(results[index]));
        }

        [Test]
        public void IsMultiplicationAssociative3To2([Range(0, 3)] int index)
        {
            // Arrange
            Expression expression = Expression.VarX * Expression.VarY * Expression.VarZ;
            Expression pattern = Expression.VarA * Expression.VarB;
            PatternMatchingResult[] results = new PatternMatchingResult[]
            {
                new  PatternMatchingResult(
                    ("a", Expression.VarX * Expression.VarY),
                    ("b", Expression.VarZ)
                ),
                new PatternMatchingResult(
                    ("a", Expression.VarX * Expression.VarZ),
                    ("b", Expression.VarY)
                ),
                new PatternMatchingResult(
                    ("a", Expression.VarY * Expression.VarZ),
                    ("b", Expression.VarX)
                ),
                new PatternMatchingResult(
                    ("a", Expression.VarY),
                    ("b", Expression.VarX * Expression.VarZ)
                )
            };

            // Act
            PatternMatchingResultSet resultSet = expression.Match(pattern);

            // Assert
            Assert.That(resultSet, Contains.Item(results[index]));
        }

        [Test]
        public void IsMultiplicationAssociative4To2([Range(0, 3)] int index)
        {
            // Arrange
            Expression expression = Expression.VarX * Expression.VarY * Expression.VarZ * 200;
            Expression pattern = Expression.VarA * Expression.VarB;
            PatternMatchingResult[] results = new PatternMatchingResult[]
            {
                new  PatternMatchingResult(
                    ("a", Expression.VarX * Expression.VarZ),
                    ("b", 200 * Expression.VarY)
                ),
                new PatternMatchingResult(
                    ("a", Expression.VarX * Expression.VarZ),
                    ("b", Expression.VarY * 200)
                ),
                new PatternMatchingResult(
                    ("a", Expression.VarY * Expression.VarZ * 200),
                    ("b", Expression.VarX)
                ),
                new PatternMatchingResult(
                    ("a", Expression.VarY),
                    ("b", Expression.VarX * Expression.VarZ * 200)
                )
            };

            // Act
            PatternMatchingResultSet resultSet = expression.Match(pattern);

            // Assert
            Assert.That(resultSet, Contains.Item(results[index]));
        }
    }
}
