using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Algebra.PatternMatching
{
    public class PatternMatchingResult : IReadOnlyDictionary<string, Expression>, IEquatable<PatternMatchingResult>
    {
        public static PatternMatchingResult Empty = new PatternMatchingResult(new Dictionary<string, Expression>());

        private readonly ReadOnlyDictionary<string, Expression> _matches;

        public PatternMatchingResult(string name, Expression match)
            : this(new Dictionary<string, Expression>() { { name, match } })
        {

        }

        private static Dictionary<string, Expression> Build((string name, Expression expr)[] matches)
        {
            Dictionary<string, Expression> dict = new Dictionary<string, Expression>();

            foreach (var item in matches)
            {
                dict.Add(item.name, item.expr);
            }

            return dict;
        }

        public PatternMatchingResult(params (string, Expression)[] matches)
            : this(Build(matches))
        {

        }

        public PatternMatchingResult(IDictionary<string, Expression> matches)
        {
            this._matches = new ReadOnlyDictionary<string, Expression>(matches);
        }

        public PatternMatchingResult CalculateJoin(PatternMatchingResult other)
        {
            Dictionary<string, Expression> join = new Dictionary<string, Expression>();

            foreach ((string matchVar, Expression matchValue1) in this)
            {
                // If there is a match for the given variable in both results
                if (other.TryGetValue(matchVar, out Expression matchValue2))
                {
                    if (!matchValue1.Equals(matchValue2))
                    {
                        return null; // If one of the matches doesn't line up, no join exists
                    }
                }

                // Add if there is only a match for the given variable in result1 or if the two matches are equal
                join.Add(matchVar, matchValue1);
            }

            foreach ((string matchVar, Expression matchValue2) in other)
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

        public Expression this[string key]
        {
            get
            {
                return _matches[key];
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return ((IReadOnlyDictionary<string, Expression>)_matches).Keys;
            }
        }

        public IEnumerable<Expression> Values
        {
            get
            {
                return ((IReadOnlyDictionary<string, Expression>)_matches).Values;
            }
        }

        public int Count
        {
            get
            {
                return _matches.Count;
            }
        }

        public bool ContainsKey(string key)
        {
            return _matches.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, Expression>> GetEnumerator()
        {
            return _matches.GetEnumerator();
        }

        public bool TryGetValue(string key, out Expression value)
        {
            return _matches.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _matches.GetEnumerator();
        }

        public bool Equals(PatternMatchingResult other)
        {
            if (other is null)
            {
                return false;
            }
            return _matches.Count == other._matches.Count
                && !_matches.Except(other._matches).Any();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PatternMatchingResult);
        }

        public override int GetHashCode()
        {
            // Use ^ because it is commutative, even though it results in a worse hash
            int v = 1875743615;
            foreach (var result in _matches)
            {
                v ^= result.Key.GetHashCode();
                v ^= (result.Value.GetHashCode() * 65);
            }
            return v;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(nameof(PatternMatchingResult));
            builder.Append("{");
            builder.Append(string.Join(", ", _matches.Select((pair, i) => $"{pair.Key}: {pair.Value}")));
            builder.Append("}");
            return builder.ToString();
        }
    }
}
