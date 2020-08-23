using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    class VariableReplacementEvaluator : TraversalEvaluator<IExpression>
    {
        private readonly IDictionary<string, IExpression> substitutions;

        public VariableReplacementEvaluator(IDictionary<string, IExpression> substitutions)
        {
            this.substitutions = substitutions;
        }

        public override IExpression EvaluateConstant(Rational value)
        {
            return Expression.ConstantFrom(value);
        }

        public override IExpression EvaluateVariable(string name)
        {
            if (substitutions.TryGetValue(name, out IExpression expression))
            {
                return expression;
            }
            throw new ArgumentException($"A substitution was not provided for variable {name}");
        }

        protected override IExpression Pow(IExpression b, IExpression e)
        {
            return Expression.Pow(b, e);
        }

        protected override IExpression EvaluateFunction(IFunction function, IList<IExpression> parameters)
        {
            return function.GetIdentity().CreateExpression(parameters);
        }

        protected override IExpression Ln(IExpression v)
        {
            return Expression.LnOf(v);
        }

        protected override IExpression Product(ICollection<IExpression> expressions)
        {
            return Expression.Multiply(expressions);
        }

        protected override IExpression Sign(IExpression v)
        {
            return Expression.SignOf(v);
        }

        protected override IExpression Sin(IExpression v)
        {
            return Expression.SinOf(v);
        }

        protected override IExpression Sum(ICollection<IExpression> expressions)
        {
            return Expression.Add(expressions);
        }
    }
}
