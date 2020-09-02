using Algebra.Evaluators;
using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.PatternMatching
{
    public class ReplaceEvaluator : IExpandedEvaluator<IEnumerable<Expression>>
    {
        private readonly Expression _patternExpression;
        private readonly Expression _replacementExpression;

        public ReplaceEvaluator(Expression patternExpression, Expression replacementExpression)
        {
            this._patternExpression = patternExpression ?? throw new ArgumentNullException();
            this._replacementExpression = replacementExpression ?? throw new ArgumentNullException();

            if (patternExpression.GetVariables().Except(replacementExpression.GetVariables()).Any())
            {
                throw new ArgumentException($"Expression {nameof(replacementExpression)} does not have all variables used within {nameof(replacementExpression)}");
            }
        }

        protected IEnumerable<Expression> EvaluateExpression(Expression expression)
        {
            PatternMatchingResultSet matches = expression.Evaluate(_patternExpression, PatternMatchingDualEvaluator.Instance);

            foreach (PatternMatchingResult result in matches)
            {
                VariableReplacementEvaluator replacementEvaluator = new VariableReplacementEvaluator(result, false);
                yield return _replacementExpression.Evaluate(replacementEvaluator);
            }
        }

        protected IEnumerable<Expression> EvaluateArgument(Expression argument, Func<Expression, Expression> argumentMap)
        {
            IEnumerable<Expression> unmappedArgumentResults = argument.Evaluate(this);

            // Map
            foreach (var unmappedArgument in unmappedArgumentResults)
            {
                yield return argumentMap(unmappedArgument);
            }
        }

        protected IEnumerable<Expression> EvaluateMonad(Expression expression, Expression argumentExpression, Func<Expression, Expression> argumentMap)
        {
            // Get this
            foreach (Expression result in EvaluateExpression(expression))
            {
                yield return result;
            }

            // Get arguments
            foreach (Expression result in EvaluateArgument(argumentExpression, argumentMap))
            {
                yield return result;
            }
        }

        /// <summary>
        /// Loops over all pairs of values and all other values in a collection
        /// </summary>
        protected IEnumerable<(T value, List<T>)> TakeOne<T>(ICollection<T> all)
        {
            List<T> vals = new List<T>(all);

            for (int i = 0; i < all.Count; i++)
            {
                T val = vals[0];
                vals.RemoveAt(0);
                yield return (val, vals);
                vals.Add(val);
            }
        }

        public IEnumerable<Expression> EvaluateConstant(Expression expression, Rational value)
        {
            return EvaluateExpression(expression);
        }

        public IEnumerable<Expression> EvaluateVariable(Expression expression, string name)
        {
            return EvaluateExpression(expression);
        }

        public IEnumerable<Expression> EvaluateArcsin(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.ArcsinOf);
        }

        public IEnumerable<Expression> EvaluateArctan(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.ArctanOf);
        }

        public IEnumerable<Expression> EvaluateLn(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.LnOf);
        }

        public IEnumerable<Expression> EvaluateSign(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.SignOf);
        }

        public IEnumerable<Expression> EvaluateSin(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.SinOf);
        }

        public IEnumerable<Expression> EvaluateExponent(Expression expression, Expression baseExpression, Expression powerExpression)
        {
            // Get this
            IEnumerable<Expression> results = EvaluateExpression(expression);

            // Get arguments
            IEnumerable<Expression> baseResults = EvaluateArgument(baseExpression, mapped => Expression.Pow(mapped, powerExpression));
            IEnumerable<Expression> powerResults = EvaluateArgument(powerExpression, mapped => Expression.Pow(baseExpression, mapped));

            // Return all
            return results.Concat(baseResults).Concat(powerResults);
        }

        public IEnumerable<Expression> EvaluateFunction(Function function)
        {
            FunctionIdentity identity = function.GetIdentity();

            // Get this
            IEnumerable<Expression> results = EvaluateExpression(function);

            // Get arguments
            foreach ((var one, var others) in TakeOne(function.GetParameters()))
            {
                Expression map(Expression mapped) => identity.CreateExpression(new Dictionary<string, Expression>(others) { { one.Key, mapped } });
                IEnumerable<Expression> argumentResults = EvaluateArgument(one.Value, map);
                results = results.Concat(argumentResults);
            }

            // Return all
            return results;
        }

        protected IEnumerable<Expression> EvaluateCommutative(Expression expression, ICollection<Expression> expressions, Func<ICollection<Expression>, Expression> builder)
        {
            // Get this
            IEnumerable<Expression> results = EvaluateExpression(expression);

            // Get arguments
            foreach ((var one, var others) in TakeOne(expressions))
            {
                Expression map(Expression mapped) => builder(new List<Expression>(others) { { mapped } });
                IEnumerable<Expression> argumentResults = EvaluateArgument(one, map);
                results = results.Concat(argumentResults);
            }

            // Return all
            return results;
        }

        public IEnumerable<Expression> EvaluateProduct(Expression expression, ICollection<Expression> expressions)
        {
            return EvaluateCommutative(expression, expressions, Expression.Multiply);
        }

        public IEnumerable<Expression> EvaluateSum(Expression expression, ICollection<Expression> expressions)
        {
            return EvaluateCommutative(expression, expressions, Expression.Add);
        }

        public virtual IEnumerable<Expression> EvaluateOther(Expression other)
        {
            throw new NotImplementedException($"Cannot replace for unknown expression {other}. Override {typeof(ReplaceEvaluator).Name} to add functionality for your new class.");
        }
    }
}
