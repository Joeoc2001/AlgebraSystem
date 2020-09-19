using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public class GetAllSubtreesEvaluator : IExpandedEvaluator<HashSet<Expression>>
    {
        public static readonly GetAllSubtreesEvaluator Instance = new GetAllSubtreesEvaluator();

        protected GetAllSubtreesEvaluator()
        {

        }

        protected HashSet<Expression> EvaluateMonad(Expression expression, Expression argumentExpression)
        {
            HashSet<Expression> result = argumentExpression.Evaluate(this);
            result.Add(expression);
            return result;
        }

        protected HashSet<Expression> EvaluateSet(Expression expression, IEnumerable<Expression> parameters)
        {
            // Get this
            HashSet<Expression> results = new HashSet<Expression>() { expression };

            // Get arguments
            foreach (Expression parameter in parameters)
            {
                var argumentResults = parameter.Evaluate(this);
                results.UnionWith(argumentResults);
            }

            return results;
        }

        public HashSet<Expression> EvaluateArcsin(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression);
        }

        public HashSet<Expression> EvaluateArctan(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression);
        }

        public HashSet<Expression> EvaluateConstant(Expression expression, IConstant value)
        {
            return new HashSet<Expression>() { expression };
        }

        public HashSet<Expression> EvaluateExponent(Expression expression, Expression baseExpression, Expression powerExpression)
        {
            HashSet<Expression> baseResult = baseExpression.Evaluate(this);
            HashSet<Expression> powerResult = powerExpression.Evaluate(this);

            baseResult.UnionWith(powerResult);
            baseResult.Add(expression);

            return baseResult;
        }

        public HashSet<Expression> EvaluateFunction(Function function)
        {
            return EvaluateSet(function, function.GetParameterList());
        }

        public HashSet<Expression> EvaluateLn(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression);
        }

        public HashSet<Expression> EvaluateOther(Expression other)
        {
            throw new NotImplementedException($"Cannot replace for unknown expression {other}. Override {typeof(GetAllSubtreesEvaluator).Name} to add functionality for your new class.");
        }

        public HashSet<Expression> EvaluateProduct(Expression expression, ICollection<Expression> expressions)
        {
            return EvaluateSet(expression, expressions);
        }

        public HashSet<Expression> EvaluateSign(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression);
        }

        public HashSet<Expression> EvaluateSin(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression);
        }

        public HashSet<Expression> EvaluateSum(Expression expression, ICollection<Expression> expressions)
        {
            return EvaluateSet(expression, expressions);
        }

        public HashSet<Expression> EvaluateVariable(Expression expression, IVariable value)
        {
            return new HashSet<Expression>() { expression };
        }
    }
}
