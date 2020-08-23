using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluation
{
    public abstract class ValueEvaluator<T> : IEvaluator<T>
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

        public T EvaluateConstant(Rational value)
        {
            return Map(GetFromRational(value));
        }

        protected abstract T Pow(T b, T e);

        public T EvaluateExponent(Expression baseExpression, Expression powerExpression)
        {
            T baseValue = baseExpression.Evaluate(this);
            T powerValue = powerExpression.Evaluate(this);

            return Map(Pow(baseValue, powerValue));
        }

        protected abstract ValueEvaluator<T> Construct(IDictionary<IFunctionIdentity, FunctionEvaluator> functionEvaluators, VariableInputSet<T> variableInputs);

        public T EvaluateFunction(IFunction function)
        {
            IFunctionIdentity identity = function.GetIdentity();

            // Evaluate parameters
            IList<Expression> parameters = function.GetParameterList();
            List<T> evaluated = new List<T>();
            foreach (Expression expression in parameters)
            {
                evaluated.Add(expression.Evaluate(this));
            }

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
            ValueEvaluator<T> rationalEvaluator = Construct(functionEvaluators, variableInputs);
            return Map(function.GetAtomicExpression().Evaluate(rationalEvaluator));
        }

        protected abstract T Ln(T v);

        public T EvaluateLn(Expression argumentExpression)
        {
            return Map(Ln(argumentExpression.Evaluate(this)));
        }

        protected abstract T Product(ICollection<T> expressions);

        public T EvaluateProduct(ICollection<Expression> expressions)
        {
            List<T> evaluated = new List<T>();
            foreach (Expression expression in expressions)
            {
                evaluated.Add(expression.Evaluate(this));
            }
            return Map(Product(evaluated));
        }

        protected abstract T Sign(T v);

        public T EvaluateSign(Expression argumentExpression)
        {
            return Map(Sign(argumentExpression.Evaluate(this)));
        }

        protected abstract T Sin(T v);

        public T EvaluateSin(Expression argumentExpression)
        {
            return Map(Sin(argumentExpression.Evaluate(this)));
        }

        protected abstract T Sum(ICollection<T> expressions);

        public T EvaluateSum(ICollection<Expression> expressions)
        {
            List<T> evaluated = new List<T>();
            foreach (Expression expression in expressions)
            {
                evaluated.Add(expression.Evaluate(this));
            }
            return Map(Sum(evaluated));
        }

        public T EvaluateVariable(string name)
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
