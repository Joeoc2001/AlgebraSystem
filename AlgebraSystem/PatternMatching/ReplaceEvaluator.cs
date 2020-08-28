using Algebra.Evaluators;
using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.PatternMatching
{
    public class ReplaceEvaluator : IExpandedEvaluator<IEnumerable<IExpression>>
    {
        private readonly IExpression _patternExpression;
        private readonly IExpression _replacementExpression;

        public ReplaceEvaluator(IExpression patternExpression, IExpression replacementExpression)
        {
            this._patternExpression = patternExpression ?? throw new ArgumentNullException();
            this._replacementExpression = replacementExpression ?? throw new ArgumentNullException();

            if (patternExpression.GetVariables().Except(replacementExpression.GetVariables()).Any())
            {
                throw new ArgumentException($"Expression {nameof(replacementExpression)} does not have all variables used within {nameof(replacementExpression)}");
            }
        }

        protected IEnumerable<IExpression> EvaluateExpression(IExpression expression)
        {
            PatternMatchingResultSet matches = expression.Evaluate(_patternExpression, PatternMatchingDualEvaluator.Instance);

            foreach (PatternMatchingResult result in matches)
            {
                VariableReplacementEvaluator replacementEvaluator = new VariableReplacementEvaluator(result, false);
                yield return _replacementExpression.Evaluate(replacementEvaluator);
            }
        }

        protected IEnumerable<IExpression> EvaluateArgument(IExpression argument, Func<IExpression, IExpression> argumentMap)
        {
            IEnumerable<IExpression> unmappedArgumentResults = argument.Evaluate(this);

            // Map
            foreach (var unmappedArgument in unmappedArgumentResults)
            {
                yield return argumentMap(unmappedArgument);
            }
        }

        protected IEnumerable<IExpression> EvaluateMonad(IExpression expression, IExpression argumentExpression, Func<IExpression, IExpression> argumentMap)
        {
            // Get this
            foreach (IExpression result in EvaluateExpression(expression))
            {
                yield return result;
            }

            // Get arguments
            foreach (IExpression result in EvaluateArgument(argumentExpression, argumentMap))
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

        public IEnumerable<IExpression> EvaluateConstant(IExpression expression, Rational value)
        {
            return EvaluateExpression(expression);
        }

        public IEnumerable<IExpression> EvaluateVariable(IExpression expression, string name)
        {
            return EvaluateExpression(expression);
        }

        public IEnumerable<IExpression> EvaluateArcsin(IExpression expression, IExpression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.ArcsinOf);
        }

        public IEnumerable<IExpression> EvaluateArctan(IExpression expression, IExpression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.ArctanOf);
        }

        public IEnumerable<IExpression> EvaluateLn(IExpression expression, IExpression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.LnOf);
        }

        public IEnumerable<IExpression> EvaluateSign(IExpression expression, IExpression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.SignOf);
        }

        public IEnumerable<IExpression> EvaluateSin(IExpression expression, IExpression argumentExpression)
        {
            return EvaluateMonad(expression, argumentExpression, Expression.SinOf);
        }

        public IEnumerable<IExpression> EvaluateExponent(IExpression expression, IExpression baseExpression, IExpression powerExpression)
        {
            // Get this
            IEnumerable<IExpression> results = EvaluateExpression(expression);

            // Get arguments
            IEnumerable<IExpression> baseResults = EvaluateArgument(baseExpression, mapped => Expression.Pow(mapped, powerExpression));
            IEnumerable<IExpression> powerResults = EvaluateArgument(powerExpression, mapped => Expression.Pow(baseExpression, mapped));

            // Return all
            return results.Concat(baseResults).Concat(powerResults);
        }

        public IEnumerable<IExpression> EvaluateFunction(IFunction function)
        {
            IFunctionIdentity identity = function.GetIdentity();

            // Get this
            IEnumerable<IExpression> results = EvaluateExpression(function);

            // Get arguments
            foreach ((var one, var others) in TakeOne(function.GetParameters()))
            {
                IExpression map(IExpression mapped) => identity.CreateExpression(new Dictionary<string, IExpression>(others) { { one.Key, mapped } });
                IEnumerable<IExpression> argumentResults = EvaluateArgument(one.Value, map);
                results = results.Concat(argumentResults);
            }

            // Return all
            return results;
        }

        protected IEnumerable<IExpression> EvaluateCommutative(IExpression expression, ICollection<IExpression> expressions, Func<ICollection<IExpression>, IExpression> builder)
        {
            // Get this
            IEnumerable<IExpression> results = EvaluateExpression(expression);

            // Get arguments
            foreach ((var one, var others) in TakeOne(expressions))
            {
                IExpression map(IExpression mapped) => builder(new List<IExpression>(others) { { mapped } });
                IEnumerable<IExpression> argumentResults = EvaluateArgument(one, map);
                results = results.Concat(argumentResults);
            }

            // Return all
            return results;
        }

        public IEnumerable<IExpression> EvaluateProduct(IExpression expression, ICollection<IExpression> expressions)
        {
            return EvaluateCommutative(expression, expressions, Expression.Multiply);
        }

        public IEnumerable<IExpression> EvaluateSum(IExpression expression, ICollection<IExpression> expressions)
        {
            return EvaluateCommutative(expression, expressions, Expression.Add);
        }

        public virtual IEnumerable<IExpression> EvaluateOther(IExpression other)
        {
            throw new NotImplementedException($"Cannot replace for unknown expression {other}. Override {typeof(ReplaceEvaluator).Name} to add functionality for your new class.");
        }
    }
}
