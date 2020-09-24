using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Atoms
    {
        internal class RealConstant : Constant, IEquatable<RealConstant>
        {
            private readonly Rational _value;
            private readonly string _name;

            private RealConstant(Rational value, string name)
            {
                _value = value.CanonicalForm;
                _name = name ?? throw new ArgumentNullException(nameof(name));
            }

            public static RealConstant FromApproximation(string name, Rational approximation)
            {
                return new RealConstant(approximation, name);
            }

            public override bool Equals(IConstant other)
            {
                return Equals(other as RealConstant);
            }

            public bool Equals(RealConstant other)
            {
                if (other is null)
                {
                    return false;
                }

                return _value.Equals(other._value) && _name.Equals(other._name);
            }

            public override double GetDoubleApproximation()
            {
                return (double)_value;
            }

            public override Rational GetRationalApproximation()
            {
                return _value;
            }

            public override bool IsRational()
            {
                return false;
            }

            public override string ToString()
            {
                return _name;
            }

            protected override int GenHashCode()
            {
                return _name.GetHashCode() * 33 + _value.GetHashCode();
            }
        }
    }
}
