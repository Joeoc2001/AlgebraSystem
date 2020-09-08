using Algebra;
using Algebra.Equivalence;
using System;
using System.Collections.Generic;

namespace AlgebraSystem.Speedtest
{
    class Program
    {
        static void Main(string[] args)
        {
            // ARANGE
            Expression eq1 = Expression.VarY * Expression.VarY;
            Expression eq2 = Expression.VarY;

            // ACT

            // ASSERT
            Console.Out.WriteLine(eq1.CompareTo(eq2));
        }
    }
}
