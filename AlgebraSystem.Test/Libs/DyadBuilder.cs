using Algebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libs
{
    class DyadBuilder
    {
        public enum Dyad
        {
            Addition,
            Subtraction,
            Multiplication,
            Division,
            Exponentiation,
            Min,
            Max
        }

        public static Expression Build(Expression left, Expression right, Dyad dyad)
        {
            switch (dyad)
            {
                case Dyad.Addition:
                    return left + right;
                case Dyad.Subtraction:
                    return left - right;
                case Dyad.Multiplication:
                    return left * right;
                case Dyad.Division:
                    return left / right;
                case Dyad.Exponentiation:
                    return Expression.Pow(left, right);
                case Dyad.Min:
                    return Expression.Min(left, right);
                case Dyad.Max:
                    return Expression.Max(left, right);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
