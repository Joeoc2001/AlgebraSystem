﻿using Rationals;
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
            public readonly Expression Base;
            public readonly Expression Power;

            new public static Expression Pow(Expression term, Expression power)
            {
                if (power.Equals(ONE))
                {
                    return term;
                }
                if (power.Equals(ZERO))
                {
                    return ONE;
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
                this.Base = term;
                this.Power = power;
            }

            public override Expression GetDerivative(string wrt)
            {
                // Check for common cases
                if (Power is Constant powerConst)
                {
                    Expression baseDerivative = Base.GetDerivative(wrt);
                    return Power * baseDerivative * Pow(Base, ConstantFrom(powerConst.GetValue() - 1));
                }

                if (Base is Constant)
                {
                    Expression exponentDerivative = Power.GetDerivative(wrt);
                    return LnOf(Base) * exponentDerivative * this;
                }

                // Big derivative (u^v)'=(u^v)(vu'/u + v'ln(u))
                // Alternatively  (u^v)'=(u^(v-1))(vu' + uv'ln(u)) but I find the first form simplifies faster
                Expression baseDeriv = Base.GetDerivative(wrt);
                Expression expDeriv = Power.GetDerivative(wrt);
                return this * ((Power * baseDeriv / Base) + (expDeriv * LnOf(Base)));
            }

            protected override bool ExactlyEquals(Expression expression)
            {
                if (!(expression is Exponent exponent))
                {
                    return false;
                }

                return Base.Equals(exponent.Base) && Power.Equals(exponent.Power);
            }

            protected override int GenHashCode()
            {
                return (65 * Base.GetHashCode() - Power.GetHashCode()) ^ 642859777;
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();

                builder.Append(ToParenthesisedString(Base));
                builder.Append("^");
                builder.Append(ToParenthesisedString(Power));

                return builder.ToString();
            }

            public override int GetOrderIndex()
            {
                return 10;
            }

            public override Expression MapChildren(ExpressionMapping.ExpressionMap map)
            {
                Expression mappedBase = map(Base);
                Expression mappedPower = map(Power);

                return Pow(mappedBase, mappedPower);
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateExponent(Base, Power);
            }
        }
    }
}