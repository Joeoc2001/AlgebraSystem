﻿using Algebra;
using Algebra.Atoms;
using Libs;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.AdditionTests
{
    [Timeout(1000)]
    class AdditionEquality
    {
        [Test]
        public void TrueFor_XPlus1_SingleInstance([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.VarX + 1;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v1, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void TrueFor_XPlus1_TwoInstances([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.VarX + 1;
            Expression v2 = Expression.VarX + 1;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void FalseFor_XPlus1_And_XPlus2([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.VarX + 1;
            Expression v2 = Expression.VarX + 2;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void TrueFor_YPlusZ_TwoInstances([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.VarY + Expression.VarZ;
            Expression v2 = Expression.VarY + Expression.VarZ;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void FalseFor_XPlusY_And_XPlusZ([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.VarX + Expression.VarY;
            Expression v2 = Expression.VarX + Expression.VarZ;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void TrueFor_YPlusZ_Commuted([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.VarY + Expression.VarZ;
            Expression v2 = Expression.VarZ + Expression.VarY;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void TrueFor_XPlusYPlusZ_Commuted([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.VarX + Expression.VarY + Expression.VarZ;
            Expression v2 = Expression.VarZ + Expression.VarY + Expression.VarX;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void TrueFor_YPlus5_Commuted([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.VarY + 5;
            Expression v2 = 5 + Expression.VarY;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void TrueFor_YPlusYSquared_Commuted([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.VarY + Expression.Pow(Expression.VarY, 2);
            Expression v2 = Expression.Pow(Expression.VarY, 2) + Expression.VarY;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void TrueFor_EmptySum_And0([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = Expression.Add(new List<Expression>());
            Expression v2 = 0;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }
    }
}
