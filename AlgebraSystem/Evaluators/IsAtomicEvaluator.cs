using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public class IsAtomicEvaluator : IEvaluator<bool>
    {
        public static readonly IsAtomicEvaluator Instance = new IsAtomicEvaluator();

        protected IsAtomicEvaluator()
        {
        }

        public bool EvaluateConstant(IConstant value)
        {
            return true;
        }

        public bool EvaluateExponent(Expression baseExpression, Expression powerExpression)
        {
            return baseExpression.Evaluate(this) && powerExpression.Evaluate(this);
        }

        public bool EvaluateFunction(Function function)
        {
            return false;
        }

        public bool EvaluateLn(Expression argumentExpression)
        {
            return argumentExpression.Evaluate(this);
        }

        public bool EvaluateSet(ICollection<Expression> expressions)
        {
            foreach (Expression expression in expressions)
            {
                if (!expression.Evaluate(this))
                {
                    return false;
                }
            }
            return true;
        }

        public bool EvaluateProduct(ICollection<Expression> expressions)
        {
            return EvaluateSet(expressions);
        }

        public bool EvaluateSign(Expression argumentExpression)
        {
            return argumentExpression.Evaluate(this);
        }

        public bool EvaluateSin(Expression argumentExpression)
        {
            return argumentExpression.Evaluate(this);
        }

        public bool EvaluateSum(ICollection<Expression> expressions)
        {
            return EvaluateSet(expressions);
        }

        public bool EvaluateVariable(IVariable value)
        {
            return true;
        }

        public bool EvaluateArcsin(Expression argumentExpression)
        {
            return argumentExpression.Evaluate(this);
        }

        public bool EvaluateArctan(Expression argumentExpression)
        {
            return argumentExpression.Evaluate(this);
        }

        public bool EvaluateOther(Expression other)
        {
            throw new NotImplementedException($"Cannot check if {other} is atomic. Override {typeof(IsAtomicEvaluator).Name} to add functionality for your new class.");
        }
    }
}
