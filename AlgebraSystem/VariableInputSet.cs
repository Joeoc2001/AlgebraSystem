using Algebra.Atoms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;


namespace Algebra
{
    /// <summary>
    /// A dictionary of variable input nodes as well as their names. 
    /// Used at function compilation time to assign all of the variables
    /// in an expression to a cell where the value can be inputted
    /// </summary>
    public class VariableInputSet : IEquatable<VariableInputSet>, IEnumerable<VariableInput>
    {
        private readonly Dictionary<string, VariableInput> values = new Dictionary<string, VariableInput>();

        public static implicit operator VariableInputSet(float x) => new VariableInputSet(x);
        public static implicit operator VariableInputSet(Vector2 v2) => new VariableInputSet(v2);
        public static implicit operator VariableInputSet(Vector3 v3) => new VariableInputSet(v3);
        public static implicit operator VariableInputSet(Vector4 v4) => new VariableInputSet(v4);

        public VariableInputSet()
        {

        }

        public VariableInputSet(float x)
            : this()
        {
            Set("X", x);
        }

        public VariableInputSet(Vector2 v2)
            : this()
        {
            Set(v2);
        }

        public VariableInputSet(Vector3 v3)
            : this()
        {
            Set(v3);
        }

        public VariableInputSet(Vector4 v4)
            : this()
        {
            Set(v4);
        }

        public VariableInput this[string v]
        {
            get => values[v.ToLower()];
        }

        public bool Contains(string name)
        {
            return values.ContainsKey(name.ToLower());
        }

        /// <summary>
        /// Adds a variable input with the given name and value 0 to the variable input set.
        /// This method throws if the given string is already present
        /// </summary>
        /// <param name="name">The name of the variable to add</param>
        /// <exception cref="ArgumentException">Thrown if a variable input with the given name is already present</exception>
        public void Add(string name)
        {
            Add(name, 0);
        }

        /// <summary>
        /// Adds a variable input with the given name and value to the variable input set.
        /// This method throws if the given string is already present
        /// </summary>
        /// <param name="name">The name of the variable to add</param>
        /// <param name="value">The value to set the variable to initially</param>
        /// <exception cref="ArgumentException">Thrown if a variable input with the given name is already present</exception>
        public void Add(string name, float value)
        {
            string lowerName = name.ToLower();
            if (values.ContainsKey(lowerName))
            {
                throw new ArgumentException("A variable with the same name has already been added");
            }

            VariableInput variableInput = new VariableInput();
            values.Add(lowerName, variableInput);
            variableInput.Value = value;
        }

        public void Set(string name, float value)
        {
            string lowerName = name.ToLower();
            if (!values.TryGetValue(lowerName, out VariableInput variableInput))
            {
                variableInput = new VariableInput();
                values.Add(lowerName, variableInput);
            }
            variableInput.Value = value;
        }

        public void Set(Vector2 v2)
        {
            Set(Variable.X.Name, v2.X);
            Set(Variable.Y.Name, v2.Y);
        }

        public void Set(Vector3 v3)
        {
            Set(new Vector2(v3.X, v3.Y));
            Set(Variable.Z.Name, v3.Z);
        }

        public void Set(Vector4 v4)
        {
            Set(new Vector3(v4.X, v4.Y, v4.Z));
            Set(Variable.W.Name, v4.W);
        }

        public bool Equals(VariableInputSet o)
        {
            if (o.values.Count != values.Count)
            {
                return false;
            }

            foreach (string name in values.Keys)
            {
                if (!o.values.TryGetValue(name, out VariableInput otherVariableInput))
                {
                    return false;
                }

                VariableInput thisVariableInput = values[name];
                if (!ReferenceEquals(thisVariableInput, otherVariableInput))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is VariableInputSet))
            {
                return false;
            }

            return this.Equals((VariableInputSet)obj);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException("Variable sets cannot be hashed"); // No hash function can exist for this due to floating point accuracy
        }

        public IEnumerator<VariableInput> GetEnumerator()
        {
            return values.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.Values.GetEnumerator();
        }

        public static bool operator ==(VariableInputSet left, VariableInputSet right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            return left.Equals(right);
        }

        public static bool operator !=(VariableInputSet left, VariableInputSet right)
        {
            return !(left == right);
        }
    }
}