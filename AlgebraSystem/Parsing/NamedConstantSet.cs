using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Algebra.Parsing
{
    public class NamedConstantSet : IReadOnlyDictionary<string, IConstant>, IEquatable<NamedConstantSet>
    {
        public static readonly NamedConstantSet DefaultConstants = new NamedConstantSet(new Dictionary<string, IConstant>()
        {
            { "PI", Constants.PI },
            { "E", Constants.E },
        });

        private readonly Dictionary<string, IConstant> _values;

        public NamedConstantSet(IEnumerable<KeyValuePair<string, IConstant>> values)
        {
            _values = new Dictionary<string, IConstant>();
            foreach ((string name, IConstant constant) in values)
            {
                _values.Add(name, constant);
            }
        }

        public bool Equals(NamedConstantSet other)
        {
            if (other is null)
            {
                return false;
            }
            return _values.Count == other.Count && !_values.Except(other).Any();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NamedConstantSet);
        }

        public override int GetHashCode()
        {
            int seed = -126233709;
            foreach ((string name, IConstant constant) in _values)
            {
                seed += name.GetHashCode() * 33 + constant.GetHashCode();
                seed *= 65;
            }
            return seed;
        }

        public IConstant this[string key]
        {
            get
            {
                return ((IReadOnlyDictionary<string, IConstant>)_values)[key];
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return ((IReadOnlyDictionary<string, IConstant>)_values).Keys;
            }
        }

        public IEnumerable<IConstant> Values
        {
            get
            {
                return ((IReadOnlyDictionary<string, IConstant>)_values).Values;
            }
        }

        public int Count
        {
            get
            {
                return ((IReadOnlyCollection<KeyValuePair<string, IConstant>>)_values).Count;
            }
        }

        public ICollection<string> Names { get => _values.Keys; }

        public bool ContainsKey(string key)
        {
            return ((IReadOnlyDictionary<string, IConstant>)_values).ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, IConstant>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, IConstant>>)_values).GetEnumerator();
        }

        public bool TryGetValue(string key, out IConstant value)
        {
            return ((IReadOnlyDictionary<string, IConstant>)_values).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_values).GetEnumerator();
        }
    }
}
