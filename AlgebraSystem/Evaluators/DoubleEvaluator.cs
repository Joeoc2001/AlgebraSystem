using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Algebra.Evaluators
{
    public class DoubleEvaluator : ValueEvaluator<double>
    {
        public DoubleEvaluator(VariableInputSet<double> variableInputs)
            : this(variableInputs, null)
        { }

        public DoubleEvaluator(VariableInputSet<double> variableInputs, IDictionary<FunctionIdentity, FunctionEvaluator> functionEvaluators)
            : base(variableInputs, functionEvaluators)
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
            return Math.Sign(v);
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
