using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.mappings
{
    class VariableReplacementMapping : TraversalMapping<Expression>
    {
        private readonly IReadOnlyDictionary<string, Expression> _substitutions;
        private readonly bool _exceptionOnVariableMissing;

        public VariableReplacementMapping(IReadOnlyDictionary<string, Expression> substitutions, bool exceptionOnVariableMissing = true)
        {
            this._substitutions = substitutions;
            this._exceptionOnVariableMissing = exceptionOnVariableMissing;
        }

        public override Expression EvaluateConstant(IConstant value)
        {
            return value.ToExpression();
        }

        public override Expression EvaluateVariable(IVariable value)
        {
            string name = value.GetName();
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
    }
}
