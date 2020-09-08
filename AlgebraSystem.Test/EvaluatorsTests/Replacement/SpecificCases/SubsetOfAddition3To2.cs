using Algebra;
using Algebra.PatternMatching;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvaluatorsTests.Replacement.SpecificCases
{
    class SubsetOfAddition3To2
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

        static Expression expression = Expression.VarX + Expression.VarY + Expression.VarZ;
        static Expression pattern = Expression.VarA + Expression.VarB;
        static Expression replacement = Expression.VarA * Expression.VarB;
        static ReplaceEvaluator replaceEvaluator = new ReplaceEvaluator(pattern, replacement);

        [Test]
        public void TestThat_Replacement_CanTakeSubsetOfAddition3to2([ValueSource(nameof(SubsetOfAddition3to2Expecteds))] Expression expected)
        {
            // Arrange

            // Act
            List<Expression> resultSet = new List<Expression>(expression.Evaluate(replaceEvaluator));

            // Assert
            Assert.That(resultSet, Contains.Item(expected));
        }

        [Test]
        public void TestThat_Replacement_CorrectCountOfAddition3to2()
        {
            // Arrange

            // Act
            List<Expression> resultSet = new List<Expression>(expression.Evaluate(replaceEvaluator));

            // Assert
            Assert.That(resultSet, Has.Count.EqualTo(SubsetOfAddition3to2Expecteds.Length));
        }
    }
}
