using Rationals;
using System;


namespace Algebra.Atoms
{
    public class Constant : Expression
    {
        public static readonly Constant ZERO = 0;
        public static readonly Constant ONE = 1;
        public static readonly Constant MINUS_ONE = -1;
        public static readonly Constant PI = Math.PI;
        public static readonly Constant E = Math.E;

        public static implicit operator Constant(int r) => Constant.From(r);
        public static implicit operator Constant(long r) => Constant.From(r);
        public static implicit operator Constant(float r) => Rational.Approximate(r);
        public static implicit operator Constant(double r) => Rational.Approximate(r);
        public static implicit operator Constant(decimal r) => Rational.Approximate(r);
        public static implicit operator Constant(Rational r) => Constant.From(r);

        private readonly Rational value;

        public static Constant From(Rational value)
        {
            return new Constant(value.CanonicalForm);
        }

        private Constant(Rational value)
        {
            this.value = value;
        }

        public override ExpressionDelegate GetDelegate(VariableInputSet set)
        {
            float approximation = (float)value;
            return () => approximation;
        }

        public override Expression GetDerivative(Variable wrt)
        {
            return 0;
        }

        protected override bool ExactlyEquals(Expression expression)
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

        public override Expression MapChildren(ExpressionMapping.ExpressionMap map)
        {
            return this;
        }
    }
}