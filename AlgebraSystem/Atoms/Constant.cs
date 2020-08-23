using Rationals;
using System;


namespace Algebra
{
    namespace Atoms
    {
        internal class Constant : Expression, IAtomicExpression
        {
            public static implicit operator Constant(int r) => Constant.FromValue(r);
            public static implicit operator Constant(long r) => Constant.FromValue(r);
            public static implicit operator Constant(float r) => Rational.Approximate(r);
            public static implicit operator Constant(double r) => Rational.Approximate(r);
            public static implicit operator Constant(decimal r) => Rational.Approximate(r);
            public static implicit operator Constant(Rational r) => Constant.FromValue(r);

            private readonly Rational value;

            public static Constant FromValue(Rational value)
            {
                return new Constant(value.CanonicalForm);
            }

            private Constant(Rational value)
            {
                this.value = value;
            }

            public override IExpression GetDerivative(string wrt)
            {
                return Expression.Zero;
            }

            protected override bool ExactlyEquals(IExpression expression)
            {
                if (!(expression is Constant constant))
                {
                    return false;
                }

                return value.Equals(constant.value);
            }

            protected override int GenHashCode()
            {
                return value.GetHashCode();
            }

            public Rational GetValue()
            {
                return value;
            }

            public override string ToString()
            {
                return $"{value}";
            }

            public override int GetOrderIndex()
            {
                return 0;
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateConstant(value);
            }

            protected override IAtomicExpression GenAtomicExpression()
            {
                return this;
            }
        }
    }
}