using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Algebra.PatternMatching
{
    public class PatternMatchingResult : IReadOnlyDictionary<string, IExpression>, IEquatable<PatternMatchingResult>
    {
        public static PatternMatchingResult Empty = new PatternMatchingResult(new Dictionary<string, IExpression>());

        private readonly ReadOnlyDictionary<string, IExpression> matches;

        public PatternMatchingResult(string name, IExpression match)
            : this(new Dictionary<string, IExpression>() { { name, match } })
        {

        }

        public PatternMatchingResult(IDictionary<string, IExpression> matches)
        {
            this.matches = new ReadOnlyDictionary<string, IExpression>(matches);
        }

        public PatternMatchingResult CalculateJoin(PatternMatchingResult other)
        {
            Dictionary<string, IExpression> join = new Dictionary<string, IExpression>();

            foreach ((string matchVar, IExpression matchValue1) in this)
            {
                // If there is a match for the given variable in both results
                if (other.TryGetValue(matchVar, out IExpression matchValue2))
                {
                    if (!matchValue1.Equals(matchValue2))
                    {
                        return null; // If one of the matches doesn't line up, no join exists
                    }
                }

                // Add if there is only a match for the given variable in result1 or if the two matches are equal
                join.Add(matchVar, matchValue1);
            }

            foreach ((string matchVar, IExpression matchValue2) in other)
            {
                // If there is a match for the given variable in both results then it was already dealt with in the above loop
                if (other.ContainsKey(matchVar))
                {
                    continue;
                }

                // Add if there is only a match for the given variable in result2
                join.Add(matchVar, matchValue2);
            }

            return new PatternMatchingResult(join);
        }

        public IExpression this[string key]
        {
            get
            {
                return matches[key];
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return ((IReadOnlyDictionary<string, IExpression>)matches).Keys;
            }
        }

        public IEnumerable<IExpression> Values
        {
            get
            {
                return ((IReadOnlyDictionary<string, IExpression>)matches).Values;
            }
        }

        public int Count
        {
            get
            {
                return matches.Count;
            }
        }

        public bool ContainsKey(string key)
        {
            return matches.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, IExpression>> GetEnumerator()
        {
            return matches.GetEnumerator();
        }

        public bool TryGetValue(string key, out IExpression value)
        {
            return matches.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return matches.GetEnumerator();
        }

        public bool Equals(PatternMatchingResult other)
        {
            if (other is null)
            {
                return false;
            }
            return matches.Count == other.matches.Count && !matches.Except(other.matches).Any();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PatternMatchingResult);
        }

        public override int GetHashCode()
        {
            // Use ^ because it is commutative, even though it results in a worse hash
            int v = 1875743615;
            foreach (var result in matches)
            {
                v ^= result.Key.GetHashCode();
                v ^= (result.Value.GetHashCode() * 65);
            }
            return v;
        }
    }
}
