using Algebra.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.Equivalence
{
    public class DefaultSimplificationMetric : IExpressionMetric, IMapping<double>
    {
        public static readonly DefaultSimplificationMetric DefaultInstance = new DefaultSimplificationMetric(FunctionIdentitySet.DefaultFunctionIdentities);

        private readonly HashSet<FunctionIdentity> _optimisedFunctions;
        public DefaultSimplificationMetric(IEnumerable<FunctionIdentity> optimisedFunctions)
        {
            _optimisedFunctions = new HashSet<FunctionIdentity>(optimisedFunctions);
        }

        public double Calculate(Expression expression)
        {
            return expression.Map(this);
        }

        public double EvaluateArcsin(Expression argumentExpression)
        {
            return 1 + argumentExpression.Map(this);
        }

        public double EvaluateArctan(Expression argumentExpression)
        {
            return 1 + argumentExpression.Map(this);
        }

        public double EvaluateConstant(IConstant value)
        {
            return 0;
        }

        public double EvaluateExponent(Expression baseExpression, Expression powerExpression)
        {
            return 1 + baseExpression.Map(this) + powerExpression.Map(this);
        }

        public double EvaluateFunction(Function function)
        {
            if (_optimisedFunctions.Contains(function.GetIdentity()))
            {
                return 1 + function.GetParameterList().Select(e => e.Map(this)).Sum();
            }

            return function.GetAtomicBodiedExpression().Map(this);
        }

        public double EvaluateLn(Expression argumentExpression)
        {
            return 1 + argumentExpression.Map(this);
        }

        public double EvaluateOther(Expression other)
        {
            throw new NotImplementedException($"Expression {other} not supported");
        }

        public double EvaluateProduct(ICollection<Expression> expressions)
        {
            return (expressions.Count - 1) + expressions.Select(e => e.Map(this)).Sum();
        }

        public double EvaluateSign(Expression argumentExpression)
        {
            return 1 + argumentExpression.Map(this);
        }

        public double EvaluateSin(Expression argumentExpression)
        {
            return 1 + argumentExpression.Map(this);
        }

        public double EvaluateSum(ICollection<Expression> expressions)
        {
            return (expressions.Count - 1) + expressions.Select(e => e.Map(this)).Sum();
        }

        public double EvaluateVariable(IVariable value)
        {
            return 0;
        }
    }
}
