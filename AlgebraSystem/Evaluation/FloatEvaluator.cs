using Rationals;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Algebra.Evaluation
{
    public class FloatEvaluator : ValueEvaluator<float>
    {
        public FloatEvaluator(VariableInputSet<float> variableInputs)
            : this(variableInputs, null)
        { }

        public FloatEvaluator(VariableInputSet<float> variableInputs, IDictionary<IFunctionIdentity, FunctionEvaluator> functionEvaluators)
            : base(variableInputs, functionEvaluators)
        {

        }

        protected override float GetFromRational(Rational value)
        {
            return (float)value;
        }

        protected override float Pow(float baseValue, float powerValue)
        {
            return MathF.Pow(baseValue, powerValue);
        }

        protected override ValueEvaluator<float> Construct(IDictionary<IFunctionIdentity, FunctionEvaluator> functionEvaluators, VariableInputSet<float> variableInputs)
        {
            return new FloatEvaluator(variableInputs, functionEvaluators);
        }

        protected override float Ln(float v)
        {
            return MathF.Log(v);
        }

        protected override float Product(ICollection<float> expressions)
        {
            float evaluated = 1;
            foreach (float expression in expressions)
            {
                evaluated *= expression;
            }
            return evaluated;
        }

        protected override float Sign(float v)
        {
            return MathF.Sign(v);
        }

        protected override float Sin(float v)
        {
            return MathF.Sin(v);
        }

        protected override float Sum(ICollection<float> expressions)
        {
            float evaluated = 0;
            foreach (float expression in expressions)
            {
                evaluated += expression;
            }
            return evaluated;
        }
    }
}
