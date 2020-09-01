using Algebra;
using Algebra.Atoms;
using AlgebraSystem.Test.Libs;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AlgebraSystem.Test.AtomTests.AdditionTests
{
    class Equality
    {
        [Test, Timeout(100)]
        public void TrueFor_XPlus1_SingleInstance([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = Expression.VarX + 1;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v1, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test, Timeout(100)]
        public void TrueFor_XPlus1_TwoInstances([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = Expression.VarX + 1;
            IExpression v2 = Expression.VarX + 1;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test, Timeout(100)]
        public void FalseFor_XPlus1_And_XPlus2([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = Expression.VarX + 1;
            IExpression v2 = Expression.VarX + 2;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test, Timeout(100)]
        public void TrueFor_YPlusZ_TwoInstances([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = Expression.VarY + Expression.VarZ;
            IExpression v2 = Expression.VarY + Expression.VarZ;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test, Timeout(100)]
        public void FalseFor_XPlusY_And_XPlusZ([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = Expression.VarX + Expression.VarY;
            IExpression v2 = Expression.VarX + Expression.VarZ;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test, Timeout(100)]
        public void TrueFor_YPlusZ_Commuted([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = Expression.VarY + Expression.VarZ;
            IExpression v2 = Expression.VarZ + Expression.VarY;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test, Timeout(100)]
        public void TrueFor_YPlus5_Commuted([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = Expression.VarY + 5;
            IExpression v2 = 5 + Expression.VarY;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }
    }
}
