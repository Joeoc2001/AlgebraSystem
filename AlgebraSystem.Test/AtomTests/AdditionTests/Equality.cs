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
        [Test]
        public void TrueFor_XPlus1_SingleInstance([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.X + 1;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v1, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void TrueFor_XPlus1_TwoInstances([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.X + 1;
            Expression v2 = Expression.X + 1;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void FalseFor_XPlus1_And_XPlus2([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.X + 1;
            Expression v2 = Expression.X + 2;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void TrueFor_YPlusZ_TwoInstances([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.Y + Expression.Z;
            Expression v2 = Expression.Y + Expression.Z;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void FalseFor_XPlusY_And_XPlusZ([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.X + Expression.Y;
            Expression v2 = Expression.X + Expression.Z;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void TrueFor_YPlusZ_Commuted([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.Y + Expression.Z;
            Expression v2 = Expression.Z + Expression.Y;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void TrueFor_YPlus5_Commuted([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.Y + 5;
            Expression v2 = 5 + Expression.Y;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }
    }
}
