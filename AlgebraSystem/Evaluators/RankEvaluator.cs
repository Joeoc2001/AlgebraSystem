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

        public int EvaluateProduct(ICollection<IExpression> expressions)
        {
            return 200;
        }

        public int EvaluateSum(ICollection<IExpression> expressions)
        {
            return 300;
        }

        public int EvaluateSin(IExpression argumentExpression)
        {
            return 400;
        }

        public int EvaluateArcsin(IExpression argumentExpression)
        {
            return 500;
        }

        public int EvaluateArctan(IExpression argumentExpression)
        {
            return 600;
        }

        public int EvaluateExponent(IExpression baseExpression, IExpression powerExpression)
        {
            return 700;
        }

        public int EvaluateLn(IExpression argumentExpression)
        {
            return 800;
        }

        public int EvaluateSign(IExpression argumentExpression)
        {
            return 800;
        }

        public int EvaluateFunction(IFunction function)
        {
            return 1000;
        }
    }
}
