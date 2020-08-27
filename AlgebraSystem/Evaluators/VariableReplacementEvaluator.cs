using Rationals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Evaluators
{
    class VariableReplacementEvaluator : TraversalEvaluator<IExpression>
    {
        private readonly IReadOnlyDictionary<string, IExpression> substitutions;
        private readonly bool exceptionOnVariableMissing;

        public VariableReplacementEvaluator(IReadOnlyDictionary<string, IExpression> substitutions, bool exceptionOnVariableMissing = true)
        {
            this.substitutions = substitutions;
            this.exceptionOnVariableMissing = exceptionOnVariableMissing;
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
            if (exceptionOnVariableMissing)
            {
                throw new ArgumentException($"A substitution was not provided for variable {name}");
            }
            else
            {
                return Expression.VariableFrom(name);
            }
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

        protected override IExpression Arcsin(IExpression expression)
        {
            return Expression.ArcsinOf(expression);
        }

        protected override IExpression Arctan(IExpression expression)
        {
            return Expression.ArctanOf(expression);
        }

        public override IExpression EvaluateOther(IExpression other)
        {
            throw new NotImplementedException($"Cannot replace variables of unknown expression {other}. Override {typeof(VariableReplacementEvaluator).Name} to add functionality for your new class.");
        }
    }
}
