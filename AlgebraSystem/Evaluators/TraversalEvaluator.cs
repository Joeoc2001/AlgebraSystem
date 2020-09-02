using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public abstract class TraversalEvaluator<T> : IEvaluator<T>
    {
        public abstract T EvaluateConstant(Rational value);

        protected abstract T Pow(T b, T e);

        public T EvaluateExponent(Expression baseExpression, Expression powerExpression)
        {
            T baseValue = baseExpression.Evaluate(this);
            T powerValue = powerExpression.Evaluate(this);

            return Pow(baseValue, powerValue);
        }

        protected abstract T EvaluateFunction(Function function, IList<T> parameters);

        public T EvaluateFunction(Function function)
        {
            // Evaluate parameters
            IList<Expression> parameters = function.GetParameterList();
            List<T> evaluated = new List<T>();
            foreach (Expression expression in parameters)
            {
                evaluated.Add(expression.Evaluate(this));
            }

            return EvaluateFunction(function, evaluated);
        }

        protected abstract T Ln(T expression);

        public T EvaluateLn(Expression argumentExpression)
        {
            return Ln(argumentExpression.Evaluate(this));
        }

        protected abstract T Product(ICollection<T> expressions);

        public T EvaluateProduct(ICollection<Expression> expressions)
        {
            List<T> evaluated = new List<T>();
            foreach (Expression expression in expressions)
            {
                evaluated.Add(expression.Evaluate(this));
            }
            return Product(evaluated);
        }

        protected abstract T Sign(T expression);

        public T EvaluateSign(Expression argumentExpression)
        {
            return Sign(argumentExpression.Evaluate(this));
        }

        protected abstract T Sin(T expression);

        public T EvaluateSin(Expression argumentExpression)
        {
            return Sin(argumentExpression.Evaluate(this));
        }

        protected abstract T Sum(ICollection<T> expressions);

        public T EvaluateSum(ICollection<Expression> expressions)
        {
            List<T> evaluated = new List<T>();
            foreach (Expression expression in expressions)
            {
                evaluated.Add(expression.Evaluate(this));
            }
            return Sum(evaluated);
        }

        public abstract T EvaluateVariable(string name);

        protected abstract T Arcsin(T expression);

        public T EvaluateArcsin(Expression argumentExpression)
        {
            return Arcsin(argumentExpression.Evaluate(this));
        }

        protected abstract T Arctan(T expression);

        public T EvaluateArctan(Expression argumentExpression)
        {
            return Arctan(argumentExpression.Evaluate(this));
        }

        public abstract T EvaluateOther(Expression other);
    }
}
