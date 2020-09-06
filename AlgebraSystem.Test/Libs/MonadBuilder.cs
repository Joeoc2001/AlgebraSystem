using Algebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraSystem.Test.Libs
{
    class MonadBuilder
    {
        public enum Monad
        {
            Negation,
            Arcsin,
            Arccos,
            Arctan,
            Ln,
            Sign,
            Sin,
            Cos,
            Tan,
            Abs,
            Sinh,
            Cosh,
            Tanh,
            Arcosh,
            Arsinh,
            Artanh,
        }

        public static Expression Build(Expression arg, Monad monad)
        {
            switch (monad)
            {
                case Monad.Negation:
                    return -arg;
                case Monad.Arcsin:
                    return Expression.ArcsinOf(arg);
                case Monad.Arccos:
                    return Expression.ArccosOf(arg);
                case Monad.Arctan:
                    return Expression.ArctanOf(arg);
                case Monad.Ln:
                    return Expression.LnOf(arg);
                case Monad.Sign:
                    return Expression.SignOf(arg);
                case Monad.Sin:
                    return Expression.SinOf(arg);
                case Monad.Cos:
                    return Expression.CosOf(arg);
                case Monad.Tan:
                    return Expression.TanOf(arg);
                case Monad.Abs:
                    return Expression.AbsOf(arg);
                case Monad.Sinh:
                    return Expression.SinhOf(arg);
                case Monad.Cosh:
                    return Expression.CoshOf(arg);
                case Monad.Tanh:
                    return Expression.TanhOf(arg);
                case Monad.Arcosh:
                    return Expression.ArcoshOf(arg);
                case Monad.Arsinh:
                    return Expression.ArsinhOf(arg);
                case Monad.Artanh:
                    return Expression.ArtanhOf(arg);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
