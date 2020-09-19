using Algebra.Atoms;
using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IEvaluator<T>
    {
        T EvaluateConstant(IConstant value);
        T EvaluateVariable(IVariable value);
        T EvaluateSum(ICollection<Expression> expressions);
        T EvaluateProduct(ICollection<Expression> expressions);
        T EvaluateExponent(Expression baseExpression, Expression powerExpression);
        T EvaluateLn(Expression argumentExpression);
        T EvaluateSign(Expression argumentExpression);
        T EvaluateSin(Expression argumentExpression);
        T EvaluateArcsin(Expression argumentExpression);
        T EvaluateArctan(Expression argumentExpression);
        T EvaluateFunction(Function function);
        T EvaluateOther(Expression other);
    }
}
