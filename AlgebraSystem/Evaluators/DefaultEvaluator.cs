using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public abstract class DefaultEvaluator<T> : IEvaluator<T>
    {
        public abstract T Default();

        public virtual T EvaluateArcsin(IExpression argumentExpression)
        {
            return Default();
        }

        public virtual T EvaluateArctan(IExpression argumentExpression)
        {
            return Default();
        }

        public virtual T EvaluateConstant(Rational value)
        {
            return Default();
        }

        public virtual T EvaluateExponent(IExpression baseExpression, IExpression powerExpression)
        {
            return Default();
        }

        public virtual T EvaluateFunction(IFunction function)
        {
            return Default();
        }

        public virtual T EvaluateLn(IExpression argumentExpression)
        {
            return Default();
        }

        public T EvaluateOther(IExpression other)
        {
            return Default();
        }

        public virtual T EvaluateProduct(ICollection<IExpression> expressions)
        {
            return Default();
        }

        public virtual T EvaluateSign(IExpression argumentExpression)
        {
            return Default();
        }

        public virtual T EvaluateSin(IExpression argumentExpression)
        {
            return Default();
        }

        public virtual T EvaluateSum(ICollection<IExpression> expressions)
        {
            return Default();
        }

        public virtual T EvaluateVariable(string name)
        {
            return Default();
        }
    }
}
