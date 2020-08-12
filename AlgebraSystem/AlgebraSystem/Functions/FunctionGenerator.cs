using Algebra.Functions.HardcodedFunctionIdentities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Functions
{
    public abstract class FunctionGenerator : IFunctionGenerator
    {
        public static readonly Dictionary<string, IFunctionGenerator> DefaultFunctions = new Dictionary<string, IFunctionGenerator>()
        {
            { "sin",  new AtomicFunctionGenerator(1, list => Expression.SinOf(list[0])  ) },
            { "ln",   new AtomicFunctionGenerator(1, list => Expression.LnOf(list[0])   ) },
            { "sign", new AtomicFunctionGenerator(1, list => Expression.SignOf(list[0]) ) },
            { "max", MaxIdentity.Instance },
            { "min", MinIdentity.Instance },
            { "select", SelectIdentity.Instance }
        };

        protected abstract Expression CreateExpressionImpl(Dictionary<string, Expression> parameters);
        public abstract ReadOnlyCollection<string> GetRequiredParameters();

        /// <summary>
        /// Return a new function node from the given parameters.
        /// This constructor matches the parameter positions with the parameter names internally
        /// </summary>
        /// <param name="nodes">The parameters to be given to the function</param>
        /// <returns>The new function node</returns>
        public Expression CreateExpression(List<Expression> nodes)
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

            return CreateExpression(parameters);
        }

        /// <summary>
        /// Return a new function node from the given parameters.
        /// This constructor uses the given parameter names and ignores their positions
        /// </summary>
        /// <param name="nodes">The parameters to be given to the function</param>
        /// <returns>The new function node</returns>
        public Expression CreateExpression(Dictionary<string, Expression> parameters)
        {
            if (!AreParametersSatisfied(parameters))
            {
                throw new ArgumentException("Parameters do not satisfy required parameters");
            }
            return CreateExpressionImpl(parameters);
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
    }
}
