using Algebra;
using Algebra.Atoms;
using Libs;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.ConstantTests
{
    [Timeout(1000)]
    class Equality
    {
        [Test]
        public void TrueFor_1_SingleInstance([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = 1;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v1, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void TrueFor_10_TwoInstances([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = 10;
            Expression v2 = 10;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void FalseFor_1_And_Minus1([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = 1;
            Expression v2 = -1;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void TrueFor_intmax_TwoInstances([Values] EqualityLibs.EqualityType type, [Values] EqualityLibs.Order order)
        {
            // Arrange
            Expression v1 = int.MaxValue;
            Expression v2 = int.MaxValue;

            // Act
            bool areEqual = EqualityLibs.AreEqual(v1, v2, type, order);

            // Assert
            Assert.IsTrue(areEqual);
        }
    }
}
