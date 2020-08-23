﻿using Rationals;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Algebra.Evaluation
{
    public class RationalEvaluator : ValueEvaluator<Rational>
    {
        private readonly BigInteger? maxSize;

        public RationalEvaluator(VariableInputSet<Rational> variableInputs)
            : this(variableInputs, null, null)
        { }

        public RationalEvaluator(VariableInputSet<Rational> variableInputs, BigInteger maxSize)
            : this(variableInputs, null, maxSize)
        { }

        public RationalEvaluator(VariableInputSet<Rational> variableInputs, IDictionary<IFunctionIdentity, FunctionEvaluator> functionEvaluators)
            : this(variableInputs, functionEvaluators, null)
        { }

        public RationalEvaluator(VariableInputSet<Rational> variableInputs, IDictionary<IFunctionIdentity, FunctionEvaluator> functionEvaluators, BigInteger? maxSize)
            : base(variableInputs, functionEvaluators)
        {
            this.maxSize = maxSize;
        }

        protected override Rational Map(Rational value)
        {
            value = value.CanonicalForm;
            if (maxSize.HasValue)
            {
                BigInteger division = BigInteger.Abs(value.Numerator / maxSize.Value);
                division = BigInteger.Max(division, BigInteger.Abs(value.Denominator / maxSize.Value));
                division = BigInteger.Min(division, BigInteger.Abs(value.Numerator));
                division = BigInteger.Min(division, BigInteger.Abs(value.Denominator));
                if (division > 1)
                {
                    value = new Rational(value.Numerator / division, value.Denominator / division);
                }
            }
            return value;
        }

        protected override Rational GetFromRational(Rational value)
        {
            return value;
        }

        protected override Rational Pow(Rational baseValue, Rational powerValue)
        {
            if (powerValue.Numerator > int.MaxValue || powerValue.Numerator < int.MinValue
                   || powerValue.Denominator > int.MaxValue || powerValue.Denominator < int.MinValue)
            {
                throw new ArgumentOutOfRangeException("Power is outside of int range. Try running again as an approximation");
            }

            Rational finalValue = Rational.Pow(baseValue, (int)powerValue.Numerator);
            return Rational.RationalRoot(finalValue, (int)powerValue.Denominator);
        }

        protected override ValueEvaluator<Rational> Construct(IDictionary<IFunctionIdentity, FunctionEvaluator> functionEvaluators, VariableInputSet<Rational> variableInputs)
        {
            return new RationalEvaluator(variableInputs, functionEvaluators, maxSize);
        }

        protected override Rational Ln(Rational v)
        {
            return (Rational)Rational.Log(v);
        }

        protected override Rational Product(ICollection<Rational> expressions)
        {
            Rational evaluated = 1;
            foreach (Rational expression in expressions)
            {
                evaluated *= expression;
            }
            return evaluated;
        }

        protected override Rational Sign(Rational v)
        {
            return v.Sign;
        }

        protected override Rational Sin(Rational v)
        {
            return (Rational)Math.Sin((double)v);
        }

        protected override Rational Sum(ICollection<Rational> expressions)
        {
            Rational evaluated = 0;
            foreach (Rational expression in expressions)
            {
                evaluated += expression;
            }
            return evaluated;
        }
    }
}
