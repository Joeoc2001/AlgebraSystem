using Algebra.Evaluators;
using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.PatternMatching
{
    /// <summary>
    /// Returns a set of expressions where all instance of a pattern have been replaced with a replacement expression.
    /// All of the variables in the replacement expression must be contained in the pattern expression.
    /// For example, if 3 * (x + y) + 2 is evaluated with an instance of this with pattern a + b and replacement a * b,
    /// the resulting expression set will be {6 * (x + y), 3 * x * y + 2}.
    /// This is useful for equality axioms, e.g. x * (y + z) == x * y + x * z
    /// </summary>
    public class ReplaceEvaluator : IExpandedEvaluator<IEnumerable<Expression>>
    {
        private readonly Expression _patternExpression;
        private readonly Expression _replacementExpression;

        public ReplaceEvaluator(Expression patternExpression, Expression replacementExpression)
        {
            this._patternExpression = patternExpression ?? throw new ArgumentNullException();
            this._replacementExpression = replacementExpression ?? throw new ArgumentNullException();

            if (replacementExpression.GetVariables().Except(patternExpression.GetVariables()).Any())
            {
                throw new ArgumentException($"Expression {patternExpression} does not have all variables used within {replacementExpression}");
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
            foreach (Expression result in results)
            {
                yield return result;
            }

            // Get arguments
            IEnumerable<Expression> baseResults = EvaluateArgument(baseExpression, mapped => Expression.Pow(mapped, powerExpression));
            foreach (Expression result in baseResults)
            {
                yield return result;
            }
            IEnumerable<Expression> powerResults = EvaluateArgument(powerExpression, mapped => Expression.Pow(baseExpression, mapped));
            foreach (Expression result in powerResults)
            {
                yield return result;
            }
        }

        public IEnumerable<Expression> EvaluateFunction(Function function)
        {
            FunctionIdentity identity = function.GetIdentity();

            // Get this
            foreach (Expression expression in EvaluateExpression(function))
            {
                yield return expression;
            }

            // Get arguments
            foreach ((var one, var others) in LazyFunctions.TakeOne(function.GetParameters()))
            {
                Expression map(Expression mapped) => identity.CreateExpression(new Dictionary<string, Expression>(others) { { one.Key, mapped } });
                foreach (Expression result in EvaluateArgument(one.Value, map))
                {
                    yield return result;
                }
            }
        }

        protected IEnumerable<Expression> EvaluateCommutative(Expression expression, ICollection<Expression> expressions, Func<ICollection<Expression>, Expression> builder)
        {
            // Get this
            foreach (Expression result in EvaluateExpression(expression))
            {
                yield return result;
            }

            // Get arguments
            foreach ((var inSet, var outSet) in LazyFunctions.DualPartitionNonEmpty(expressions))
            {
                Expression inExpression = builder(inSet);
                Expression map(Expression mapped) => builder(new List<Expression>(outSet) { { mapped } });
                foreach (Expression result in EvaluateArgument(inExpression, map))
                {
                    yield return result;
                }
            }
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
