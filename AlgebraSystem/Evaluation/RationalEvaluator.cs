using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluation
{
    public class RationalEvaluator : IEvaluator<Rational>
    {
        public delegate Rational RationalFunctionEvaluator(ICollection<Rational> argumentExpressions);

        private readonly IDictionary<IFunctionIdentity, RationalFunctionEvaluator> functionEvaluators;
        private readonly VariableInputSet<Rational> variableInputs;

        public RationalEvaluator(IDictionary<IFunctionIdentity, RationalFunctionEvaluator> functionEvaluators, VariableInputSet<Rational> variableInputs)
        {
            this.functionEvaluators = functionEvaluators;
            this.variableInputs = variableInputs;
        }

        public Rational EvaluateConstant(Rational value)
        {
            return value;
        }

        public Rational EvaluateExponent(Expression baseExpression, Expression powerExpression)
        {
            Rational baseValue = baseExpression.Evaluate(this);
            Rational powerValue = powerExpression.Evaluate(this);

            if (powerValue.Numerator > int.MaxValue || powerValue.Numerator < int.MinValue
                || powerValue.Denominator > int.MaxValue || powerValue.Denominator < int.MinValue)
            {
                throw new ArgumentOutOfRangeException("Power is outside of int range. Try running again as an approximation");
            }

            // TODO: The output of this is not always rational (eg root 2)

            Rational finalValue = Rational.Pow(baseValue, (int)powerValue.Numerator);
            return Rational.RationalRoot(finalValue, (int)powerValue.Denominator);
        }

        public Rational EvaluateFunction(IFunction function)
        {
            IFunctionIdentity identity = function.GetIdentity();

            // Evaluate parameters
            IList<Expression> parameters = function.GetParameterList();
            List<Rational> evaluated = new List<Rational>();
            foreach (Expression expression in parameters)
            {
                evaluated.Add(expression.Evaluate(this));
            }

            // Check for a faster method
            if (functionEvaluators.TryGetValue(identity, out RationalFunctionEvaluator evaluator))
            {
                return evaluator(evaluated);
            }

            // Evaluate fully
            VariableInputSet<Rational> variableInputs = new VariableInputSet<Rational>();
            var evaluatedEnumerator = evaluated.GetEnumerator();
            foreach (string variableName in identity.GetRequiredParameters())
            {
                evaluatedEnumerator.MoveNext();
                variableInputs.Add(variableName, evaluatedEnumerator.Current);
            }
            RationalEvaluator rationalEvaluator = new RationalEvaluator(functionEvaluators, variableInputs);
            return function.GetAtomicExpression().Evaluate(rationalEvaluator);
        }

        public Rational EvaluateLn(Expression argumentExpression)
        {
            return (Rational)Rational.Log(argumentExpression.Evaluate(this));
        }

        public Rational EvaluateProduct(ICollection<Expression> expressions)
        {
            Rational evaluated = 1;
            foreach (Expression expression in expressions)
            {
                evaluated *= expression.Evaluate(this);
            }
            return evaluated;
        }

        public Rational EvaluateSign(Expression argumentExpression)
        {
            return argumentExpression.Evaluate(this).Sign;
        }

        public Rational EvaluateSin(Expression argumentExpression)
        {
            return (Rational)Math.Sin((double)argumentExpression.Evaluate(this));
        }

        public Rational EvaluateSum(ICollection<Expression> expressions)
        {
            Rational evaluated = 0;
            foreach (Expression expression in expressions)
            {
                evaluated += expression.Evaluate(this);
            }
            return evaluated;
        }

        public Rational EvaluateVariable(string name)
        {
            return variableInputs.Get(name).Value;
        }
    }
}
