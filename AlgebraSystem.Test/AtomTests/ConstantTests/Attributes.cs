using Algebra;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.ConstantTests
{
    class Attributes
    {
        [Test]
        public void GetOrderIndex_Is0([ValueSource(typeof(InputValues), nameof(InputValues.expressions))] Expression expression)
        {
            // ARANGE

            // ACT
            int orderIndex = expression.GetOrderIndex();

            // ASSERT
            Assert.AreEqual(0, orderIndex);
        }
    }
}
