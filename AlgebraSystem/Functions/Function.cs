﻿using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Algebra.Functions
{
    public class Function: Expression
    {
        private readonly FunctionIdentity identity;
        private readonly Dictionary<string, Expression> parameters;

        public Function(FunctionIdentity identity, Dictionary<string, Expression> parameters)
        {
            // Ensure that all required parameters are filled
            if (!identity.AreParametersSatisfied(parameters))
            {
                throw new ArgumentException("Incorrect function parameters were provided");
            }

            this.identity = identity;
            this.parameters = parameters;
        }

        public Dictionary<string, Expression> GetParameters()
        {
            return parameters;
        }

        public FunctionIdentity GetIdentity()
        {
            return identity;
        }

        public override ExpressionDelegate GetDelegate(VariableInputSet set)
        {
            // Create delegates for parameters
            Dictionary<string, ExpressionDelegate> parameterDelegates = new Dictionary<string, ExpressionDelegate>();
            foreach (KeyValuePair<string, Expression> parameter in parameters)
            {
                ExpressionDelegate newDelegate = parameter.Value.GetDelegate(set);
                parameterDelegates.Add(parameter.Key, newDelegate);
            }

            // Create a delegate from the parameter expressions
            return identity.GetDelegate(parameterDelegates);
        }

        public override Expression GetDerivative(Variable wrt)
        {
            return identity.GetDerivative(this, wrt);
        }

        public bool Equals(Function obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!identity.Equals(obj.identity))
            {
                return false;
            }

            IDictionary<string, Expression> p1 = GetParameters();
            IDictionary<string, Expression> p2 = obj.GetParameters();

            if (p1.Count != p2.Count)
            {
                return false;
            }

            foreach (string parameterName in p1.Keys)
            {
                if (!p2.TryGetValue(parameterName, out Expression expression2))
                {
                    return false; // The parameter with given name doesn't exist in p2
                }
                Expression expression1 = p1[parameterName];
                if (!expression1.Equals(expression2))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(Expression obj)
        {
            return Equals(obj as Function);
        }

        public override int GenHashCode()
        {
            int value = identity.GetHashSeed();

            Dictionary<string, Expression> parameters = GetParameters();
            List<string> parameterNames = parameters.Keys.ToList();
            parameterNames.Sort(StringComparer.CurrentCulture);

            foreach (string paramName in parameterNames)
            {
                value *= 33;
                value ^= parameters[paramName].GenHashCode();
            }

            return value;
        }

        public override int GetOrderIndex()
        {
            return 0;
        }

        public override Expression Map(ExpressionMapping map)
        {
            Expression currentThis = this;

            IDictionary<string, Expression> parameters = GetParameters();

            if (map.ShouldMapChildren(this))
            {
                Dictionary<string, Expression> mappedParameters = new Dictionary<string, Expression>(parameters.Count);

                foreach (string parameterName in parameters.Keys)
                {
                    mappedParameters.Add(parameterName, parameters[parameterName].Map(map));
                }

                currentThis = identity.CreateExpression(mappedParameters);
            }

            if (map.ShouldMapThis(this))
            {
                currentThis = map.PostMap(currentThis);
            }

            return currentThis;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            ReadOnlyCollection<string> paramNames = identity.GetRequiredParameters();

            builder.Append(identity.GetName());
            builder.Append(" (");
            for (int i = 0; i < paramNames.Count; i++)
            {
                builder.Append(parameters[paramNames[i]]);
                if (i < paramNames.Count - 1)
                {
                    builder.Append(", ");
                }
            }
            builder.Append(")");

            return builder.ToString();
        }

        public Expression GetEquivalentAtomicExpression()
        {
            Expression atomicVariabledExpression = identity.AtomicExpression;

            // Replace variables with their expressions
            Expression atomicExpression = atomicVariabledExpression.Map(new ExpressionMapping()
            {
                PostMap = expression =>
                {
                    switch (expression)
                    {
                        case Variable v:
                            return parameters[v.Name];
                        default:
                            return expression;
                    }
                }
            });

            return atomicExpression;
        }
    }
}
