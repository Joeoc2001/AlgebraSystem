using Algebra;
using System;

namespace AlgebraSystem.Speedtest
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression expression1 = 3 * (Expression.VarY + Expression.VarX) + 2;
            foreach (Expression replaced in expression1.Replace(Expression.VarX + Expression.VarY, Expression.VarX * Expression.VarY))
            {
                Console.WriteLine(replaced);
            }
        }
    }
}
