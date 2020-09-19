using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.ConstantValues
{
    public class RealValue : IConstantValue, IEquatable<RealValue>
    {
        private readonly string _name;
        private readonly Rational _rational;

        public RealValue(string name, Rational rationalApproximation)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _rational = rationalApproximation;
        }

        public bool Equals(RealValue other)
        {
            if (other is null)
            {
                return false;
            }

            return _name.Equals(other._name) && _rational.Equals(other._rational);
        }

        public bool Equals(IConstantValue other)
        {
            return Equals(other as RealValue);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RealValue);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode() * 33 + _rational.GetHashCode();
        }

        public override string ToString()
        {
            return _name;
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
            return false;
        }
    }
}
