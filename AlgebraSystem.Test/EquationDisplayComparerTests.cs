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
            Expression a = Expression.X;
            Expression b = Expression.X;
            int checkInt = ExpressionDisplayComparer.COMPARER.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_YComparedToY_Is0()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Y;
            Expression b = Expression.Y;
            int checkInt = ExpressionDisplayComparer.COMPARER.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_ZComparedToZ_Is0()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Z;
            Expression b = Expression.Z;
            int checkInt = ExpressionDisplayComparer.COMPARER.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_X_IsLessThanY()
        {
            // ARANGE

            // ACT
            Expression a = Expression.X;
            Expression b = Expression.Y;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanZ()
        {
            // ARANGE

            // ACT
            Expression a = Expression.X;
            Expression b = Expression.Z;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsLessThanZ()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Y;
            Expression b = Expression.Z;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThan1()
        {
            // ARANGE

            // ACT
            Expression a = Expression.X;
            Expression b = 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanMinus1()
        {
            // ARANGE

            // ACT
            Expression a = Expression.X;
            Expression b = -1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Y;
            Expression b = Expression.X;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Z_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Z;
            Expression b = Expression.X;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Z_IsGreaterThanY()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Z;
            Expression b = Expression.Y;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_1_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = 1;
            Expression b = Expression.X;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Minus1_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = -1;
            Expression b = Expression.X;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusOne_IsLessThanZPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Expression.X + 1;
            Expression b = Expression.Z + 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusOne_IsLessThanXMinusOne()
        {
            // ARANGE

            // ACT
            Expression a = Expression.X + 1;
            Expression b = Expression.X - 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Expression.X;
            Expression b = Expression.X + 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusY_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Expression.X + Expression.Y;
            Expression b = Expression.X + 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Y;
            Expression b = Expression.X + 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusY_IsLessThanYPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Expression.X + Expression.Y;
            Expression b = Expression.Y + 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquared_IsLessThanX()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Pow(Expression.X, 2);
            Expression b = Expression.X;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquaredComparedToXSquared_IsZero()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Pow(Expression.X, 2);
            Expression b = Expression.Pow(Expression.X, 2);

            // ASSERT
            Assert.AreEqual(0, ExpressionDisplayComparer.COMPARER.Compare(a, b));
        }

        [Test]
        public void Variable_XCubed_IsLessThanXSquared()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Pow(Expression.X, 3);
            Expression b = Expression.Pow(Expression.X, 2);
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquared_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = Expression.X;
            Expression b = Expression.Pow(Expression.X, 2);
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XCubed_IsGreaterThanXSquared()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Pow(Expression.X, 2);
            Expression b = Expression.Pow(Expression.X, 3);
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }
    }
}
