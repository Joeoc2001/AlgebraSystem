using Algebra.Atoms;
using Algebra.mappings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Algebra
{
    namespace Functions
    {
        public class Function : Expression, IEquatable<Function>
        {
            private readonly FunctionIdentity _identity;
            private readonly ReadOnlyDictionary<string, Expression> _parameters;

            public Function(FunctionIdentity identity, IDictionary<string, Expression> parameters)
            {
                // Ensure that all required parameters are filled
                if (!identity.AreParametersSatisfied(parameters))
                {
                    throw new ArgumentException("Incorrect function parameters were provided");
                }

                this._identity = identity;
                this._parameters = new ReadOnlyDictionary<string, Expression>(parameters);
            }

            public ReadOnlyDictionary<string, Expression> GetParameters()
            {
                return _parameters;
            }

            public ReadOnlyCollection<Expression> GetParameterList()
            {
                List<Expression> parameterList = new List<Expression>();

                foreach (string name in _identity.GetRequiredParameters())
                {
                    parameterList.Add(_parameters[name]);
                }

                return parameterList.AsReadOnly();
            }
            
            public FunctionIdentity GetIdentity()
            {
                return _identity;
            }

            public override Expression GetDerivative(string wrt)
            {
                return GetAtomicExpression().GetDerivative(wrt);
            }

            protected override int GenHashCode()
            {
                int value = _identity.GetHashSeed();

                IDictionary<string, Expression> parameters = GetParameters();
                List<string> parameterNames = parameters.Keys.ToList();
                parameterNames.Sort(StringComparer.CurrentCulture);

                foreach (string paramName in parameterNames)
                {
                    value *= 33;
                    value ^= parameters[paramName].GetHashCode();
                }

                return value;
            }

            public override int GetOrderIndex()
            {
                return 0;
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();

                IList<string> paramNames = _identity.GetRequiredParameters();

                builder.Append(_identity.GetName());
                builder.Append(" (");
                for (int i = 0; i < paramNames.Count; i++)
                {
                    builder.Append(_parameters[paramNames[i]]);
                    if (i < paramNames.Count - 1)
                    {
                        builder.Append(", ");
                    }
                }
                builder.Append(")");

                return builder.ToString();
            }

            protected override Expression GenAtomicExpression()
            {
                Expression atomicVariabledExpression = _identity.GetBodyAsAtomicExpression();

                // Replace variables with their expressions
                Dictionary<string, Expression> atomicReplacements = new Dictionary<string, Expression>();
                foreach (var parameter in _parameters)
                {
                    atomicReplacements.Add(parameter.Key, parameter.Value.GetAtomicExpression());
                }
                return atomicVariabledExpression.Map(new VariableReplacementMapping(atomicReplacements));
            }

            /// <summary>
            /// Returns this expression but with this function replaced with its atomic form.
            /// E.g. tan(tan(x)) becomes sin(tan(x))/sin(tan(x))
            /// </summary>
            /// <returns>An expression with this function instead being atomic</returns>
            public Expression GetAtomicBodiedExpression()
            {
                Expression atomicVariabledExpression = _identity.GetBodyAsAtomicExpression();

                // Replace variables with their expressions
                return atomicVariabledExpression.Map(new VariableReplacementMapping(_parameters));
            }

            public override void Map(IMapping mapping)
            {
                mapping.EvaluateFunction(this);
            }

            public override T Map<T>(IMapping<T> mapping)
            {
                return mapping.EvaluateFunction(this);
            }

            public override T Map<T>(IExtendedMapping<T> mapping)
            {
                return mapping.EvaluateFunction(this);
            }

            public override T Map<T>(Expression other, IDualMapping<T> mapping)
            {
                if (other is Function function)
                {
                    return mapping.EvaluateFunctions(this, function);
                }
                return mapping.EvaluateOthers(this, other);
            }

            public bool Equals(Function other)
            {
                return Equals((Expression)other);
            }
        }
    }
}
