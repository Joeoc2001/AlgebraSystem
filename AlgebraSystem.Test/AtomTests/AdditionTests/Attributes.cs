using Algebra;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.AdditionTests
{
    class Attributes
    {
        [Test]
        public void GetOrderIndex_Is30([ValueSource(typeof(InputValues), nameof(InputValues.expressions))] Expression expression)
        {
            // ARANGE

            // ACT
            int orderIndex = expression.GetOrderIndex();

            // ASSERT
            Assert.AreEqual(30, orderIndex);
        }
    }
}
