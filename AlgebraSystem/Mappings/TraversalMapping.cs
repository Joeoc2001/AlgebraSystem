using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Mappings
{
    public abstract class TraversalMapping<T> : IMapping<T>
    {
        public abstract T EvaluateConstant(IConstant value);

        protected abstract T Pow(T b, T e);

        public T EvaluateExponent(Expression baseExpression, Expression powerExpression)
        {
            T baseValue = baseExpression.Map(this);
            T powerValue = powerExpression.Map(this);

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
                evaluated.Add(expression.Map(this));
            }

            return EvaluateFunction(function, evaluated);
        }

        protected abstract T Ln(T expression);

        public T EvaluateLn(Expression argumentExpression)
        {
            return Ln(argumentExpression.Map(this));
        }

        protected abstract T Product(ICollection<T> expressions);

        public T EvaluateProduct(ICollection<Expression> expressions)
        {
            List<T> evaluated = new List<T>();
            foreach (Expression expression in expressions)
            {
                evaluated.Add(expression.Map(this));
            }
            return Product(evaluated);
        }

        protected abstract T Sign(T expression);

        public T EvaluateSign(Expression argumentExpression)
        {
            return Sign(argumentExpression.Map(this));
        }

        protected abstract T Sin(T expression);

        public T EvaluateSin(Expression argumentExpression)
        {
            return Sin(argumentExpression.Map(this));
        }

        protected abstract T Sum(ICollection<T> expressions);

        public T EvaluateSum(ICollection<Expression> expressions)
        {
            List<T> evaluated = new List<T>();
            foreach (Expression expression in expressions)
            {
                evaluated.Add(expression.Map(this));
            }
            return Sum(evaluated);
        }

        public abstract T EvaluateVariable(IVariable variable);

        protected abstract T Arcsin(T expression);

        public T EvaluateArcsin(Expression argumentExpression)
        {
            return Arcsin(argumentExpression.Map(this));
        }

        protected abstract T Arctan(T expression);

        public T EvaluateArctan(Expression argumentExpression)
        {
            return Arctan(argumentExpression.Map(this));
        }

        [Obsolete]
        public virtual T EvaluateOther(Expression other) { return default; }
    }
}
