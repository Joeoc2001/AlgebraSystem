using Algebra;
using System;

namespace AlgebraSystem.Speedtest
{
    class Program
    {
        static void Main(string[] args)
        {
            IExpression expression1 = Expression.Multiply(Expression.VarX, 2);
            foreach (var item in expression1.Evaluate())
            {

            }
            Console.WriteLine(equal);
        }
    }
}
