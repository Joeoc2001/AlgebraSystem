using Algebra;
using AtomTests.AdditionTests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.AdditionTests
{
    class AdditionParsing
    {
        [Test]
        public void ToStringParsedIsEqualToOriginal([ValueSource(typeof(AdditionInputValues), nameof(AdditionInputValues.allExpressions))] Expression expression)
        {
            // ARANGE
            string str = expression.ToString();

            // ACT
            Expression parsed = Expression.From(str);

            // ASSERT
            Assert.AreEqual(expression, parsed);
        }
    }
}
