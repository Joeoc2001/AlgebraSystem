using Algebra.Evaluators;
using Rationals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace Algebra.PatternMatching
{
    class PatternMatchingDualEvaluator : IDualEvaluator<PatternMatchingResultSet>
    {
        public PatternMatchingResultSet EvaluateConstants(Rational valuePattern, Rational valueToBeMatched)
        {
            if (valuePattern.Equals(valueToBeMatched))
            {
                return PatternMatchingResultSet.All;
            }
            return PatternMatchingResultSet.None;
        }

        public PatternMatchingResultSet EvaluateArcsins(IExpression argumentPattern, IExpression argumentToBeMatched)
        {
            return argumentPattern.DualEvaluate(argumentToBeMatched, this);
        }

        public PatternMatchingResultSet EvaluateArctans(IExpression argumentPattern, IExpression argumentToBeMatched)
        {
            return argumentPattern.DualEvaluate(argumentToBeMatched, this);
        }

        public PatternMatchingResultSet EvaluateLns(IExpression argumentPattern, IExpression argumentToBeMatched)
        {
            return argumentPattern.DualEvaluate(argumentToBeMatched, this);
        }

        public PatternMatchingResultSet EvaluateSigns(IExpression argumentPattern, IExpression argumentToBeMatched)
        {
            return argumentPattern.DualEvaluate(argumentToBeMatched, this);
        }

        public PatternMatchingResultSet EvaluateSins(IExpression argumentPattern, IExpression argumentToBeMatched)
        {
            return argumentPattern.DualEvaluate(argumentToBeMatched, this);
        }

        public PatternMatchingResultSet EvaluateExponents(IExpression baseArgumentPattern, IExpression powerArgumentPattern, IExpression baseArgumentToBeMatched, IExpression powerArgumentToBeMatched)
        {
            PatternMatchingResultSet baseInputs = baseArgumentPattern.DualEvaluate(baseArgumentToBeMatched, this);

            // Short circuit if we can before we pattern match power inputs
            if (baseInputs.IsNone)
            {
                return baseInputs;
            }

            PatternMatchingResultSet powerInputs = powerArgumentPattern.DualEvaluate(powerArgumentToBeMatched, this);

            return baseInputs.Intersect(powerInputs);
        }

        public PatternMatchingResultSet EvaluateFunctions(IFunction functionPattern, IFunction functionToBeMatched)
        {
            if (!functionPattern.GetIdentity().Equals(functionToBeMatched.GetIdentity()))
            {
                return PatternMatchingResultSet.None; // If the functions are different, assume no pattern can be matched with them
            }

            var parametersPattern = functionPattern.GetParameters();
            var parametersToBeMatched = functionPattern.GetParameters();

            PatternMatchingResultSet resultSet = PatternMatchingResultSet.All;
            foreach ((string parameterName, IExpression parameterPattern) in parametersPattern)
            {
                if (!parametersToBeMatched.TryGetValue(parameterName, out IExpression parameterToBeMatched))
                {
                    throw new NotSupportedException("Two functions with the same identity should always have the same parameter names");
                }

                PatternMatchingResultSet parameterInputs = parameterPattern.DualEvaluate(parameterToBeMatched, this);

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

        protected PatternMatchingResultSet GetResultsForSets(ICollection<IExpression> argumentsPattern, ICollection<IExpression> argumentsToBeMatched, Func<ICollection<IExpression>, IExpression> builder)
        {
            PatternMatchingResultSet results = PatternMatchingResultSet.None;

            foreach (var item in GetAllPartitions(new List<IExpression>(argumentsToBeMatched), argumentsToBeMatched.Count))
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

                    foreach (List<IExpression> part in permutation)
                    {
                        IExpression partExpression = builder(part);
                        PatternMatchingResultSet partResults = patternEnumerator.Current.DualEvaluate(partExpression, this);
                        permResults.Intersect(partResults);
                    }

                    results.Union(permResults);
                }
            }

            return results;
        }

        public PatternMatchingResultSet EvaluateProducts(ICollection<IExpression> argumentsPattern, ICollection<IExpression> argumentsToBeMatched)
        {
            return GetResultsForSets(argumentsPattern, argumentsToBeMatched, Expression.Multiply);
        }

        public PatternMatchingResultSet EvaluateSums(ICollection<IExpression> argumentsPattern, ICollection<IExpression> argumentsToBeMatched)
        {
            return GetResultsForSets(argumentsPattern, argumentsToBeMatched, Expression.Add);
        }

        public PatternMatchingResultSet EvaluateVariables(string namePattern, string nameToBeMatched)
        {
            return new PatternMatchingResultSet(new PatternMatchingResult(namePattern, Expression.VariableFrom(nameToBeMatched)));
        }

        public PatternMatchingResultSet EvaluateOthers(IExpression expressionPattern, IExpression expressionToBeMatched)
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
