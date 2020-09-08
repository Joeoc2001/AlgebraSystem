using Algebra;
using Algebra.PatternMatching;
using AlgebraSystem.Test.Libs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraSystem.Test.EvaluatorsTests.Replacement
{
    class SpecificCases
    {
        private static readonly Expression[] SubsetOfAddition3to2Expecteds = new Expression[]
        {
            Expression.VarX * (Expression.VarY + Expression.VarZ),
            Expression.VarY * (Expression.VarX + Expression.VarZ),
            Expression.VarZ * (Expression.VarY + Expression.VarX),
            (Expression.VarZ * Expression.VarY) + Expression.VarX,
            (Expression.VarX * Expression.VarZ) + Expression.VarY,
            (Expression.VarX * Expression.VarY) + Expression.VarZ,
        };

        [Test]
        public void TestThat_Replacement_CanTakeSubsetOfAddition3to2([ValueSource(nameof(SubsetOfAddition3to2Expecteds))] Expression expected)
        {
            // Arrange
            Expression expression = Expression.VarX + Expression.VarY + Expression.VarZ;
            Expression pattern = Expression.VarA + Expression.VarB;
            Expression replacement = Expression.VarA * Expression.VarB;
            ReplaceEvaluator replaceEvaluator = new ReplaceEvaluator(pattern, replacement);

            // Act
            IEnumerable<Expression> resultSet = expression.Evaluate(replaceEvaluator);

            // Assert
            Assert.That(new List<Expression>(resultSet), Has.Count.EqualTo(SubsetOfAddition3to2Expecteds.Length));
            Assert.That(resultSet, Contains.Item(expected));
        }

        private static readonly Expression[] SubsetOfMultiplication3to2Expecteds = new Expression[]
        {
            Expression.VarX * (Expression.VarY + Expression.VarZ),
            Expression.VarY * (Expression.VarX + Expression.VarZ),
            Expression.VarZ * (Expression.VarY + Expression.VarX),
            (Expression.VarZ * Expression.VarY) + Expression.VarX,
            (Expression.VarX * Expression.VarZ) + Expression.VarY,
            (Expression.VarX * Expression.VarY) + Expression.VarZ,
        };

        [Test]
        public void TestThat_Replacement_CanTakeSubsetOfMultiplication3to2([ValueSource(nameof(SubsetOfMultiplication3to2Expecteds))] Expression expected)
        {
            // Arrange
            Expression expression = Expression.VarX * Expression.VarY * Expression.VarZ;
            Expression pattern = Expression.VarA * Expression.VarB;
            Expression replacement = Expression.VarA + Expression.VarB;
            ReplaceEvaluator replaceEvaluator = new ReplaceEvaluator(pattern, replacement);

            // Act
            IEnumerable<Expression> resultSet = expression.Evaluate(replaceEvaluator);

            // Assert
            Assert.That(new List<Expression>(resultSet), Has.Count.EqualTo(SubsetOfMultiplication3to2Expecteds.Length));
            Assert.That(resultSet, Contains.Item(expected));
        }
    }
}
