using Algebra.Evaluators;
using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace Algebra.PatternMatching
{
    /// <summary>
    /// Finds all matches between an expression (1st argument) and a pattern (2nd argument).
    /// For example, x + y matches (2 * a) + b with both (x, y) = (2 * a, b) and (x, y) = (b, 2 * a).
    /// x + y however does not match 2 * (a + b) as it only checks the root node.
    /// Note from above that commutative operations match twice, for example x + y matches a + b with both (x, y) = (a, b) and (x, y) = (b, a).
    /// </summary>
    public class PatternMatchingDualEvaluator : IDualEvaluator<PatternMatchingResultSet>
    {
        public static readonly PatternMatchingDualEvaluator Instance = new PatternMatchingDualEvaluator();

        private PatternMatchingDualEvaluator()
        {

        }

        public PatternMatchingResultSet EvaluateConstants(IConstant valueToBeMatched, IConstant valuePattern)
        {
            if (valuePattern.Equals(valueToBeMatched))
            {
                return PatternMatchingResultSet.All;
            }
            return PatternMatchingResultSet.None;
        }

        public PatternMatchingResultSet EvaluateArcsins(Expression argumentToBeMatched, Expression argumentPattern)
        {
            return argumentToBeMatched.Evaluate(argumentPattern, this);
        }

        public PatternMatchingResultSet EvaluateArctans(Expression argumentToBeMatched, Expression argumentPattern)
        {
            return argumentToBeMatched.Evaluate(argumentPattern, this);
        }

        public PatternMatchingResultSet EvaluateLns(Expression argumentToBeMatched, Expression argumentPattern)
        {
            return argumentToBeMatched.Evaluate(argumentPattern, this);
        }

        public PatternMatchingResultSet EvaluateSigns(Expression argumentToBeMatched, Expression argumentPattern)
        {
            return argumentToBeMatched.Evaluate(argumentPattern, this);
        }

        public PatternMatchingResultSet EvaluateSins(Expression argumentToBeMatched, Expression argumentPattern)
        {
            return argumentToBeMatched.Evaluate(argumentPattern, this);
        }

        public PatternMatchingResultSet EvaluateExponents(Expression baseArgumentToBeMatched, Expression powerArgumentToBeMatched, Expression baseArgumentPattern, Expression powerArgumentPattern)
        {
            PatternMatchingResultSet baseInputs = baseArgumentToBeMatched.Evaluate(baseArgumentPattern, this);

            // Short circuit if we can before we pattern match power inputs
            if (baseInputs.IsNone)
            {
                return baseInputs;
            }

            PatternMatchingResultSet powerInputs = powerArgumentToBeMatched.Evaluate(powerArgumentPattern, this);

            return baseInputs.Intersect(powerInputs);
        }

        public PatternMatchingResultSet EvaluateFunctions(Function functionToBeMatched, Function functionPattern)
        {
            if (!functionPattern.GetIdentity().Equals(functionToBeMatched.GetIdentity()))
            {
                return PatternMatchingResultSet.None; // If the functions are different, assume no pattern can be matched with them
            }

            var parametersPattern = functionPattern.GetParameters();
            var parametersToBeMatched = functionToBeMatched.GetParameters();

            PatternMatchingResultSet resultSet = PatternMatchingResultSet.All;
            foreach ((string parameterName, Expression parameterPattern) in parametersPattern)
            {
                if (!parametersToBeMatched.TryGetValue(parameterName, out Expression parameterToBeMatched))
                {
                    throw new NotSupportedException("Two functions with the same identity should always have the same parameter names");
                }

                PatternMatchingResultSet parameterInputs = parameterToBeMatched.Evaluate(parameterPattern, this);

                resultSet = resultSet.Intersect(parameterInputs);

                // Short circuit if we can
                if (resultSet.IsNone)
                {
                    return resultSet;
                }
            }

            return resultSet;
        }

        protected PatternMatchingResultSet GetResultsForSets(ICollection<Expression> argumentsToBeMatched, ICollection<Expression> argumentsPattern, Func<ICollection<Expression>, Expression> builder)
        {
            PatternMatchingResultSet results = PatternMatchingResultSet.None;

            foreach (var toBeMatchedPartitioning in LazyFunctions.Partition(new List<Expression>(argumentsToBeMatched)))
            {
                // This can be done way faster but I can't find an algorithm online
                // TODO: Stop being an idiot and figure out an algorithm for myself
                if (toBeMatchedPartitioning.Count != argumentsPattern.Count)
                {
                    continue;
                }

                foreach (var toMatchPartitionPerm in LazyFunctions.Permute(toBeMatchedPartitioning))
                {
                    PatternMatchingResultSet permResults = PatternMatchingResultSet.All;

                    foreach ((List<Expression> toBeMatchedPartition, Expression argumentPattern) in toMatchPartitionPerm.Zip(argumentsPattern, (a, b) => (a, b)))
                    {
                        Expression partitionExpression = builder(toBeMatchedPartition);
                        PatternMatchingResultSet partitionResults = partitionExpression.Evaluate(argumentPattern, this);
                        permResults = permResults.Intersect(partitionResults);
                    }

                    results = results.Union(permResults);
                }
            }

            return results;
        }

        public PatternMatchingResultSet EvaluateProducts(ICollection<Expression> argumentsToBeMatched, ICollection<Expression> argumentsPattern)
        {
            return GetResultsForSets(argumentsToBeMatched, argumentsPattern, Expression.Multiply);
        }

        public PatternMatchingResultSet EvaluateSums(ICollection<Expression> argumentsToBeMatched, ICollection<Expression> argumentsPattern)
        {
            return GetResultsForSets(argumentsToBeMatched, argumentsPattern, Expression.Add);
        }

        public PatternMatchingResultSet EvaluateVariables(IVariable valueToBeMatched, IVariable valuePattern)
        {
            return new PatternMatchingResultSet(new PatternMatchingResult(valuePattern.GetName(), valueToBeMatched.ToExpression()));
        }

        public PatternMatchingResultSet EvaluateOthers(Expression expressionToBeMatched, Expression expressionPattern)
        {
            IsVariableEvaluator.Result result = expressionPattern.Evaluate(IsVariableEvaluator.Instance);
            if (result.IsVariable())
            {
                return new PatternMatchingResultSet(new PatternMatchingResult(result.Get().GetName(), expressionToBeMatched));
            }

            return PatternMatchingResultSet.None;
        }
    }
}
