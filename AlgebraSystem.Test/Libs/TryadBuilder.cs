using Algebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libs
{
    class TryadBuilder
    {
        public enum Tryad
        {
            Addition,
            Multiplication,
            Select
        }

        public static Expression Build(Expression a, Expression b, Expression c, Tryad tryad)
        {
            switch (tryad)
            {
                case Tryad.Addition:
                    return a + b + c;
                case Tryad.Multiplication:
                    return a * b * c;
                case Tryad.Select:
                    return Expression.SelectOn(a, b, c);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
