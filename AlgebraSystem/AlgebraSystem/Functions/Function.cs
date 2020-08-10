using Algebra.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.Functions
{
    public class Function : Expression
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
            // Create expressions for parameters
            Dictionary<string, ExpressionDelegate> parameterExpressions = new Dictionary<string, ExpressionDelegate>();
            foreach (KeyValuePair<string, Expression> parameter in parameters)
            {
                ExpressionDelegate newDelegate = parameter.Value.GetDelegate(set);
                parameterExpressions.Add(parameter.Key, newDelegate);
            }

            // Create an expression from the parameter expressions
            return identity.GetExpression(parameterExpressions);
        }

        public override Expression GetDerivative(Variable wrt)
        {
            return identity.GetDerivative(wrt);
        }


        public bool Equals(Function obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
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
                if (!p2.TryGetValue(parameterName, out Expression equation2))
                {
                    return false; // The parameter with given name doesn't exist in p2
                }
                Expression equation1 = p1[parameterName];
                if (!equation1.Equals(equation2))
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

        public override Expression Map(EquationMapping map)
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

                currentThis = identity.CreateEquation(mappedParameters);
            }

            if (map.ShouldMapThis(this))
            {
                currentThis = map.PostMap(currentThis);
            }

            return currentThis;
        }

        [Obsolete]
        public override string ToRunnableString()
        {
            throw new NotImplementedException();
        }
    }
}
