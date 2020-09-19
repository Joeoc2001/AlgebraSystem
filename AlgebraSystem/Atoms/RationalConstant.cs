using Rationals;
using System;


namespace Algebra
{
    namespace Atoms
    {
        internal class RationalConstant : Constant, IEquatable<RationalConstant>
        {
            public static implicit operator RationalConstant(int r) => FromValue(r);
            public static implicit operator RationalConstant(long r) => FromValue(r);
            public static implicit operator RationalConstant(float r) => Rational.Approximate(r);
            public static implicit operator RationalConstant(double r) => Rational.Approximate(r);
            public static implicit operator RationalConstant(decimal r) => Rational.Approximate(r);
            public static implicit operator RationalConstant(Rational r) => FromValue(r);

            private readonly Rational _value;

            private RationalConstant(Rational value)
            {
                this._value = value.CanonicalForm;
            }

            public static RationalConstant FromValue(Rational value)
            {
                return new RationalConstant(value);
            }

            public override bool Equals(IConstant other)
            {
                return Equals(other as RationalConstant);
            }

            public bool Equals(RationalConstant other)
            {
                if (other is null)
                {
                    return false;
                }

                return _value.Equals(other._value);
            }

            public override double GetDoubleApproximation()
            {
                return (double)_value;
            }

            public override Rational GetRationalApproximation()
            {
                return _value;
            }

            public Rational GetValue()
            {
                return _value;
            }

            public override bool IsRational()
            {
                return true;
            }

            public override string ToString()
            {
                return _value.ToString("C");
            }

            protected override int GenHashCode()
            {
                return _value.GetHashCode();
            }
        }
    }
}