using Algebra;
using System;

namespace AlgebraSystem.Speedtest
{
    class Program
    {
        static void Main(string[] args)
        {
            IExpression expression = Expression.Multiply(Expression.VarX, 2);
            Console.WriteLine(expression.ToString());
        }
    }
}
