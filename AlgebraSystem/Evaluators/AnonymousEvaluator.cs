using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public class AnonymousEvaluator<T> : IEvaluator<T>
    {
        private readonly Func<T> _defaultFunc;

        public Func<Expression, T> Arcsin = null;
        public Func<Expression, T> Arctan = null;
        public Func<Rational, T> Constant = null;
        public Func<Expression, Expression, T> Exponent = null;
        public Func<Function, T> Function = null;
        public Func<Expression, T> Ln = null;
        public Func<ICollection<Expression>, T> Product = null;
        public Func<Expression, T> Sign = null;
        public Func<Expression, T> Sin = null;
        public Func<ICollection<Expression>, T> Sum = null;
        public Func<string, T> Variable = null;

        public AnonymousEvaluator(Func<T> defaultFunc)
        {
            _defaultFunc = defaultFunc;
        }

        private T Evaluate<U>(Func<U, T> func, U value)
        {
            return func is null ? Default() : func(value);
        }

        private T Default()
        {
            return _defaultFunc();
        }

        public T EvaluateArcsin(Expression argumentExpression)
        {
            return Evaluate(Arcsin, argumentExpression);
        }

        public T EvaluateArctan(Expression argumentExpression)
        {
            return Evaluate(Arctan, argumentExpression);
        }

        public T EvaluateConstant(Rational value)
        {
            return Evaluate(Constant, value);
        }

        public T EvaluateExponent(Expression baseExpression, Expression powerExpression)
        {
            return Exponent is null ? Default() : Exponent(baseExpression, powerExpression);
        }

        public T EvaluateFunction(Function function)
        {
            return Evaluate(Function, function);
        }

        public T EvaluateLn(Expression argumentExpression)
        {
            return Evaluate(Ln, argumentExpression);
        }

        public virtual T EvaluateOther(Expression other)
        {
            return Default();
        }

        public T EvaluateProduct(ICollection<Expression> expressions)
        {
            return Evaluate(Product, expressions);
        }

        public T EvaluateSign(Expression argumentExpression)
        {
            return Evaluate(Sign, argumentExpression);
        }

        public T EvaluateSin(Expression argumentExpression)
        {
            return Evaluate(Sin, argumentExpression);
        }

        public T EvaluateSum(ICollection<Expression> expressions)
        {
            return Evaluate(Sum, expressions);
        }

        public T EvaluateVariable(string name)
        {
            return Evaluate(Variable, name);
        }
    }
}
