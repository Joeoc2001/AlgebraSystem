using Algebra;
using Algebra.Equivalence;
using System;
using System.Collections.Generic;
using Algebra.Parsing;

namespace AlgebraSystem.Speedtest
{
    class Program
    {
        static void Main(string[] args)
        {
            // ARANGE
            string expression = "x + 1";
            Expression expected = Expression.VarX + 1;

            // ACT
            Expression result = Parser.Parse(expression);

            // ASSERT
            Console.Out.WriteLine(expected.Equals(result));
            Console.ReadKey();
        }
    }
}
