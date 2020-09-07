using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Evaluators
{
    class VariableReplacementEvaluator : TraversalEvaluator<Expression>
    {
        private readonly IReadOnlyDictionary<string, Expression> _substitutions;
        private readonly bool _exceptionOnVariableMissing;

        public VariableReplacementEvaluator(IReadOnlyDictionary<string, Expression> substitutions, bool exceptionOnVariableMissing = true)
        {
            this._substitutions = substitutions;
            this._exceptionOnVariableMissing = exceptionOnVariableMissing;
        }

        public override Expression EvaluateConstant(Rational value)
        {
            return Expression.ConstantFrom(value);
        }

        public override Expression EvaluateVariable(string name)
        {
            if (_substitutions.TryGetValue(name, out Expression expression))
            {
                return expression;
            }
            if (_exceptionOnVariableMissing)
            {
                throw new ArgumentException($"A substitution was not provided for variable {name}");
            }
            else
            {
                return Expression.VariableFrom(name);
            }
        }

        protected override Expression Pow(Expression b, Expression e)
        {
            return Expression.Pow(b, e);
        }

        protected override Expression EvaluateFunction(Function function, IList<Expression> parameters)
        {
            return function.GetIdentity().CreateExpression(parameters);
        }

        protected override Expression Ln(Expression v)
        {
            return Expression.LnOf(v);
        }

        protected override Expression Product(ICollection<Expression> expressions)
        {
            return Expression.Multiply(expressions);
        }

        protected override Expression Sign(Expression v)
        {
            return Expression.SignOf(v);
        }

        protected override Expression Sin(Expression v)
        {
            return Expression.SinOf(v);
        }

        protected override Expression Sum(ICollection<Expression> expressions)
        {
            return Expression.Add(expressions);
        }

        protected override Expression Arcsin(Expression expression)
        {
            return Expression.ArcsinOf(expression);
        }

        protected override Expression Arctan(Expression expression)
        {
            return Expression.ArctanOf(expression);
        }

        public override Expression EvaluateOther(Expression other)
        {
            throw new NotImplementedException($"Cannot replace variables of unknown expression {other}. Override {typeof(VariableReplacementEvaluator).Name} to add functionality for your new class.");
        }
    }
}
