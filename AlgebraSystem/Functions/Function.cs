﻿using Algebra.Atoms;
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
            private readonly IFunctionIdentity _identity;
            private readonly ReadOnlyDictionary<string, IExpression> _parameters;

            public Function(IFunctionIdentity identity, IDictionary<string, IExpression> parameters)
            {
                // Ensure that all required parameters are filled
                if (!identity.AreParametersSatisfied(parameters))
                {
                    throw new ArgumentException("Incorrect function parameters were provided");
                }

                this._identity = identity;
                this._parameters = new ReadOnlyDictionary<string, IExpression>(parameters);
            }

            public ReadOnlyDictionary<string, IExpression> GetParameters()
            {
                return _parameters;
            }

            public ReadOnlyCollection<IExpression> GetParameterList()
            {
                List<IExpression> parameterList = new List<IExpression>();

                foreach (string name in _identity.GetRequiredParameters())
                {
                    parameterList.Add(_parameters[name]);
                }

                return parameterList.AsReadOnly();
            }

            public IFunctionIdentity GetIdentity()
            {
                return _identity;
            }

            public override IExpression GetDerivative(string wrt)
            {
                return GetAtomicExpression().GetDerivative(wrt);
            }

            protected override int GenHashCode()
            {
                int value = _identity.GetHashSeed();

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

            protected override IExpression GenAtomicExpression()
            {
                IExpression atomicVariabledExpression = _identity.GetBodyAsAtomicExpression();

                // Replace variables with their expressions
                Dictionary<string, IExpression> atomicReplacements = new Dictionary<string, IExpression>();
                foreach (var parameter in _parameters)
                {
                    atomicReplacements.Add(parameter.Key, parameter.Value.GetAtomicExpression());
                }
                return atomicVariabledExpression.Evaluate(new VariableReplacementEvaluator(atomicReplacements));
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
