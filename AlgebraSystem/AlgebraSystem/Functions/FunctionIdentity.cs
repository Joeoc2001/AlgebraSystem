using Algebra.Atoms;
using Algebra.Functions.HardcodedFunctionIdentities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Functions
{
    public class FunctionIdentity
    {
        public static readonly Dictionary<string, FunctionIdentity> DefaultFunctions = new Dictionary<string, FunctionIdentity>()
        {
            { "max", MaxIdentity.Instance },
            { "select", SelectIdentity.Instance }
        };

        private readonly List<string> parameterNames;
        private readonly int hashSeed;

        public readonly Expression AtomicExpression;

        public delegate Expression.ExpressionDelegate GetDelegateDelegate(Dictionary<string, Expression.ExpressionDelegate> parameterDelegates);
        public delegate Expression GetDerivativeDelegate(Dictionary<string, Expression> parameterExpressions, Variable wrt);

        private readonly GetDelegateDelegate getDelegate;
        private readonly GetDerivativeDelegate getDerivative;

        public FunctionIdentity(List<string> parameterNames, int hashSeed, Expression atomicExpression, GetDelegateDelegate getDelegate, GetDerivativeDelegate getDerivative)
        {
            this.parameterNames = parameterNames ?? throw new ArgumentNullException(nameof(parameterNames));
            this.hashSeed = hashSeed;

            this.AtomicExpression = atomicExpression;

            this.getDelegate = getDelegate;
            this.getDerivative = getDerivative;
        }

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
        public Expression CreateExpression(Dictionary<string, Expression> nodes)
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


        public Expression.ExpressionDelegate GetDelegate(Dictionary<string, Expression.ExpressionDelegate> parameterDelegates)
        {
            return getDelegate(parameterDelegates);
        }

        public Expression GetDerivative(Function function, Variable wrt)
        {
            if (!function.GetIdentity().Equals(this))
            {
                throw new ArgumentException("Function is not of this identity");
            }

            Dictionary<string, Expression> parameterExpressions = function.GetParameters();
            return getDerivative(parameterExpressions, wrt);
        }
    }
}
