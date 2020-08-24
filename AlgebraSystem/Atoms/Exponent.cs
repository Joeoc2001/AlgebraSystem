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
            private readonly IExpression term;
            private readonly IExpression power;

            new public static IExpression Pow(IExpression term, IExpression power)
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

            public Exponent(IExpression term, IExpression power)
            {
                this.term = term;
                this.power = power;
            }

            public override IExpression GetDerivative(string wrt)
            {
                // Check for common cases
                if (power is Constant powerConst)
                {
                    IExpression baseDerivative = term.GetDerivative(wrt);
                    return power * baseDerivative * Pow(term, ConstantFrom(powerConst.GetValue() - 1));
                }

                if (term is Constant)
                {
                    IExpression exponentDerivative = power.GetDerivative(wrt);
                    return LnOf(term) * exponentDerivative * this;
                }

                // Big derivative (u^v)'=(u^v)(vu'/u + v'ln(u))
                // Alternatively  (u^v)'=(u^(v-1))(vu' + uv'ln(u)) but I find the first form simplifies faster
                IExpression baseDeriv = term.GetDerivative(wrt);
                IExpression expDeriv = power.GetDerivative(wrt);
                return this * ((power * baseDeriv / term) + (expDeriv * LnOf(term)));
            }

            protected override bool ExactlyEquals(IExpression expression)
            {
                if (!(expression is Exponent exponent))
                {
                    return false;
                }

                return term.Equals(exponent.term) && power.Equals(exponent.power);
            }

            protected override int GenHashCode()
            {
                return (65 * term.GetHashCode() - power.GetHashCode()) ^ 642859777;
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();

                builder.Append(ToParenthesisedString(this, term));
                builder.Append("^");
                builder.Append(ToParenthesisedString(this, power));

                return builder.ToString();
            }

            public override int GetOrderIndex()
            {
                return 10;
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateExponent(term, power);
            }

            public override T DualEvaluate<T>(IExpression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Exponent other)
                {
                    return evaluator.EvaluateExponents(this.term, this.power, other.term, other.power);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }

            protected override IAtomicExpression GenAtomicExpression()
            {
                IAtomicExpression baseAtomic = term.GetAtomicExpression();
                IAtomicExpression powerAtomic = term.GetAtomicExpression();
                return AtomicExpression.GetAtomicExpression(Expression.Pow(baseAtomic, powerAtomic));
            }
        }
    }
}