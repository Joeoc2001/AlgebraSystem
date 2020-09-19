using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.ConstantValues
{
    public class RationalValue : IConstantValue, IEquatable<RationalValue>
    {
        private readonly Rational _rational;

        public RationalValue(Rational rational)
        {
            _rational = rational.CanonicalForm;
        }

        public double GetDoubleApproximation()
        {
            return (double)_rational;
        }

        public Rational GetRationalApproximation()
        {
            return _rational;
        }

        public bool IsRational()
        {
            return true;
        }

        public override string ToString()
        {
            return _rational.ToString("C");
        }

        public bool Equals(RationalValue obj)
        {
            if (obj is null)
            {
                return false;
            }
            return _rational.Equals(obj._rational);
        }

        public bool Equals(IConstantValue obj)
        {
            return Equals(obj as RationalValue);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RationalValue);
        }

        public override int GetHashCode()
        {
            return _rational.GetHashCode();
        }
    }
}
