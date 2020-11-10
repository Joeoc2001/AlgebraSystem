using Algebra.Mappings;
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
    public class ReplaceMapping : IExtendedMapping<HashSet<Expression>>
    {
        private readonly Expression _patternExpression;
        private readonly Expression _replacementExpression;

        public ReplaceMapping(Expression patternExpression, Expression replacementExpression)
        {
            this._patternExpression = patternExpression ?? throw new ArgumentNullException();
            this._replacementExpression = replacementExpression ?? throw new ArgumentNullException();

            if (replacementExpression.GetVariables().Except(patternExpression.GetVariables()).Any())
            {
                throw new ArgumentException($"Expression {patternExpression} does not have all variables used within {replacementExpression}");
            }
        }

        protected HashSet<Expression> EvaluateExpression(Expression expression)
        {
            PatternMatchingResultSet matches = expression.Map(_patternExpression, PatternMatchingDualMapping.Instance);

            HashSet<Expression> result = new HashSet<Expression>();

            foreach (PatternMatchingResult match in matches)
            {
                VariableReplacementMapping replacementmapping = new VariableReplacementMapping(match, false);
                result.Add(_replacementExpression.Map(replacementmapping));
            }

            return result;
        }

        protected IEnumerable<Expression> EvaluateArgument(Expression argument, Func<Expression, Expression> argumentMap)
        {
            // Map
            foreach (var unmappedArgument in argument.Map(this))
            {
                yield return argumentMap(unmappedArgument);
            }
        }

        protected HashSet<Expression> EvaluateMonad(Expression expression, Expression argumentExpression, Func<Expression, Expression> argumentMap)
        {
            // Get this
            HashSet<Expression> result = EvaluateExpression(expression);

            // Get arguments
            result.UnionWith(EvaluateArgument(argumentExpression, argumentMap));

            return result;
        }

        public HashSet<Expression> EvaluateConstant(Expression expression, IConstant value)
        {
            return EvaluateExpression(expression);
        }

        public HashSet<Expression> EvaluateVariable(Expression expression, IVariable value)
        {
            return EvaluateExpression(expression);
        }

        public HashSet<Expression> EvaluateArcsin(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.ArcsinOf);
        }

        public HashSet<Expression> EvaluateArctan(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.ArctanOf);
        }

        public HashSet<Expression> EvaluateLn(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.LnOf);
        }

        public HashSet<Expression> EvaluateSign(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.SignOf);
        }

        public HashSet<Expression> EvaluateSin(Expression expression, Expression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.SinOf);
        }

        public HashSet<Expression> EvaluateExponent(Expression expression, Expression baseExpression, Expression powerExpression)
        {
            // Get this
            HashSet<Expression> results = EvaluateExpression(expression);

            // Get arguments
            var baseResults = EvaluateArgument(baseExpression, mapped => Expression.Pow(mapped, powerExpression));
            results.UnionWith(baseResults);

            var powerResults = EvaluateArgument(powerExpression, mapped => Expression.Pow(baseExpression, mapped));
            results.UnionWith(powerResults);

            return results;
        }

        public HashSet<Expression> EvaluateFunction(Function function)
        {
            FunctionIdentity identity = function.GetIdentity();

            // Get this
            HashSet<Expression> results = EvaluateExpression(function);

            // Get arguments
            foreach ((var one, var others) in LazyFunctions.TakeOne(function.GetParameters()))
            {
                Dictionary<string, Expression> othersDict = others.ToDictionary(x => x.Key, x => x.Value);
                Expression map(Expression mapped) => identity.CreateExpression(new Dictionary<string, Expression>(othersDict) { { one.Key, mapped } });
                var argumentResults = EvaluateArgument(one.Value, map);
                results.UnionWith(argumentResults);
            }

            return results;
        }

        protected HashSet<Expression> EvaluateCommutative(Expression expression, ICollection<Expression> expressions, Func<ICollection<Expression>, Expression> builder)
        {
            // Get this
            HashSet<Expression> results = EvaluateExpression(expression);

            // Get arguments
            foreach ((var inSet, var outSet) in LazyFunctions.DualPartitionNonEmpty(expressions))
            {
                Expression inExpression = builder(inSet);
                Expression map(Expression mapped) => builder(new List<Expression>(outSet) { { mapped } });
                var argumentResults = EvaluateArgument(inExpression, map);
                results.UnionWith(argumentResults);
            }

            return results;
        }

        public HashSet<Expression> EvaluateProduct(Expression expression, ICollection<Expression> expressions)
        {
            return EvaluateCommutative(expression, expressions, Expression.Multiply);
        }

        public HashSet<Expression> EvaluateSum(Expression expression, ICollection<Expression> expressions)
        {
            return EvaluateCommutative(expression, expressions, Expression.Add);
        }

        public virtual HashSet<Expression> EvaluateOther(Expression other)
        {
            throw new NotImplementedException($"Cannot replace for unknown expression {other}. Override {typeof(ReplaceMapping).Name} to add functionality for your new class.");
        }
    }
}
