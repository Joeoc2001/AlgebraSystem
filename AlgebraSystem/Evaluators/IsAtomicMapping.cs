using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.mappings
{
    public class IsAtomicMapping : IMapping<bool>
    {
        public static readonly IsAtomicMapping Instance = new IsAtomicMapping();

        protected IsAtomicMapping()
        {
        }

        public bool EvaluateConstant(IConstant value)
        {
            return true;
        }

        public bool EvaluateExponent(Expression baseExpression, Expression powerExpression)
        {
            return baseExpression.Map(this) && powerExpression.Map(this);
        }

        public bool EvaluateFunction(Function function)
        {
            return false;
        }

        public bool EvaluateLn(Expression argumentExpression)
        {
            return argumentExpression.Map(this);
        }

        public bool EvaluateSet(ICollection<Expression> expressions)
        {
            foreach (Expression expression in expressions)
            {
                if (!expression.Map(this))
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
            return argumentExpression.Map(this);
        }

        public bool EvaluateSin(Expression argumentExpression)
        {
            return argumentExpression.Map(this);
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
            return argumentExpression.Map(this);
        }

        public bool EvaluateArctan(Expression argumentExpression)
        {
            return argumentExpression.Map(this);
        }

        public bool EvaluateOther(Expression other)
        {
            throw new NotImplementedException($"Cannot check if {other} is atomic. Override {typeof(IsAtomicMapping).Name} to add functionality for your new class.");
        }
    }
}
