using Rationals;
using System;


namespace Algebra
{
    namespace Atoms
    {
        internal class RationalConstant : Constant
        {

            private readonly Rational _value;

            private RationalConstant(Rational value)
            {
                this._value = value;
            }

            public static RationalConstant FromValue(Rational value)
            {
                return new RationalConstant(value.CanonicalForm);
            }

            public Rational GetValue()
            {
                return _value;
            }
        }
    }
}