﻿using Algebra.Atoms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.ComponentModel;

namespace Algebra
{
    /// <summary>
    /// A dictionary of variable input nodes as well as their names. 
    /// Used at function compilation time to assign all of the variables
    /// in an expression to a cell where the value can be inputted
    /// </summary>
    public class VariableInputSet<T> : IEquatable<VariableInputSet<T>>, IEnumerable<VariableInput<T>>
    {
        private readonly Dictionary<string, VariableInput<T>> _values = new Dictionary<string, VariableInput<T>>();

        public VariableInputSet()
        {

        }

        public VariableInput<T> this[string v]
        {
            get => _values[v];
        }

        public bool Contains(string name)
        {
            return _values.ContainsKey(name);
        }

        /// <summary>
        /// Adds a variable input with the given name and value default to the variable input set.
        /// This method throws if the given string is already present
        /// </summary>
        /// <param name="name">The name of the variable to add</param>
        /// <exception cref="ArgumentException">Thrown if a variable input with the given name is already present</exception>
        public void Add(string name)
        {
            Add(name, default);
        }

        /// <summary>
        /// Checks if this variable input set has no variables in it
        /// </summary>
        /// <returns>True iff this is empty</returns>
        public bool IsEmpty()
        {
            return _values.Count == 0;
        }

        /// <summary>
        /// Adds a variable input with the given name and value to this variable input set.
        /// This method throws if the given string is already present
        /// </summary>
        /// <param name="name">The name of the variable to add</param>
        /// <param name="value">The value to set the variable to initially</param>
        /// <exception cref="ArgumentException">Thrown if a variable input with the given name is already present</exception>
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

        /// <summary>
        /// Sets a variable input with the given name and value in this variable input set, or adds it if a variable is not already present with the given name
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <param name="value">The value to set the variable to</param>
        public void Set(string name, T value)
        {
            if (!_values.TryGetValue(name, out VariableInput<T> variableInput))
            {
                variableInput = new VariableInput<T>();
                _values.Add(name, variableInput);
            }
            variableInput.Value = value;
        }

        /// <summary>
        /// Gets a variable input with the given name in this variable input set.
        /// This method throws if the given string is not present
        /// </summary>
        /// <param name="name">The name of the variable to get</param>
        /// <exception cref="ArgumentException">Thrown if a variable input with the given name is not present</exception>
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