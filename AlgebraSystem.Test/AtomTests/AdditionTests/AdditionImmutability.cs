using Algebra;
using Algebra.Mappings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtomTests.AdditionTests
{
    class AdditionImmutability
    {
        [Test]
        public void CopiesArgumentList()
        {
            // ARANGE
            List<Expression> arguments = new List<Expression>() { Expression.VarX, Expression.VarY, Expression.VarZ };
            Expression expression = Expression.Add(arguments);
            Expression expected = Expression.VarX + Expression.VarY + Expression.VarZ;

            // ACT
            arguments.Add(Expression.VarA);

            // ASSERT
            Assert.AreEqual(expected, expression);
        }

        [Test]
        public void DoesntExposeThroughEvaluator()
        {
            // ARANGE
            Expression expression = Expression.VarX + Expression.VarY + Expression.VarZ;
            Expression expected = Expression.VarX + Expression.VarY + Expression.VarZ;
            AnonymousMapping<bool> evaluator = new AnonymousMapping<bool>(() => false)
            {
                Sum = args =>
                {
                    args.Remove(0);
                    return true;
                }
            };

            // ACT

            // ASSERT
            Assert.Throws(typeof(NotSupportedException), () => expression.Map(evaluator));
        }
    }
}
