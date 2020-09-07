using Rationals;
using System;


namespace Algebra
{
    namespace Atoms
    {
        internal class Constant : Expression
        {
            public static implicit operator Constant(int r) => Constant.FromValue(r);
            public static implicit operator Constant(long r) => Constant.FromValue(r);
            public static implicit operator Constant(float r) => Rational.Approximate(r);
            public static implicit operator Constant(double r) => Rational.Approximate(r);
            public static implicit operator Constant(decimal r) => Rational.Approximate(r);
            public static implicit operator Constant(Rational r) => Constant.FromValue(r);

            private readonly Rational _value;

            public static Constant FromValue(Rational value)
            {
                return new Constant(value.CanonicalForm);
            }

            private Constant(Rational value)
            {
                this._value = value;
            }

            public override Expression GetDerivative(string wrt)
            {
                return Expression.Zero;
            }

            protected override int GenHashCode()
            {
                return _value.GetHashCode();
            }

            public Rational GetValue()
            {
                return _value;
            }

            public override string ToString()
            {
                return $"{_value}";
            }

            public override int GetOrderIndex()
            {
                return 0;
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateConstant(_value);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateConstant(this, _value);
            }

            public override T Evaluate<T>(Expression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Constant other)
                {
                    return evaluator.EvaluateConstants(this._value, other._value);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }

            protected override Expression GenAtomicExpression()
            {
                return this;
            }
        }
    }
}