using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Algebra
{
    namespace Functions
    {
        internal class Function : Expression, IFunction
        {
            private readonly IFunctionIdentity identity;
            private readonly ReadOnlyDictionary<string, Expression> parameters;

            public Function(IFunctionIdentity identity, IDictionary<string, Expression> parameters)
            {
                // Ensure that all required parameters are filled
                if (!identity.AreParametersSatisfied(parameters))
                {
                    throw new ArgumentException("Incorrect function parameters were provided");
                }

                this.identity = identity;
                this.parameters = new ReadOnlyDictionary<string, Expression>(parameters);
            }

            public ReadOnlyDictionary<string, Expression> GetParameters()
            {
                return parameters;
            }

            public ReadOnlyCollection<Expression> GetParameterList()
            {
                List<Expression> parameterList = new List<Expression>();

                foreach (string name in identity.GetRequiredParameters())
                {
                    parameterList.Add(parameters[name]);
                }

                return parameterList.AsReadOnly();
            }

            public IFunctionIdentity GetIdentity()
            {
                return identity;
            }

            public override Expression GetDerivative(string wrt)
            {
                return GetAtomicExpression().GetDerivative(wrt);
            }

            protected override bool ExactlyEquals(Expression expression)
            {
                if (!(expression is Function function))
                {
                    return false;
                }

                if (!identity.Equals(function.identity))
                {
                    return false;
                }

                IDictionary<string, Expression> p1 = GetParameters();
                IDictionary<string, Expression> p2 = function.GetParameters();

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
                    if (!expression1.Equals(expression2, EqualityLevel.Exactly))
                    {
                        return false;
                    }
                }

                return true;
            }

            protected override int GenHashCode()
            {
                int value = identity.GetHashSeed();

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

            public override Expression MapChildren(ExpressionMapping.ExpressionMap map)
            {
                IDictionary<string, Expression> parameters = GetParameters();

                Dictionary<string, Expression> mappedParameters = new Dictionary<string, Expression>(parameters.Count);

                foreach (string parameterName in parameters.Keys)
                {
                    mappedParameters.Add(parameterName, map(parameters[parameterName]));
                }

                return identity.CreateExpression(mappedParameters);
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();

                IList<string> paramNames = identity.GetRequiredParameters();

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

            protected override Expression GenAtomicExpression()
            {
                Expression atomicVariabledExpression = identity.GetBodyAsAtomicExpression();

                // Replace variables with their expressions
                Expression atomicExpression = atomicVariabledExpression.PostMap(new ExpressionMapping()
                {
                    ShouldMapThis = eq => eq is Variable,
                    Map = expression =>
                    {
                        switch (expression)
                        {
                            case Variable v:
                                return parameters[v.Name].GetAtomicExpression();
                            default:
                                return expression;
                        }
                    }
                });

                return atomicExpression;
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateFunction(this);
            }
        }
    }
}
