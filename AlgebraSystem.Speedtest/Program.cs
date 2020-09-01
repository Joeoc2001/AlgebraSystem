using Algebra;
using System;

namespace AlgebraSystem.Speedtest
{
    class Program
    {
        static void Main(string[] args)
        {
            IExpression expression1 = Expression.Multiply(Expression.VarX, 2);
            IExpression expression2 = Expression.Multiply(Expression.VarX, 2);
            bool equal = expression1.Equals(expression2, EqualityLevel.Atomic);
            Console.WriteLine(equal);
        }
    }
}
