using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public class IsAtomicEvaluator : IEvaluator<bool>
    {
        public IsAtomicEvaluator()
        {
        }

        public bool EvaluateConstant(Rational value)
        {
            return true;
        }

        public bool EvaluateExponent(IExpression baseExpression, IExpression powerExpression)
        {
            return baseExpression.Evaluate(this) && powerExpression.Evaluate(this);
        }

        public bool EvaluateFunction(IFunction function)
        {
            return false;
        }

        public bool EvaluateLn(IExpression argumentExpression)
        {
            return argumentExpression.Evaluate(this);
        }

        public bool EvaluateSet(ICollection<IExpression> expressions)
        {
            foreach (IExpression expression in expressions)
            {
                if (!expression.Evaluate(this))
                {
                    return false;
                }
            }
            return true;
        }

        public bool EvaluateProduct(ICollection<IExpression> expressions)
        {
            return EvaluateSet(expressions);
        }

        public bool EvaluateSign(IExpression argumentExpression)
        {
            return argumentExpression.Evaluate(this);
        }

        public bool EvaluateSin(IExpression argumentExpression)
        {
            return argumentExpression.Evaluate(this);
        }

        public bool EvaluateSum(ICollection<IExpression> expressions)
        {
            return EvaluateSet(expressions);
        }

        public bool EvaluateVariable(string name)
        {
            return true;
        }

        public bool EvaluateArcsin(IExpression argumentExpression)
        {
            return argumentExpression.Evaluate(this);
        }

        public bool EvaluateArctan(IExpression argumentExpression)
        {
            return argumentExpression.Evaluate(this);
        }
    }
}
