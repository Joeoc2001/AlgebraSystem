using Algebra.Atoms;
using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IExtendedMapping<T>
    {
        T EvaluateConstant(Expression expression, IConstant value);
        T EvaluateVariable(Expression expression, IVariable value);
        T EvaluateSum(Expression expression, ICollection<Expression> argumentExpressions);
        T EvaluateProduct(Expression expression, ICollection<Expression> argumentExpressions);
        T EvaluateExponent(Expression expression, Expression baseExpression, Expression powerExpression);
        T EvaluateLn(Expression expression, Expression argumentExpression);
        T EvaluateSign(Expression expression, Expression argumentExpression);
        T EvaluateSin(Expression expression, Expression argumentExpression);
        T EvaluateArcsin(Expression expression, Expression argumentExpression);
        T EvaluateArctan(Expression expression, Expression argumentExpression);
        T EvaluateFunction(Function function);
    }
}
