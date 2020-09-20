﻿using Algebra.Functions;
using Algebra.Functions.FunctionIdentities;
using Rationals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;

namespace Algebra.Evaluators
{
    public class RationalEvaluator : ValueEvaluator<Rational>
    {
        public static readonly ReadOnlyDictionary<FunctionIdentity, FunctionEvaluator> DefaultFunctionEvaluators =
            new ReadOnlyDictionary<FunctionIdentity, FunctionEvaluator>(new Dictionary<FunctionIdentity, FunctionEvaluator>()
            {
                { AbsIdentity.Instance, d => Rational.Abs(d[0]) },
                { ArccosIdentity.Instance, d => (Rational)Math.Acos((double)d[0]) },
                { CoshIdentity.Instance, d => (Rational)Math.Cosh((double)d[0]) },
                { CosIdentity.Instance, d => (Rational)Math.Cos((double) d[0]) },
                { DivIdentity.Instance, d => d[0] / d[1] },
                { LogIdentity.Instance, d => (Rational)Math.Log((double)d[0], (double)d[1]) },
                { MaxIdentity.Instance, d => d[0] > d[1] ? d[0] : d[1] },
                { MinIdentity.Instance, d => d[0] < d[1] ? d[0] : d[1] },
                { SelectIdentity.Instance, d => d[2] < 0 ? d[0] : (d[2] > 0 ? d[1] : (d[0] + d[1]) / 2) },
                { SinhIdentity.Instance, d => (Rational)Math.Sinh((double)d[0]) },
                { SqrtIdentity.Instance, d => (Rational)Math.Sqrt((double)d[0]) },
                { TanhIdentity.Instance, d => (Rational)Math.Tanh((double)d[0]) },
                { TanIdentity.Instance, d => (Rational)Math.Tan((double)d[0]) },
            });

        private readonly BigInteger? _maxSize;

        public RationalEvaluator(VariableInputSet<Rational> variableInputs)
            : this(variableInputs, null, null)
        { }

        public RationalEvaluator(VariableInputSet<Rational> variableInputs, BigInteger maxSize)
            : this(variableInputs, null, maxSize)
        { }

        public RationalEvaluator(VariableInputSet<Rational> variableInputs, IDictionary<FunctionIdentity, FunctionEvaluator> functionEvaluators)
            : this(variableInputs, functionEvaluators, null)
        { }

        public RationalEvaluator(VariableInputSet<Rational> variableInputs, IDictionary<FunctionIdentity, FunctionEvaluator> functionEvaluators, BigInteger? maxSize)
            : base(variableInputs, functionEvaluators)
        {
            this._maxSize = maxSize;
        }

        protected override Rational Map(Rational value)
        {
            value = value.CanonicalForm;
            if (_maxSize.HasValue)
            {
                BigInteger division = BigInteger.Abs(value.Numerator / _maxSize.Value);
                division = BigInteger.Max(division, BigInteger.Abs(value.Denominator / _maxSize.Value));
                division = BigInteger.Min(division, BigInteger.Abs(value.Numerator));
                division = BigInteger.Min(division, BigInteger.Abs(value.Denominator));
                if (division > 1)
                {
                    value = new Rational(value.Numerator / division, value.Denominator / division);
                }
            }
            return value;
        }

        protected override Rational GetFromConstant(IConstant value)
        {
            return value.GetRationalApproximation();
        }

        protected override Rational PowOf(Rational baseValue, Rational powerValue)
        {
            if (powerValue.Numerator > int.MaxValue || powerValue.Numerator < int.MinValue
                   || powerValue.Denominator > int.MaxValue || powerValue.Denominator < int.MinValue)
            {
                throw new ArgumentOutOfRangeException("Power is outside of int range. Try running again as an approximation");
            }

            Rational finalValue = Rational.Pow(baseValue, (int)powerValue.Numerator);
            return Rational.RationalRoot(finalValue, (int)powerValue.Denominator);
        }

        protected override TraversalEvaluator<Rational> Construct(IDictionary<FunctionIdentity, FunctionEvaluator> functionEvaluators, VariableInputSet<Rational> variableInputs)
        {
            return new RationalEvaluator(variableInputs, functionEvaluators, _maxSize);
        }

        protected override Rational LnOf(Rational v)
        {
            return (Rational)Rational.Log(v);
        }

        protected override Rational ProductOf(ICollection<Rational> expressions)
        {
            Rational evaluated = 1;
            foreach (Rational expression in expressions)
            {
                evaluated *= expression;
            }
            return evaluated;
        }

        protected override Rational SignOf(Rational v)
        {
            return v.Sign;
        }

        protected override Rational SinOf(Rational v)
        {
            return (Rational)Math.Sin((double)v);
        }

        protected override Rational SumOf(ICollection<Rational> expressions)
        {
            Rational evaluated = 0;
            foreach (Rational expression in expressions)
            {
                evaluated += expression;
            }
            return evaluated;
        }

        protected override Rational ArcsinOf(Rational v)
        {
            return (Rational)Math.Asin((double)v);
        }

        protected override Rational ArctanOf(Rational v)
        {
            return (Rational)Math.Atan((double)v);
        }

        public override Rational EvaluateOther(Expression other)
        {
            throw new NotImplementedException($"Cannot evaluate {other}. Override {typeof(RationalEvaluator).Name} to add functionality for your new class.");
        }
    }
}
