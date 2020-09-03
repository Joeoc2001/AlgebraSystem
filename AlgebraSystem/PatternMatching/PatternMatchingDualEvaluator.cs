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
    /// </summary>
    public class PatternMatchingDualEvaluator : IDualEvaluator<PatternMatchingResultSet>
    {
        public static readonly PatternMatchingDualEvaluator Instance = new PatternMatchingDualEvaluator();

        private PatternMatchingDualEvaluator()
        {

        }

        public PatternMatchingResultSet EvaluateConstants(Rational valueToBeMatched, Rational valuePattern)
        {
            if (valuePattern.Equals(valueToBeMatched))
            {
                return PatternMatchingResultSet.All;
            }
            return PatternMatchingResultSet.None;
        }

        public PatternMatchingResultSet EvaluateArcsins(Expression argumentToBeMatched, Expression argumentPattern)
        {
            return argumentPattern.Evaluate(argumentToBeMatched, this);
        }

        public PatternMatchingResultSet EvaluateArctans(Expression argumentToBeMatched, Expression argumentPattern)
        {
            return argumentPattern.Evaluate(argumentToBeMatched, this);
        }

        public PatternMatchingResultSet EvaluateLns(Expression argumentToBeMatched, Expression argumentPattern)
        {
            return argumentPattern.Evaluate(argumentToBeMatched, this);
        }

        public PatternMatchingResultSet EvaluateSigns(Expression argumentToBeMatched, Expression argumentPattern)
        {
            return argumentPattern.Evaluate(argumentToBeMatched, this);
        }

        public PatternMatchingResultSet EvaluateSins(Expression argumentToBeMatched, Expression argumentPattern)
        {
            return argumentPattern.Evaluate(argumentToBeMatched, this);
        }

        public PatternMatchingResultSet EvaluateExponents(Expression baseArgumentToBeMatched, Expression powerArgumentToBeMatched, Expression baseArgumentPattern, Expression powerArgumentPattern)
        {
            PatternMatchingResultSet baseInputs = baseArgumentPattern.Evaluate(baseArgumentToBeMatched, this);

            // Short circuit if we can before we pattern match power inputs
            if (baseInputs.IsNone)
            {
                return baseInputs;
            }

            PatternMatchingResultSet powerInputs = powerArgumentPattern.Evaluate(powerArgumentToBeMatched, this);

            return baseInputs.Intersect(powerInputs);
        }

        public PatternMatchingResultSet EvaluateFunctions(Function functionToBeMatched, Function functionPattern)
        {
            if (!functionPattern.GetIdentity().Equals(functionToBeMatched.GetIdentity()))
            {
                return PatternMatchingResultSet.None; // If the functions are different, assume no pattern can be matched with them
            }

            var parametersPattern = functionPattern.GetParameters();
            var parametersToBeMatched = functionPattern.GetParameters();

            PatternMatchingResultSet resultSet = PatternMatchingResultSet.All;
            foreach ((string parameterName, Expression parameterPattern) in parametersPattern)
            {
                if (!parametersToBeMatched.TryGetValue(parameterName, out Expression parameterToBeMatched))
                {
                    throw new NotSupportedException("Two functions with the same identity should always have the same parameter names");
                }

                PatternMatchingResultSet parameterInputs = parameterPattern.Evaluate(parameterToBeMatched, this);

                resultSet = resultSet.Intersect(parameterInputs);

                // Short circuit if we can
                if (resultSet.IsNone)
                {
                    return resultSet;
                }
            }

            return resultSet;
        }

        protected static IEnumerable<List<List<T>>> GetAllPartitions<T>(IList<T> elements, int maxlen)
        {
            if (maxlen <= 0)
            {
                yield return new List<List<T>>();
            }
            else
            {
                T elem = elements[maxlen - 1];
                var shorter = GetAllPartitions(elements, maxlen - 1);
                foreach (var part in shorter)
                {
                    foreach (var list in part.ToArray())
                    {
                        list.Add(elem);
                        yield return part;
                        list.RemoveAt(list.Count - 1);
                    }
                    var newlist = new List<T>
                    {
                        elem
                    };
                    part.Add(newlist);
                    yield return part;
                    part.RemoveAt(part.Count - 1);
                }
            }
        }

        protected static IEnumerable<List<T>> GetPermutations<T>(IList<T> elements)
        {
            if (elements.Count == 0)
            {
                yield return new List<T>();
            }

            for (int i = 0; i < elements.Count; i++)
            {
                T other = elements[0];
                elements.RemoveAt(0);

                foreach (List<T> otherPerm in GetPermutations(elements))
                {
                    otherPerm.Add(other);
                    yield return otherPerm;
                    otherPerm.RemoveAt(otherPerm.Count - 1);
                }

                elements.Add(other);
            }
        }

        protected PatternMatchingResultSet GetResultsForSets(ICollection<Expression> argumentsToBeMatched, ICollection<Expression> argumentsPattern, Func<ICollection<Expression>, Expression> builder)
        {
            PatternMatchingResultSet results = PatternMatchingResultSet.None;

            foreach (var item in GetAllPartitions(new List<Expression>(argumentsToBeMatched), argumentsToBeMatched.Count))
            {
                // This can be done way faster but I can't find an algorithm online
                // TODO: Stop being an idiot and figure out an algorithm for myself
                if (item.Count != argumentsPattern.Count)
                {
                    continue;
                }

                var patternEnumerator = argumentsPattern.GetEnumerator();
                foreach (var permutation in GetPermutations(item))
                {
                    patternEnumerator.MoveNext();

                    PatternMatchingResultSet permResults = PatternMatchingResultSet.All;

                    foreach (List<Expression> part in permutation)
                    {
                        Expression partExpression = builder(part);
                        PatternMatchingResultSet partResults = patternEnumerator.Current.Evaluate(partExpression, this);
                        permResults.Intersect(partResults);
                    }

                    results.Union(permResults);
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

        public PatternMatchingResultSet EvaluateVariables(string nameToBeMatched, string namePattern)
        {
            return new PatternMatchingResultSet(new PatternMatchingResult(namePattern, Expression.VariableFrom(nameToBeMatched)));
        }

        public PatternMatchingResultSet EvaluateOthers(Expression expressionToBeMatched, Expression expressionPattern)
        {
            IsVariableEvaluator.Result result = expressionPattern.Evaluate(IsVariableEvaluator.Instance);
            if (result.IsVariable())
            {
                return new PatternMatchingResultSet(new PatternMatchingResult(result.GetName(), expressionToBeMatched));
            }

            return PatternMatchingResultSet.None;
        }
    }
}
