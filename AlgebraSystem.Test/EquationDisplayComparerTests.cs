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
            Expression a = Expression.VarX;
            Expression b = Expression.VarX;
            int checkInt = ExpressionComparer.Instance.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_YComparedToY_Is0()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarY;
            Expression b = Expression.VarY;
            int checkInt = ExpressionComparer.Instance.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_ZComparedToZ_Is0()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarZ;
            Expression b = Expression.VarZ;
            int checkInt = ExpressionComparer.Instance.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_X_IsLessThanY()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarX;
            Expression b = Expression.VarY;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanZ()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarX;
            Expression b = Expression.VarZ;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsLessThanZ()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarY;
            Expression b = Expression.VarZ;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThan1()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarX;
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
            Expression a = Expression.VarX;
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
            Expression a = Expression.VarY;
            Expression b = Expression.VarX;
            bool check = ExpressionComparer.Instance.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Z_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarZ;
            Expression b = Expression.VarX;
            bool check = ExpressionComparer.Instance.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Z_IsGreaterThanY()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarZ;
            Expression b = Expression.VarY;
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
            Expression b = Expression.VarX;
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
            Expression b = Expression.VarX;
            bool check = ExpressionComparer.Instance.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusOne_IsLessThanZPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarX + 1;
            Expression b = Expression.VarZ + 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusOne_IsLessThanXMinusOne()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarX + 1;
            Expression b = Expression.VarX - 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarX;
            Expression b = Expression.VarX + 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusY_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarX + Expression.VarY;
            Expression b = Expression.VarX + 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarY;
            Expression b = Expression.VarX + 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusY_IsLessThanYPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarX + Expression.VarY;
            Expression b = Expression.VarY + 1;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquared_IsLessThanX()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Pow(Expression.VarX, 2);
            Expression b = Expression.VarX;
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquaredComparedToXSquared_IsZero()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Pow(Expression.VarX, 2);
            Expression b = Expression.Pow(Expression.VarX, 2);

            // ASSERT
            Assert.AreEqual(0, ExpressionComparer.Instance.Compare(a, b));
        }

        [Test]
        public void Variable_XCubed_IsLessThanXSquared()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Pow(Expression.VarX, 3);
            Expression b = Expression.Pow(Expression.VarX, 2);
            bool check = ExpressionComparer.Instance.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquared_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = Expression.VarX;
            Expression b = Expression.Pow(Expression.VarX, 2);
            bool check = ExpressionComparer.Instance.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XCubed_IsGreaterThanXSquared()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Pow(Expression.VarX, 2);
            Expression b = Expression.Pow(Expression.VarX, 3);
            bool check = ExpressionComparer.Instance.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }
    }
}
