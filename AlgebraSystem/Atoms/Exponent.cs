using Rationals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Algebra
{
    namespace Atoms
    {
        internal class Exponent : Expression
        {
            private readonly Expression _term;
            private readonly Expression _power;

            new public static Expression Pow(Expression term, Expression power)
            {
                if (power.Equals(One))
                {
                    return term;
                }
                if (power.Equals(Zero)) // Controversial but assume that 0^0 == 1
                {
                    return One;
                }

                if (term is RationalConstant termConstant && power is RationalConstant exponentConstant)
                {
                    Rational numerator = exponentConstant.GetValue().Numerator;
                    Rational denominator = exponentConstant.GetValue().Denominator;
                    if (denominator == 1 && numerator > -10 && numerator < 10) // Bounds for sanity sake
                    {
                        Rational value = Rational.Pow(termConstant.GetValue(), (int)numerator);
                        return ConstantFrom(value);
                    }
                }

                return new Exponent(term, power);
            }

            public Exponent(Expression term, Expression power)
            {
                this._term = term ?? throw new ArgumentNullException(nameof(term));
                this._power = power ?? throw new ArgumentNullException(nameof(power));
            }

            public override Expression GetDerivative(string wrt)
            {
                // Check for common cases
                if (_power is RationalConstant powerConst)
                {
                    Expression baseDerivative = _term.GetDerivative(wrt);
                    return _power * baseDerivative * Pow(_term, ConstantFrom(powerConst.GetValue() - 1));
                }

                if (_term is RationalConstant)
                {
                    Expression exponentDerivative = _power.GetDerivative(wrt);
                    return LnOf(_term) * exponentDerivative * this;
                }

                // Big derivative (u^v)'=(u^v)(vu'/u + v'ln(u))
                // Alternatively  (u^v)'=(u^(v-1))(vu' + uv'ln(u)) but I find the first form simplifies faster
                Expression baseDeriv = _term.GetDerivative(wrt);
                Expression expDeriv = _power.GetDerivative(wrt);
                return this * ((_power * baseDeriv / _term) + (expDeriv * LnOf(_term)));
            }

            public Expression GetPower()
            {
                return _power;
            }

            public Expression GetTerm()
            {
                return _term;
            }

            protected override int GenHashCode()
            {
                return (65 * _term.GetHashCode() - _power.GetHashCode()) ^ 642859777;
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();

                builder.Append(ToParenthesisedString(this, _term));
                builder.Append("^");
                builder.Append(ToParenthesisedString(this, _power));

                return builder.ToString();
            }

            public override int GetOrderIndex()
            {
                return 10;
            }

            public override void Map(IMapping mapping)
            {
                mapping.EvaluateExponent(_term, _power);
            }

            public override T Map<T>(IMapping<T> mapping)
            {
                return mapping.EvaluateExponent(_term, _power);
            }

            public override T Map<T>(IExtendedMapping<T> mapping)
            {
                return mapping.EvaluateExponent(this, _term, _power);
            }

            public override T Map<T>(Expression otherExpression, IDualMapping<T> mapping)
            {
                if (otherExpression is Exponent other)
                {
                    return mapping.EvaluateExponents(this._term, this._power, other._term, other._power);
                }
                return mapping.EvaluateOthers(this, otherExpression);
            }

            protected override Expression GenAtomicExpression()
            {
                Expression baseAtomic = _term.GetAtomicExpression();
                Expression powerAtomic = _term.GetAtomicExpression();
                return Expression.Pow(baseAtomic, powerAtomic);
            }
        }
    }
}