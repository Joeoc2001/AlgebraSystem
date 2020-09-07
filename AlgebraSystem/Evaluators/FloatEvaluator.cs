using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Algebra.Evaluators
{
    public class FloatEvaluator : ValueEvaluator<float>
    {
        public FloatEvaluator(VariableInputSet<float> variableInputs)
            : this(variableInputs, null)
        { }

        public FloatEvaluator(VariableInputSet<float> variableInputs, IDictionary<FunctionIdentity, FunctionEvaluator> functionEvaluators)
            : base(variableInputs, functionEvaluators)
        {

        }

        protected override float GetFromRational(Rational value)
        {
            return (float)value;
        }

        protected override float PowOf(float baseValue, float powerValue)
        {
            return MathF.Pow(baseValue, powerValue);
        }

        protected override TraversalEvaluator<float> Construct(IDictionary<FunctionIdentity, FunctionEvaluator> functionEvaluators, VariableInputSet<float> variableInputs)
        {
            return new FloatEvaluator(variableInputs, functionEvaluators);
        }

        protected override float LnOf(float v)
        {
            return MathF.Log(v);
        }

        protected override float ProductOf(ICollection<float> expressions)
        {
            float evaluated = 1;
            foreach (float expression in expressions)
            {
                evaluated *= expression;
            }
            return evaluated;
        }

        protected override float SignOf(float v)
        {
            return MathF.Sign(v);
        }

        protected override float SinOf(float v)
        {
            return MathF.Sin(v);
        }

        protected override float SumOf(ICollection<float> expressions)
        {
            float evaluated = 0;
            foreach (float expression in expressions)
            {
                evaluated += expression;
            }
            return evaluated;
        }

        protected override float ArcsinOf(float v)
        {
            return MathF.Asin(v);
        }

        protected override float ArctanOf(float v)
        {
            return MathF.Atan(v);
        }

        public override float EvaluateOther(Expression other)
        {
            throw new NotImplementedException($"Cannot evaluate {other}");
        }
    }
}
