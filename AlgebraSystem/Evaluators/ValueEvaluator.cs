using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public abstract class ValueEvaluator<T> : TraversalEvaluator<T>
    {
        public delegate T FunctionEvaluator(IList<T> argumentExpressions);

        private readonly Dictionary<FunctionIdentity, FunctionEvaluator> _functionEvaluators;
        private readonly VariableInputSet<T> _variableInputs;

        public ValueEvaluator(VariableInputSet<T> variableInputs, IDictionary<FunctionIdentity, FunctionEvaluator> functionEvaluators)
        {
            this._functionEvaluators = new Dictionary<FunctionIdentity, FunctionEvaluator>(functionEvaluators ?? throw new ArgumentNullException(nameof(functionEvaluators)));
            this._variableInputs = variableInputs;
        }

        /// <summary>
        /// Run at the end of each evaluation, useful for applying simplifications etc
        /// </summary>
        protected virtual T Map(T value)
        {
            return value;
        }

        protected abstract T GetFromConstant(IConstant value);

        public override sealed T EvaluateConstant(IConstant value)
        {
            return Map(GetFromConstant(value));
        }

        protected abstract T PowOf(T b, T e);

        protected override sealed T Pow(T baseValue, T powerValue)
        {
            return Map(PowOf(baseValue, powerValue));
        }

        protected abstract TraversalEvaluator<T> Construct(IDictionary<FunctionIdentity, FunctionEvaluator> functionEvaluators, VariableInputSet<T> variableInputs);

        protected override sealed T EvaluateFunction(Function function, IList<T> evaluated)
        {
            FunctionIdentity identity = function.GetIdentity();

            // Check for a faster method
            if (_functionEvaluators.TryGetValue(identity, out FunctionEvaluator evaluator))
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
            TraversalEvaluator<T> rationalEvaluator = Construct(_functionEvaluators, variableInputs);
            return Map(identity.GetBodyAsAtomicExpression().Evaluate(rationalEvaluator));
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

        public override sealed T EvaluateVariable(IVariable value)
        {
            string name = value.GetName();
            if (!_variableInputs.Contains(name))
            {
                throw new VariableNotPresentException($"Variable {name} could not be found in the given variable input set");
            }
            return Map(_variableInputs.Get(name).Value);
        }

        protected abstract T ArcsinOf(T v);

        protected override sealed T Arcsin(T expression)
        {
            return Map(ArcsinOf(expression));
        }

        protected abstract T ArctanOf(T v);

        protected override sealed T Arctan(T expression)
        {
            return Map(ArctanOf(expression));
        }
    }
}
