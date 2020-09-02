using Algebra.Functions.HardcodedFunctionIdentities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Functions
{
    public abstract class FunctionGenerator
    {
        private readonly string _name;
        private readonly ReadOnlyCollection<string> _parameterNames;

        protected FunctionGenerator(string name, IEnumerable<string> parameterNames)
            : this(name, new List<string>(parameterNames).AsReadOnly())
        {

        }

        protected FunctionGenerator(string name, ReadOnlyCollection<string> parameterNames)
        {
            this._name = name ?? throw new ArgumentNullException(nameof(name));
            this._parameterNames = parameterNames ?? throw new ArgumentNullException(nameof(parameterNames));
        }

        protected abstract Expression CreateExpressionImpl(IDictionary<string, Expression> parameters);

        public ReadOnlyCollection<string> GetRequiredParameters()
        {
            return _parameterNames;
        }

        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Return a new function node from the given parameters.
        /// This constructor matches the parameter positions with the parameter names internally
        /// </summary>
        /// <param name="nodes">The parameters to be given to the function</param>
        /// <returns>The new function node</returns>
        public Expression CreateExpression(params Expression[] nodes)
        {
            return CreateExpression(new List<Expression>(nodes));
        }

        /// <summary>
        /// Return a new function node from the given parameters.
        /// This constructor matches the parameter positions with the parameter names internally
        /// </summary>
        /// <param name="nodes">The parameters to be given to the function</param>
        /// <returns>The new function node</returns>
        public Expression CreateExpression(IList<Expression> nodes)
        {
            IList<string> requiredParameters = GetRequiredParameters();

            if (nodes.Count != requiredParameters.Count)
            {
                throw new ArgumentException("Incorrect number of parameters were provided");
            }

            Dictionary<string, Expression> parameters = new Dictionary<string, Expression>();
            for (int i = 0; i < requiredParameters.Count; i++)
            {
                parameters.Add(requiredParameters[i], nodes[i]);
            }

            return CreateExpression(parameters);
        }

        /// <summary>
        /// Return a new function node from the given parameters.
        /// This constructor uses the given parameter names and ignores their positions
        /// </summary>
        /// <param name="nodes">The parameters to be given to the function</param>
        /// <returns>The new function node</returns>
        public Expression CreateExpression(IDictionary<string, Expression> parameters)
        {
            if (!AreParametersSatisfied(parameters))
            {
                throw new ArgumentException("Parameters do not satisfy required parameters");
            }
            return CreateExpressionImpl(parameters);
        }

        public bool AreParametersSatisfied(IDictionary<string, Expression> parameters)
        {
            // Ensure that all required parameters are filled
            IList<string> requiredParameters = GetRequiredParameters();
            if (parameters.Count != requiredParameters.Count)
            {
                return false;
            }

            foreach (string parameter in requiredParameters)
            {
                if (!parameters.ContainsKey(parameter))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
