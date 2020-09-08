using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public class RankEvaluator : IEvaluator<int>
    {
        public static readonly RankEvaluator Instance = new RankEvaluator();

        protected RankEvaluator()
        {

        }

        public int EvaluateConstant(Rational value)
        {
            return 0;
        }

        public int EvaluateVariable(string name)
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
            throw new NotImplementedException($"Cannot get rank for unknown expression {other}. Override {typeof(RankEvaluator).Name} to add functionality for your new class.");
        }
    }
}
