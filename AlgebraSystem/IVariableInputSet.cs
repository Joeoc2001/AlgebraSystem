using Algebra.Atoms;
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
    public interface IVariableInputSet<T> : IEquatable<VariableInputSet<T>>, IEnumerable<VariableInput<T>>
    {
        VariableInput<T> this[string v]
        {
            get;
        }

        bool Contains(string name);

        /// <summary>
        /// Adds a variable input with the given name and value default to the variable input set.
        /// This method throws if the given string is already present
        /// </summary>
        /// <param name="name">The name of the variable to add</param>
        /// <exception cref="ArgumentException">Thrown if a variable input with the given name is already present</exception>
        void Add(string name);

        /// <summary>
        /// Checks if this variable input set has no variables in it
        /// </summary>
        /// <returns>True iff this is empty</returns>
        bool IsEmpty();

        /// <summary>
        /// Adds a variable input with the given name and value to this variable input set.
        /// This method throws if the given string is already present
        /// </summary>
        /// <param name="name">The name of the variable to add</param>
        /// <param name="value">The value to set the variable to initially</param>
        /// <exception cref="ArgumentException">Thrown if a variable input with the given name is already present</exception>
        void Add(string name, T value);

        /// <summary>
        /// Sets a variable input with the given name and value in this variable input set, or adds it if a variable is not already present with the given name
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <param name="value">The value to set the variable to</param>
        void Set(string name, T value);

        /// <summary>
        /// Gets a variable input with the given name in this variable input set.
        /// This method throws if the given string is not present
        /// </summary>
        /// <param name="name">The name of the variable to get</param>
        /// <exception cref="ArgumentException">Thrown if a variable input with the given name is not present</exception>
        VariableInput<T> Get(string name);
    }
}