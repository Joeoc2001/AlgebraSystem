using Algebra;
using System;

namespace AlgebraSystem.Speedtest
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression expression1 = Expression.Multiply(Expression.VarX, 2);
            Console.WriteLine(expression1);
        }
    }
}
