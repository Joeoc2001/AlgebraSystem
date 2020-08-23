using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public abstract class ValueEvaluator<T> : TraversalEvaluator<T>
    {
        public delegate T FunctionEvaluator(ICollection<T> argumentExpressions);

        private readonly IDictionary<IFunctionIdentity, FunctionEvaluator> functionEvaluators;
        private readonly VariableInputSet<T> variableInputs;

        public ValueEvaluator(VariableInputSet<T> variableInputs, IDictionary<IFunctionIdentity, FunctionEvaluator> functionEvaluators)
        {
            this.functionEvaluators = functionEvaluators ?? new Dictionary<IFunctionIdentity, FunctionEvaluator>();
            this.variableInputs = variableInputs;
        }

        /// <summary>
        /// Run at the end of each evaluation, useful for applying simplifications etc
        /// </summary>
        protected virtual T Map(T value)
        {
            return value;
        }

        protected abstract T GetFromRational(Rational value);

        public override sealed T EvaluateConstant(Rational value)
        {
            return Map(GetFromRational(value));
        }

        protected abstract T PowOf(T b, T e);

        protected override sealed T Pow(T baseValue, T powerValue)
        {
            return Map(PowOf(baseValue, powerValue));
        }

        protected abstract TraversalEvaluator<T> Construct(IDictionary<IFunctionIdentity, FunctionEvaluator> functionEvaluators, VariableInputSet<T> variableInputs);

        protected override sealed T EvaluateFunction(IFunction function, IList<T> evaluated)
        {
            IFunctionIdentity identity = function.GetIdentity();

            // Check for a faster method
            if (functionEvaluators.TryGetValue(identity, out FunctionEvaluator evaluator))
            {
                return Map(evaluator(evaluated));
            }

            // Evaluate fully
            VariableInputSet<T> variableInputs = new VariableInputSet<T>();
            var evaluatedEnumerator = evaluated.GetEnumerator();
            foreach (string variableName in identity.GetRequiredParameters())
            {
                evaluatedEnumerator.MoveNext();
                variableInputs.Add(variableName, evaluatedEnumerator.Current);
            }
            TraversalEvaluator<T> rationalEvaluator = Construct(functionEvaluators, variableInputs);
            return Map(function.GetAtomicExpression().Evaluate(rationalEvaluator));
        }

        protected abstract T LnOf(T v);

        protected override sealed T Ln(T argument)
        {
            return Map(LnOf(argument));
        }

        protected abstract T ProductOf(ICollection<T> expressions);

        protected override sealed T Product(ICollection<T> evaluated)
        {
            return Map(ProductOf(evaluated));
        }

        protected abstract T SignOf(T v);

        protected override sealed T Sign(T argument)
        {
            return Map(SignOf(argument));
        }

        protected abstract T SinOf(T v);

        protected override sealed T Sin(T argument)
        {
            return Map(SinOf(argument));
        }

        protected abstract T SumOf(ICollection<T> expressions);

        protected override sealed T Sum(ICollection<T> evaluated)
        {
            return Map(SumOf(evaluated));
        }

        public override sealed T EvaluateVariable(string name)
        {
            name = name.ToLower();
            if (!variableInputs.Contains(name))
            {
                throw new VariableNotPresentException($"Variable {name} could not be found in the given variable input set");
            }
            return Map(variableInputs.Get(name).Value);
        }
    }
}
