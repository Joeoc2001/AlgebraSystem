using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public abstract class TraversalEvaluator<T> : IEvaluator<T>
    {
        public TraversalEvaluator()
        {

        }

        public abstract T EvaluateConstant(Rational value);

        protected abstract T Pow(T b, T e);

        public T EvaluateExponent(IExpression baseExpression, IExpression powerExpression)
        {
            T baseValue = baseExpression.Evaluate(this);
            T powerValue = powerExpression.Evaluate(this);

            return Pow(baseValue, powerValue);
        }

        protected abstract T EvaluateFunction(IFunction function, IList<T> parameters);

        public T EvaluateFunction(IFunction function)
        {
            // Evaluate parameters
            IList<IExpression> parameters = function.GetParameterList();
            List<T> evaluated = new List<T>();
            foreach (IExpression expression in parameters)
            {
                evaluated.Add(expression.Evaluate(this));
            }

            return EvaluateFunction(function, evaluated);
        }

        protected abstract T Ln(T v);

        public T EvaluateLn(IExpression argumentExpression)
        {
            return Ln(argumentExpression.Evaluate(this));
        }

        protected abstract T Product(ICollection<T> expressions);

        public T EvaluateProduct(ICollection<IExpression> expressions)
        {
            List<T> evaluated = new List<T>();
            foreach (IExpression expression in expressions)
            {
                evaluated.Add(expression.Evaluate(this));
            }
            return Product(evaluated);
        }

        protected abstract T Sign(T v);

        public T EvaluateSign(IExpression argumentExpression)
        {
            return Sign(argumentExpression.Evaluate(this));
        }

        protected abstract T Sin(T v);

        public T EvaluateSin(IExpression argumentExpression)
        {
            return Sin(argumentExpression.Evaluate(this));
        }

        protected abstract T Sum(ICollection<T> expressions);

        public T EvaluateSum(ICollection<IExpression> expressions)
        {
            List<T> evaluated = new List<T>();
            foreach (IExpression expression in expressions)
            {
                evaluated.Add(expression.Evaluate(this));
            }
            return Sum(evaluated);
        }

        public abstract T EvaluateVariable(string name);
    }
}
