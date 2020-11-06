using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.mappings
{
    public class RankMapping : IMapping<int>
    {
        public static readonly RankMapping Instance = new RankMapping();

        protected RankMapping()
        {

        }

        public int EvaluateConstant(IConstant value)
        {
            return 0;
        }

        public int EvaluateVariable(IVariable value)
        {
            return 100;
        }

        public int EvaluateProduct(ICollection<Expression> expressions)
        {
            return 200;
        }

        public int EvaluateSum(ICollection<Expression> expressions)
        {
            return 300;
        }

        public int EvaluateSin(Expression argumentExpression)
        {
            return 400;
        }

        public int EvaluateArcsin(Expression argumentExpression)
        {
            return 500;
        }

        public int EvaluateArctan(Expression argumentExpression)
        {
            return 600;
        }

        public int EvaluateExponent(Expression baseExpression, Expression powerExpression)
        {
            return 700;
        }

        public int EvaluateLn(Expression argumentExpression)
        {
            return 800;
        }

        public int EvaluateSign(Expression argumentExpression)
        {
            return 900;
        }

        public int EvaluateFunction(Function function)
        {
            return 1000;
        }

        public int EvaluateOther(Expression other)
        {
            throw new NotImplementedException($"Cannot get rank for unknown expression {other}. Override {typeof(RankMapping).Name} to add functionality for your new class.");
        }
    }
}
