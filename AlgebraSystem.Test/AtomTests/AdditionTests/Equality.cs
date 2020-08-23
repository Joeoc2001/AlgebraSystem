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
            IExpression v1 = IExpression.X + 1;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v1, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void TrueFor_XPlus1_TwoInstances([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = IExpression.X + 1;
            IExpression v2 = IExpression.X + 1;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void FalseFor_XPlus1_And_XPlus2([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = IExpression.X + 1;
            IExpression v2 = IExpression.X + 2;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void TrueFor_YPlusZ_TwoInstances([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = IExpression.Y + IExpression.Z;
            IExpression v2 = IExpression.Y + IExpression.Z;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void FalseFor_XPlusY_And_XPlusZ([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = IExpression.X + IExpression.Y;
            IExpression v2 = IExpression.X + IExpression.Z;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void TrueFor_YPlusZ_Commuted([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = IExpression.Y + IExpression.Z;
            IExpression v2 = IExpression.Z + IExpression.Y;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void TrueFor_YPlus5_Commuted([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            IExpression v1 = IExpression.Y + 5;
            IExpression v2 = 5 + IExpression.Y;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }
    }
}
