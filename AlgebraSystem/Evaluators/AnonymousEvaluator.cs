using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public class AnonymousEvaluator<T> : IEvaluator<T>
    {
        private readonly Func<T> _defaultFunc;

        public readonly Func<IExpression, T> Arcsin = null;
        public readonly Func<IExpression, T> Arctan = null;
        public readonly Func<Rational, T> Constant = null;
        public readonly Func<IExpression, IExpression, T> Exponent = null;
        public readonly Func<IFunction, T> Function = null;
        public readonly Func<IExpression, T> Ln = null;
        public readonly Func<ICollection<IExpression>, T> Product = null;
        public readonly Func<IExpression, T> Sign = null;
        public readonly Func<IExpression, T> Sin = null;
        public readonly Func<ICollection<IExpression>, T> Sum = null;
        public readonly Func<string, T> Variable = null;

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

        public T EvaluateArcsin(IExpression argumentExpression)
        {
            return Evaluate(Arcsin, argumentExpression);
        }

        public T EvaluateArctan(IExpression argumentExpression)
        {
            return Evaluate(Arctan, argumentExpression);
        }

        public T EvaluateConstant(Rational value)
        {
            return Evaluate(Constant, value);
        }

        public T EvaluateExponent(IExpression baseExpression, IExpression powerExpression)
        {
            return Exponent is null ? Default() : Exponent(baseExpression, powerExpression);
        }

        public T EvaluateFunction(IFunction function)
        {
            return Evaluate(Function, function);
        }

        public T EvaluateLn(IExpression argumentExpression)
        {
            return Evaluate(Ln, argumentExpression);
        }

        public virtual T EvaluateOther(IExpression other)
        {
            return Default();
        }

        public T EvaluateProduct(ICollection<IExpression> expressions)
        {
            return Evaluate(Product, expressions);
        }

        public T EvaluateSign(IExpression argumentExpression)
        {
            return Evaluate(Sign, argumentExpression);
        }

        public T EvaluateSin(IExpression argumentExpression)
        {
            return Evaluate(Sin, argumentExpression);
        }

        public T EvaluateSum(ICollection<IExpression> expressions)
        {
            return Evaluate(Sum, expressions);
        }

        public T EvaluateVariable(string name)
        {
            return Evaluate(Variable, name);
        }
    }
}
