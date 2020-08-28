using NUnit.Framework;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;

namespace AtomTests
{
    class ExpressionDisplayComparerTests
    {

        [Test]
        public void Variable_XComparedToX_Is0()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarX;
            IExpression b = Expression.VarX;
            int checkInt = ExpressionComparer.Instance.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_YComparedToY_Is0()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarY;
            IExpression b = Expression.VarY;
            int checkInt = ExpressionComparer.Instance.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_ZComparedToZ_Is0()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarZ;
            IExpression b = Expression.VarZ;
            int checkInt = ExpressionComparer.Instance.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_X_IsLessThanY()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarX;
            IExpression b = Expression.VarY;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanZ()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarX;
            IExpression b = Expression.VarZ;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsLessThanZ()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarY;
            IExpression b = Expression.VarZ;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThan1()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarX;
            Expression b = 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanMinus1()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarX;
            Expression b = -1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarY;
            IExpression b = Expression.VarX;
            bool check = ExpressionComparer.Instance.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Z_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarZ;
            IExpression b = Expression.VarX;
            bool check = ExpressionComparer.Instance.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Z_IsGreaterThanY()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarZ;
            IExpression b = Expression.VarY;
            bool check = ExpressionComparer.Instance.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_1_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = 1;
            IExpression b = Expression.VarX;
            bool check = ExpressionComparer.Instance.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Minus1_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = -1;
            IExpression b = Expression.VarX;
            bool check = ExpressionComparer.Instance.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusOne_IsLessThanZPlusOne()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarX + 1;
            IExpression b = Expression.VarZ + 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusOne_IsLessThanXMinusOne()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarX + 1;
            IExpression b = Expression.VarX - 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarX;
            IExpression b = Expression.VarX + 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusY_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarX + Expression.VarY;
            IExpression b = Expression.VarX + 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarY;
            IExpression b = Expression.VarX + 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusY_IsLessThanYPlusOne()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarX + Expression.VarY;
            IExpression b = Expression.VarY + 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquared_IsLessThanX()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.Pow(Expression.VarX, 2);
            IExpression b = Expression.VarX;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquaredComparedToXSquared_IsZero()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.Pow(Expression.VarX, 2);
            IExpression b = Expression.Pow(Expression.VarX, 2);

            // ASSERT
            Assert.AreEqual(0, ExpressionComparer.Instance.Compare(a, b));
        }

        [Test]
        public void Variable_XCubed_IsLessThanXSquared()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.Pow(Expression.VarX, 3);
            IExpression b = Expression.Pow(Expression.VarX, 2);
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquared_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.VarX;
            IExpression b = Expression.Pow(Expression.VarX, 2);
            bool check = ExpressionComparer.Instance.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XCubed_IsGreaterThanXSquared()
        {
            // ARANGE

            // ACT
            IExpression a = Expression.Pow(Expression.VarX, 2);
            IExpression b = Expression.Pow(Expression.VarX, 3);
            bool check = ExpressionComparer.Instance.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }
    }
}
