using Algebra.Evaluators;
using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.PatternMatching
{
    public class ReplaceEvaluator : IExpandedEvaluator<HashSet<IExpression>>
    {
        private readonly IExpression patternExpression;
        private readonly IExpression replacementExpression;

        public ReplaceEvaluator(IExpression patternExpression, IExpression replacementExpression)
        {
            this.patternExpression = patternExpression ?? throw new ArgumentNullException();
            this.replacementExpression = replacementExpression ?? throw new ArgumentNullException();

            if (patternExpression.GetVariables().Except(replacementExpression.GetVariables()).Any())
            {
                throw new ArgumentException($"Expression {nameof(replacementExpression)} does not have all variables used within {nameof(replacementExpression)}");
            }
        }

        protected HashSet<IExpression> EvaluateExpression(IExpression expression)
        {
            PatternMatchingResultSet matches = expression.Evaluate(patternExpression, PatternMatchingDualEvaluator.Instance);

            HashSet<IExpression> expressions = new HashSet<IExpression>();

            foreach (PatternMatchingResult result in matches)
            {
                VariableReplacementEvaluator replacementEvaluator = new VariableReplacementEvaluator(result, false);
                expressions.Add(replacementExpression.Evaluate(replacementEvaluator));
            }

            return expressions;
        }

        protected HashSet<IExpression> EvaluateArgument(IExpression argument, Func<IExpression, IExpression> argumentMap)
        {
            HashSet<IExpression> unmappedArgumentResults = argument.Evaluate(this);

            // Map
            HashSet<IExpression> argumentResults = new HashSet<IExpression>();
            foreach (var unmappedArgument in unmappedArgumentResults)
            {
                argumentResults.Add(argumentMap(unmappedArgument));
            }

            return argumentResults;
        }

        protected HashSet<IExpression> EvaluateMonad(IExpression expression, IExpression argumentExpression, Func<IExpression, IExpression> argumentMap)
        {
            // Get this
            HashSet<IExpression> results = EvaluateExpression(expression);

            // Get arguments
            HashSet<IExpression> argumentResults = EvaluateArgument(argumentExpression, argumentMap);

            // Return both
            results.UnionWith(argumentResults);
            return results;
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

        public HashSet<IExpression> EvaluateConstant(IExpression expression, Rational value)
        {
            return EvaluateExpression(expression);
        }

        public HashSet<IExpression> EvaluateVariable(IExpression expression, string name)
        {
            return EvaluateExpression(expression);
        }

        public HashSet<IExpression> EvaluateArcsin(IExpression expression, IExpression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.ArcsinOf);
        }

        public HashSet<IExpression> EvaluateArctan(IExpression expression, IExpression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.ArctanOf);
        }

        public HashSet<IExpression> EvaluateLn(IExpression expression, IExpression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.LnOf);
        }

        public HashSet<IExpression> EvaluateSign(IExpression expression, IExpression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.SignOf);
        }

        public HashSet<IExpression> EvaluateSin(IExpression expression, IExpression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.SinOf);
        }

        public HashSet<IExpression> EvaluateExponent(IExpression expression, IExpression baseExpression, IExpression powerExpression)
        {
            // Get this
            HashSet<IExpression> results = EvaluateExpression(expression);

            // Get arguments
            HashSet<IExpression> baseResults = EvaluateArgument(baseExpression, mapped => Expression.Pow(mapped, powerExpression));
            HashSet<IExpression> powerResults = EvaluateArgument(powerExpression, mapped => Expression.Pow(baseExpression, mapped));

            // Return all
            results.UnionWith(baseResults);
            results.UnionWith(powerResults);
            return results;
        }

        public HashSet<IExpression> EvaluateFunction(IFunction function)
        {
            IFunctionIdentity identity = function.GetIdentity();

            // Get this
            HashSet<IExpression> results = EvaluateExpression(function);

            // Get arguments
            foreach ((var one, var others) in TakeOne(function.GetParameters()))
            {
                IExpression map(IExpression mapped) => identity.CreateExpression(new Dictionary<string, IExpression>(others) { { one.Key, mapped } });
                HashSet<IExpression> argumentResults = EvaluateArgument(one.Value, map);
                results.UnionWith(argumentResults);
            }

            // Return all
            return results;
        }

        protected HashSet<IExpression> EvaluateCommutative(IExpression expression, ICollection<IExpression> expressions, Func<ICollection<IExpression>, IExpression> builder)
        {
            // Get this
            HashSet<IExpression> results = EvaluateExpression(expression);

            // Get arguments
            foreach ((var one, var others) in TakeOne(expressions))
            {
                IExpression map(IExpression mapped) => builder(new List<IExpression>(others) { { mapped } });
                HashSet<IExpression> argumentResults = EvaluateArgument(one, map);
                results.UnionWith(argumentResults);
            }

            // Return all
            return results;
        }

        public HashSet<IExpression> EvaluateProduct(IExpression expression, ICollection<IExpression> expressions)
        {
            return EvaluateCommutative(expression, expressions, Expression.Multiply);
        }

        public HashSet<IExpression> EvaluateSum(IExpression expression, ICollection<IExpression> expressions)
        {
            return EvaluateCommutative(expression, expressions, Expression.Add);
        }

        public virtual HashSet<IExpression> EvaluateOther(IExpression other)
        {
            throw new NotImplementedException($"Cannot replace for unknown expression {other}. Override {typeof(ReplaceEvaluator).Name} to add functionality for your new class.");
        }
    }
}
