using Algebra.Atoms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.ComponentModel;

namespace Algebra
{
    /// <see cref="IVariableInputSet{T}"/>
    public class VariableInputSet<T> : IVariableInputSet<T>
    {
        private readonly Dictionary<string, VariableInput<T>> _values = new Dictionary<string, VariableInput<T>>();

        public VariableInputSet()
        {

        }

        public VariableInput<T> this[string v]
        {
            get => _values[v];
        }

        /// <see cref="IVariableInputSet{T}.Contains(string)"/>
        public bool Contains(string name)
        {
            return _values.ContainsKey(name);
        }

        /// <see cref="IVariableInputSet{T}.Add(string)"/>
        public void Add(string name)
        {
            Add(name, default);
        }

        /// <see cref="IVariableInputSet{T}.IsEmpty"/>
        public bool IsEmpty()
        {
            return _values.Count == 0;
        }

        /// <see cref="IVariableInputSet{T}.Add(string, T)"/>
        public void Add(string name, T value)
        {
            if (_values.ContainsKey(name))
            {
                throw new ArgumentException("A variable with the same name has already been added");
            }

            VariableInput<T> variableInput = new VariableInput<T>();
            _values.Add(name, variableInput);
            variableInput.Value = value;
        }

        /// <see cref="IVariableInputSet{T}.Set(string, T)"/>
        public void Set(string name, T value)
        {
            if (!_values.TryGetValue(name, out VariableInput<T> variableInput))
            {
                variableInput = new VariableInput<T>();
                _values.Add(name, variableInput);
            }
            variableInput.Value = value;
        }

        /// <see cref="IVariableInputSet{T}.Get(string)"/>
        public VariableInput<T> Get(string name)
        {
            if (!_values.TryGetValue(name, out VariableInput<T> variableInput))
            {
                throw new ArgumentException($"No variable exists in this set with name {name}");
            }
            return variableInput;
        }

        public bool Equals(VariableInputSet<T> o)
        {
            if (o._values.Count != _values.Count)
            {
                return false;
            }

            foreach (string name in _values.Keys)
            {
                if (!o._values.TryGetValue(name, out VariableInput<T> otherVariableInput))
                {
                    return false;
                }

                VariableInput<T> thisVariableInput = _values[name];
                if (ReferenceEquals(thisVariableInput, otherVariableInput))
                {
                    continue;
                }

                if (!thisVariableInput.Value.Equals(otherVariableInput.Value))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is VariableInputSet<T>))
            {
                return false;
            }

            return this.Equals((VariableInputSet<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                List<string> valueNames = _values.Keys.ToList();
                valueNames.Sort(StringComparer.CurrentCulture);

                foreach (string name in valueNames)
                {
                    hash *= 33;
                    hash ^= name.GetHashCode();
                    hash *= 17;
                    hash ^= _values[name].GetHashCode();
                }
                return hash;
            }
        }

        public IEnumerator<VariableInput<T>> GetEnumerator()
        {
            return _values.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.Values.GetEnumerator();
        }

        public static bool operator ==(VariableInputSet<T> left, VariableInputSet<T> right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            return left.Equals(right);
        }

        public static bool operator !=(VariableInputSet<T> left, VariableInputSet<T> right)
        {
            return !(left == right);
        }
    }
}