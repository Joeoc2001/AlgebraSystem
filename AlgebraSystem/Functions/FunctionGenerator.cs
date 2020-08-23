using Algebra.Functions.HardcodedFunctionIdentities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Functions
{
    public abstract class FunctionGenerator : IFunctionGenerator
    {
        private static readonly IFunctionGenerator sinFunctionGenerator  = new AtomicFunctionGenerator("sin", 1, list => Expression.SinOf(list[0]));
        private static readonly IFunctionGenerator lnFunctionGenerator   = new AtomicFunctionGenerator("ln", 1, list => Expression.LnOf(list[0]));
        private static readonly IFunctionGenerator signFunctionGenerator = new AtomicFunctionGenerator("sign", 1, list => Expression.SignOf(list[0]));

        public static readonly Dictionary<string, IFunctionGenerator> DefaultFunctions = new Dictionary<string, IFunctionGenerator>()
        {
            { "sin", sinFunctionGenerator },
            { "cos", CosIdentity.Instance },
            { "tan", TanIdentity.Instance },
            { "ln", lnFunctionGenerator },
            { "log", LogIdentity.Instance },
            { "sign", signFunctionGenerator },
            { "max", MaxIdentity.Instance },
            { "min", MinIdentity.Instance },
            { "select", SelectIdentity.Instance },
            { "abs", AbsIdentity.Instance },
            { "sinh", SinhIdentity.Instance },
            { "cosh", CoshIdentity.Instance },
            { "tanh", TanhIdentity.Instance },
        };

        private readonly string name;
        private readonly ReadOnlyCollection<string> parameterNames;

        protected FunctionGenerator(string name, List<string> parameterNames)
            : this(name, parameterNames.AsReadOnly())
        {

        }

        protected FunctionGenerator(string name, ReadOnlyCollection<string> parameterNames)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.parameterNames = parameterNames ?? throw new ArgumentNullException(nameof(parameterNames));
        }

        protected abstract IExpression CreateExpressionImpl(IDictionary<string, IExpression> parameters);

        public ReadOnlyCollection<string> GetRequiredParameters()
        {
            return parameterNames;
        }

        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Return a new function node from the given parameters.
        /// This constructor matches the parameter positions with the parameter names internally
        /// </summary>
        /// <param name="nodes">The parameters to be given to the function</param>
        /// <returns>The new function node</returns>
        public IExpression CreateExpression(IList<IExpression> nodes)
        {
            IList<string> requiredParameters = GetRequiredParameters();

            if (nodes.Count != requiredParameters.Count)
            {
                throw new ArgumentException("Incorrect number of parameters were provided");
            }

            Dictionary<string, IExpression> parameters = new Dictionary<string, IExpression>();
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
        public IExpression CreateExpression(IDictionary<string, IExpression> parameters)
        {
            if (!AreParametersSatisfied(parameters))
            {
                throw new ArgumentException("Parameters do not satisfy required parameters");
            }
            return CreateExpressionImpl(parameters);
        }

        public bool AreParametersSatisfied(IDictionary<string, IExpression> parameters)
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
