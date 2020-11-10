using Algebra;
using Algebra.PatternMatching;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvaluatorsTests.Replacement.SpecificCases
{
    class SubsetOfMultiplication3To2
    {
        private static readonly Expression[] SubsetOfMultiplication3to2Expecteds = new Expression[]
        {
            Expression.VarX * (Expression.VarY + Expression.VarZ),
            Expression.VarY * (Expression.VarX + Expression.VarZ),
            Expression.VarZ * (Expression.VarY + Expression.VarX),
            (Expression.VarZ * Expression.VarY) + Expression.VarX,
            (Expression.VarX * Expression.VarZ) + Expression.VarY,
            (Expression.VarX * Expression.VarY) + Expression.VarZ,
        };

        static Expression expression = Expression.VarX * Expression.VarY * Expression.VarZ;
        static Expression pattern = Expression.VarA * Expression.VarB;
        static Expression replacement = Expression.VarA + Expression.VarB;
        static ReplaceMapping replaceEvaluator = new ReplaceMapping(pattern, replacement);

        [Test]
        public void TestThat_Replacement_CanTakeSubsetOfMultiplication3to2([ValueSource(nameof(SubsetOfMultiplication3to2Expecteds))] Expression expected)
        {
            // Arrange

            // Act
            IEnumerable<Expression> resultSet = expression.Map(replaceEvaluator);

            // Assert
            Assert.That(resultSet, Contains.Item(expected));
        }

        [Test]
        public void TestThat_Replacement_CorrectCountOfMultiplication3to2()
        {
            // Arrange

            // Act
            IEnumerable<Expression> resultSet = expression.Map(replaceEvaluator);

            // Assert
            Assert.That(new List<Expression>(resultSet), Has.Count.EqualTo(SubsetOfMultiplication3to2Expecteds.Length));
        }
    }
}
