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
            Expression eq = (Expression.VarX + 2) * (Expression.VarX + 3);
            Expression expected = Expression.VarX * Expression.VarX
                + 5 * Expression.VarX
                + 6;

            // ACT
            bool contained = AreInSameClass(eq, expected);

            // ASSERT
            Console.Out.WriteLine(contained);
        }
    }
}
