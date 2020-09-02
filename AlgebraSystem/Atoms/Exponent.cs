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
                if (power.Equals(Zero))
                {
                    return One;
                }

                if (term is Constant termConstant && power is Constant exponentConstant)
                {
                    Rational numerator = exponentConstant.GetValue().Numerator;
                    Rational denominator = exponentConstant.GetValue().Denominator;
                    if (numerator > -10 && numerator < 10) // Bounds for sanity sake
                    {
                        Rational value = Rational.Pow(termConstant.GetValue(), (int)numerator);
                        if (value >= 0)
                        {
                            value = Rational.RationalRoot(value, (int)denominator);
                            return ConstantFrom(value);
                        }
                    }
                }

                return new Exponent(term, power);
            }

            public Exponent(Expression term, Expression power)
            {
                this._term = term;
                this._power = power;
            }

            public override Expression GetDerivative(string wrt)
            {
                // Check for common cases
                if (_power is Constant powerConst)
                {
                    Expression baseDerivative = _term.GetDerivative(wrt);
                    return _power * baseDerivative * Pow(_term, ConstantFrom(powerConst.GetValue() - 1));
                }

                if (_term is Constant)
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

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateExponent(_term, _power);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateExponent(this, _term, _power);
            }

            public override T Evaluate<T>(Expression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Exponent other)
                {
                    return evaluator.EvaluateExponents(this._term, this._power, other._term, other._power);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
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