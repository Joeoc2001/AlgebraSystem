using Algebra.Atoms;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IEvaluator<T>
    {
        T EvaluateConstant(Rational value);
        T EvaluateVariable(string name);
        T EvaluateSum(ICollection<Expression> expressions);
        T EvaluateProduct(ICollection<Expression> expressions);
        T EvaluateExponent(Expression baseExpression, Expression powerExpression);
        T EvaluateLn(Expression argumentExpression);
        T EvaluateSign(Expression argumentExpression);
        T EvaluateSin(Expression argumentExpression);
        T EvaluateFunction(IFunction function);
    }
}
