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
            IExpression a = IExpression.X;
            IExpression b = IExpression.X;
            int checkInt = ExpressionDisplayComparer.COMPARER.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_YComparedToY_Is0()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.Y;
            IExpression b = IExpression.Y;
            int checkInt = ExpressionDisplayComparer.COMPARER.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_ZComparedToZ_Is0()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.Z;
            IExpression b = IExpression.Z;
            int checkInt = ExpressionDisplayComparer.COMPARER.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_X_IsLessThanY()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.X;
            IExpression b = IExpression.Y;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanZ()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.X;
            IExpression b = IExpression.Z;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsLessThanZ()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.Y;
            IExpression b = IExpression.Z;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThan1()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.X;
            IExpression b = 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanMinus1()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.X;
            IExpression b = -1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.Y;
            IExpression b = IExpression.X;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Z_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.Z;
            IExpression b = IExpression.X;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Z_IsGreaterThanY()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.Z;
            IExpression b = IExpression.Y;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_1_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            IExpression a = 1;
            IExpression b = IExpression.X;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Minus1_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            IExpression a = -1;
            IExpression b = IExpression.X;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusOne_IsLessThanZPlusOne()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.X + 1;
            IExpression b = IExpression.Z + 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusOne_IsLessThanXMinusOne()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.X + 1;
            IExpression b = IExpression.X - 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.X;
            IExpression b = IExpression.X + 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusY_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.X + IExpression.Y;
            IExpression b = IExpression.X + 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.Y;
            IExpression b = IExpression.X + 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusY_IsLessThanYPlusOne()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.X + IExpression.Y;
            IExpression b = IExpression.Y + 1;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquared_IsLessThanX()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.Pow(IExpression.X, 2);
            IExpression b = IExpression.X;
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquaredComparedToXSquared_IsZero()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.Pow(IExpression.X, 2);
            IExpression b = IExpression.Pow(IExpression.X, 2);

            // ASSERT
            Assert.AreEqual(0, ExpressionDisplayComparer.COMPARER.Compare(a, b));
        }

        [Test]
        public void Variable_XCubed_IsLessThanXSquared()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.Pow(IExpression.X, 3);
            IExpression b = IExpression.Pow(IExpression.X, 2);
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquared_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.X;
            IExpression b = IExpression.Pow(IExpression.X, 2);
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XCubed_IsGreaterThanXSquared()
        {
            // ARANGE

            // ACT
            IExpression a = IExpression.Pow(IExpression.X, 2);
            IExpression b = IExpression.Pow(IExpression.X, 3);
            bool check = ExpressionDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }
    }
}
