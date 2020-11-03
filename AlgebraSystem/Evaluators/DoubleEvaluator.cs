using Algebra.Functions;
using Algebra.Functions.FunctionIdentities;
using Rationals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;

namespace Algebra.Evaluators
{
    public class DoubleEvaluator : ValueEvaluator<double>
    {
        public static readonly ReadOnlyDictionary<FunctionIdentity, FunctionEvaluator> DefaultFunctionEvaluators =
            new ReadOnlyDictionary<FunctionIdentity, FunctionEvaluator>(new Dictionary<FunctionIdentity, FunctionEvaluator>()
            {
                { AbsIdentity.Instance, d => Math.Abs(d[0]) },
                { ArccosIdentity.Instance, d => Math.Acos(d[0]) },
                { CoshIdentity.Instance, d => Math.Cosh(d[0]) },
                { CosIdentity.Instance, d => Math.Cos(d[0]) },
                { DivIdentity.Instance, d => d[0] / d[1] },
                { LogIdentity.Instance, d => Math.Log(d[0], d[1]) },
                { MaxIdentity.Instance, d => Math.Max(d[0], d[1]) },
                { MinIdentity.Instance, d => Math.Min(d[0], d[1]) },
                { SelectIdentity.Instance, d => UtilityMethods.Select(d[0], d[1], d[2]) },
                { SinhIdentity.Instance, d => Math.Sinh(d[0]) },
                { SqrtIdentity.Instance, d => Math.Sqrt(d[0]) },
                { TanhIdentity.Instance, d => Math.Tanh(d[0]) },
                { TanIdentity.Instance, d => Math.Tan(d[0]) },
            });

        public DoubleEvaluator(VariableInputSet<double> variableInputs, IDictionary<FunctionIdentity, FunctionEvaluator> functionEvaluators = null)
            : base(variableInputs, functionEvaluators ?? DefaultFunctionEvaluators)
        {

        }

        protected override double GetFromConstant(IConstant value)
        {
            return value.GetDoubleApproximation();
        }

        protected override double PowOf(double baseValue, double powerValue)
        {
            return Math.Pow(baseValue, powerValue);
        }

        protected override TraversalEvaluator<double> Construct(IDictionary<FunctionIdentity, FunctionEvaluator> functionEvaluators, VariableInputSet<double> variableInputs)
        {
            return new DoubleEvaluator(variableInputs, functionEvaluators);
        }

        protected override double LnOf(double v)
        {
            return Math.Log(v);
        }

        protected override double ProductOf(ICollection<double> expressions)
        {
            double evaluated = 1;
            foreach (double expression in expressions)
            {
                evaluated *= expression;
            }
            return evaluated;
        }

        protected override double SignOf(double v)
        {
            return double.IsNaN(v) ? double.NaN : Math.Sign(v);
        }

        protected override double SinOf(double v)
        {
            return Math.Sin(v);
        }

        protected override double SumOf(ICollection<double> expressions)
        {
            double evaluated = 0;
            foreach (double expression in expressions)
            {
                evaluated += expression;
            }
            return evaluated;
        }

        protected override double ArcsinOf(double v)
        {
            return Math.Asin(v);
        }

        protected override double ArctanOf(double v)
        {
            return Math.Atan(v);
        }

        public override double EvaluateOther(Expression other)
        {
            throw new NotImplementedException($"Cannot evaluate {other}");
        }
    }
}
