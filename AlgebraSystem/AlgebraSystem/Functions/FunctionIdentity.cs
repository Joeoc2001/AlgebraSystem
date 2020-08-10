using Algebra.Operations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Functions
{
    public abstract class FunctionIdentity
    {
        public static readonly Dictionary<string, FunctionIdentity> DefaultFunctions = new Dictionary<string, FunctionIdentity>()
        {

        };

        private readonly List<string> parameterNames;
        private readonly int hashSeed;

        protected FunctionIdentity(List<string> parameterNames, int hashSeed)
        {
            this.parameterNames = parameterNames ?? throw new ArgumentNullException(nameof(parameterNames));
            this.hashSeed = hashSeed;
        }

        /// <summary>
        /// Return a new function node from the given parameters.
        /// This constructor matches the parameter positions with the parameter names internally
        /// </summary>
        /// <param name="nodes">The parameters to be given to the function</param>
        /// <returns>The new function node</returns>
        public Expression CreateEquation(List<Expression> nodes)
        {
            ReadOnlyCollection<string> requiredParameters = GetRequiredParameters();

            if (nodes.Count != requiredParameters.Count)
            {
                throw new ArgumentException("Incorrect number of parameters were provided");
            }

            Dictionary<string, Expression> parameters = new Dictionary<string, Expression>();
            for (int i = 0; i < requiredParameters.Count; i++)
            {
                parameters.Add(requiredParameters[i], nodes[i]);
            }

            return CreateEquation(parameters);
        }

        /// <summary>
        /// Return a new function node from the given parameters.
        /// This constructor uses the given parameter names and ignores their positions
        /// </summary>
        /// <param name="nodes">The parameters to be given to the function</param>
        /// <returns>The new function node</returns>
        public Expression CreateEquation(Dictionary<string, Expression> nodes)
        {
            return new Function(this, nodes);
        }

        public ReadOnlyCollection<string> GetRequiredParameters()
        {
            return parameterNames.AsReadOnly();
        }

        public bool AreParametersSatisfied(Dictionary<string, Expression> parameters)
        {
            // Ensure that all required parameters are filled
            ReadOnlyCollection<string> requiredParameters = GetRequiredParameters();
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

        public int GetHashSeed()
        {
            return hashSeed;
        }

        public abstract Expression.ExpressionDelegate GetExpression(Dictionary<string, Expression.ExpressionDelegate> parameterExpressions);

        public abstract Expression GetDerivative(Variable wrt);
    }
}
