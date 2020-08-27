using Algebra.Atoms;
using Algebra.Evaluators;
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
            private readonly ReadOnlyDictionary<string, IExpression> parameters;

            public Function(IFunctionIdentity identity, IDictionary<string, IExpression> parameters)
            {
                // Ensure that all required parameters are filled
                if (!identity.AreParametersSatisfied(parameters))
                {
                    throw new ArgumentException("Incorrect function parameters were provided");
                }

                this.identity = identity;
                this.parameters = new ReadOnlyDictionary<string, IExpression>(parameters);
            }

            public ReadOnlyDictionary<string, IExpression> GetParameters()
            {
                return parameters;
            }

            public ReadOnlyCollection<IExpression> GetParameterList()
            {
                List<IExpression> parameterList = new List<IExpression>();

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

            public override IExpression GetDerivative(string wrt)
            {
                return GetAtomicExpression().GetDerivative(wrt);
            }

            protected override bool ExactlyEquals(IExpression expression)
            {
                if (!(expression is Function function))
                {
                    return false;
                }

                if (!identity.Equals(function.identity))
                {
                    return false;
                }

                IDictionary<string, IExpression> p1 = GetParameters();
                IDictionary<string, IExpression> p2 = function.GetParameters();

                if (p1.Count != p2.Count)
                {
                    return false;
                }

                foreach (string parameterName in p1.Keys)
                {
                    if (!p2.TryGetValue(parameterName, out IExpression expression2))
                    {
                        return false; // The parameter with given name doesn't exist in p2
                    }
                    IExpression expression1 = p1[parameterName];
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

                IDictionary<string, IExpression> parameters = GetParameters();
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

            protected override IAtomicExpression GenAtomicExpression()
            {
                IExpression atomicVariabledExpression = identity.GetBodyAsAtomicExpression();

                // Replace variables with their expressions
                Dictionary<string, IExpression> atomicReplacements = new Dictionary<string, IExpression>();
                foreach (var parameter in parameters)
                {
                    atomicReplacements.Add(parameter.Key, parameter.Value.GetAtomicExpression());
                }
                IExpression atomicExpression = atomicVariabledExpression.Evaluate(new VariableReplacementEvaluator(atomicReplacements));

                return AtomicExpression.GetAtomicExpression(atomicExpression);
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateFunction(this);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateFunction(this);
            }

            public override T Evaluate<T>(IExpression other, IDualEvaluator<T> evaluator)
            {
                if (other is Function function)
                {
                    return evaluator.EvaluateFunctions(this, function);
                }
                return evaluator.EvaluateOthers(this, other);
            }

            public bool Equals(IFunction other)
            {
                return Equals((IExpression)other);
            }
        }
    }
}
