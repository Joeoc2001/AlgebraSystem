using NUnit.Framework;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;

namespace AtomTests
{
    class EquationDisplayComparerTests
    {

        [Test]
        public void Variable_XComparedToX_Is0()
        {
            // ARANGE

            // ACT
            Expression a = Variable.X;
            Expression b = Variable.X;
            int checkInt = EquationDisplayComparer.COMPARER.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_YComparedToY_Is0()
        {
            // ARANGE

            // ACT
            Expression a = Variable.Y;
            Expression b = Variable.Y;
            int checkInt = EquationDisplayComparer.COMPARER.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_ZComparedToZ_Is0()
        {
            // ARANGE

            // ACT
            Expression a = Variable.Z;
            Expression b = Variable.Z;
            int checkInt = EquationDisplayComparer.COMPARER.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_WComparedToW_Is0()
        {
            // ARANGE

            // ACT
            Expression a = Variable.W;
            Expression b = Variable.W;
            int checkInt = EquationDisplayComparer.COMPARER.Compare(a, b);

            // ASSERT
            Assert.AreEqual(0, checkInt);
        }

        [Test]
        public void Variable_X_IsLessThanY()
        {
            // ARANGE

            // ACT
            Expression a = Variable.X;
            Expression b = Variable.Y;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanZ()
        {
            // ARANGE

            // ACT
            Expression a = Variable.X;
            Expression b = Variable.Z;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsGreaterThanThanW()
        {
            // ARANGE

            // ACT
            Expression a = Variable.X;
            Expression b = Variable.W;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsLessThanZ()
        {
            // ARANGE

            // ACT
            Expression a = Variable.Y;
            Expression b = Variable.Z;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsGreaterThanW()
        {
            // ARANGE

            // ACT
            Expression a = Variable.Y;
            Expression b = Variable.W;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Z_IsGreaterThanW()
        {
            // ARANGE

            // ACT
            Expression a = Variable.Z;
            Expression b = Variable.W;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThan1()
        {
            // ARANGE

            // ACT
            Expression a = Variable.X;
            Expression b = 1;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanMinus1()
        {
            // ARANGE

            // ACT
            Expression a = Variable.X;
            Expression b = -1;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = Variable.Y;
            Expression b = Variable.X;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Z_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = Variable.Z;
            Expression b = Variable.X;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_W_IsLessThanX()
        {
            // ARANGE

            // ACT
            Expression a = Variable.W;
            Expression b = Variable.X;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Z_IsGreaterThanY()
        {
            // ARANGE

            // ACT
            Expression a = Variable.Z;
            Expression b = Variable.Y;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_W_IsLessThanY()
        {
            // ARANGE

            // ACT
            Expression a = Variable.W;
            Expression b = Variable.Y;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_W_IsLessThanZ()
        {
            // ARANGE

            // ACT
            Expression a = Variable.W;
            Expression b = Variable.Z;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_1_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = 1;
            Expression b = Variable.X;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Minus1_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = -1;
            Expression b = Variable.X;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusOne_IsLessThanZPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Variable.X + 1;
            Expression b = Variable.Z + 1;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusOne_IsLessThanXMinusOne()
        {
            // ARANGE

            // ACT
            Expression a = Variable.X + 1;
            Expression b = Variable.X - 1;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_X_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Variable.X;
            Expression b = Variable.X + 1;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusY_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Variable.X + Variable.Y;
            Expression b = Variable.X + 1;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_Y_IsLessThanXPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Variable.Y;
            Expression b = Variable.X + 1;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XPlusY_IsLessThanYPlusOne()
        {
            // ARANGE

            // ACT
            Expression a = Variable.X + Variable.Y;
            Expression b = Variable.Y + 1;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquared_IsLessThanX()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Pow(Variable.X, 2);
            Expression b = Variable.X;
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquaredComparedToXSquared_IsZero()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Pow(Variable.X, 2);
            Expression b = Expression.Pow(Variable.X, 2);

            // ASSERT
            Assert.AreEqual(0, EquationDisplayComparer.COMPARER.Compare(a, b));
        }

        [Test]
        public void Variable_XCubed_IsLessThanXSquared()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Pow(Variable.X, 3);
            Expression b = Expression.Pow(Variable.X, 2);
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) < 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XSquared_IsGreaterThanX()
        {
            // ARANGE

            // ACT
            Expression a = Variable.X;
            Expression b = Expression.Pow(Variable.X, 2);
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }

        [Test]
        public void Variable_XCubed_IsGreaterThanXSquared()
        {
            // ARANGE

            // ACT
            Expression a = Expression.Pow(Variable.X, 2);
            Expression b = Expression.Pow(Variable.X, 3);
            bool check = EquationDisplayComparer.COMPARER.Compare(a, b) > 0;

            // ASSERT
            Assert.IsTrue(check);
        }
    }
}
