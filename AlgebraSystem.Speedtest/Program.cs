using Algebra;
using Algebra.Equivalence;
using System;
using System.Collections.Generic;

namespace AlgebraSystem.Speedtest
{
    class Program
    {
        private static bool AreInSameClass(Expression start, Expression end)
        {
            EquivalenceClass equivalence = start.GetEquivalenceClass();
            return equivalence.IsInClass(end, -1);
        }

        static void Main(string[] args)
        {
            // ARANGE
            Expression eq = (Expression.VarX + 1) * (Expression.VarX + 2) * (Expression.VarX + 3) * (Expression.VarX + 4);
            Expression expected = Expression.VarX * Expression.VarX * Expression.VarX * Expression.VarX
                + 10 * Expression.VarX * Expression.VarX * Expression.VarX
                + 35 * Expression.VarX * Expression.VarX
                + 50 * Expression.VarX
                + 24;

            // ACT
            bool contained = AreInSameClass(eq, expected);

            // ASSERT
            Console.Out.WriteLine(contained);
        }
    }
}
