using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rationals;
using Algebra;
using Algebra.Atoms;
using Algebra.Parsing;

namespace AtomTests
{
    public class LogTests
    {
        [Test]
        public void Log_DoesntSimplify_WhenConstantParameter()
        {
            // ARANGE

            // ACT
            Expression e = Expression.LnOf(10);

            // ASSERT
            Assert.IsFalse(e is Constant);
        }

        [Test]
        public void Log_GetOrderIndex_Is0()
        {
            // ARANGE

            // ACT
            Expression equation = Expression.LnOf(Variable.X);

            // ASSERT
            Assert.AreEqual(0, equation.GetOrderIndex());
        }

        [Test]
        public void Log_GetHash_IsNotArgumentHash1()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.Y;
            Expression equation = Expression.LnOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = equation.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Log_GetHash_IsNotArgumentHash2()
        {
            // ARANGE

            // ACT
            Expression argument = 2;
            Expression equation = Expression.LnOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = equation.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Log_GetHash_IsNotArgumentHash3()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.X + 1;
            Expression equation = Expression.LnOf(argument);
            int hash1 = argument.GetHashCode();
            int hash2 = equation.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Log_GetHash_IsNotSignHash()
        {
            // ARANGE

            // ACT
            Expression argument = Variable.X;
            Expression ln = Expression.LnOf(argument);
            Expression sign = Expression.SignOf(argument);
            int hash1 = ln.GetHashCode();
            int hash2 = sign.GetHashCode();

            // ASSERT
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Log_Map_DoesntChangeOriginal()
        {
            // ARANGE
            Expression equation1 = Expression.LnOf(Variable.X);
            Expression equation2 = Expression.LnOf(Variable.X);

            // ACT
            equation2.Map(a => Expression.LnOf(Variable.Y));

            // ASSERT
            Assert.AreEqual(equation1, equation2);
        }

        [Test]
        public void Log_Map_ReturnsAlternative()
        {
            // ARANGE
            Expression equation1 = Expression.LnOf(Variable.X);

            // ACT
            Expression equation2 = equation1.Map(a => Expression.LnOf(Variable.Y));

            // ASSERT
            Assert.AreEqual(Expression.LnOf(Variable.Y), equation2);
        }

        [Test]
        public void Log_Map_MapsChildren()
        {
            // ARANGE
            Expression equation1 = Expression.LnOf(Variable.X);

            // ACT
            Expression equation2 = equation1.Map(a => a is Variable ? Variable.Z : a);

            // ASSERT
            Assert.AreEqual(Expression.LnOf(Variable.Z), equation2);
        }

        [Test]
        public void Log_Map_CanSkipSelf()
        {
            // ARANGE
            Expression equation1 = Expression.LnOf(Variable.X);
            EquationMapping mapping = new EquationMapping()
            {
                PostMap = a => Variable.Z,
                ShouldMapThis = a => !(a is Ln)
            };

            // ACT
            Expression equation2 = equation1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Expression.LnOf(Variable.Z), equation2);
        }

        [Test]
        public void Log_Map_CanSkipChildren()
        {
            // ARANGE
            Expression equation1 = Expression.LnOf(Variable.X);
            EquationMapping mapping = new EquationMapping()
            {
                PostMap = a => a is Variable ? Variable.Z : a,
                ShouldMapChildren = a => false
            };

            // ACT
            Expression equation2 = equation1.Map(mapping);

            // ASSERT
            Assert.AreEqual(Expression.LnOf(Variable.X), equation2);
        }
    }
}
