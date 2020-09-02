using Algebra.Atoms;
using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IExpandedEvaluator<T>
    {
        T EvaluateConstant(Expression expression, Rational value);
        T EvaluateVariable(Expression expression, string name);
        T EvaluateSum(Expression expression, ICollection<Expression> expressions);
        T EvaluateProduct(Expression expression, ICollection<Expression> expressions);
        T EvaluateExponent(Expression expression, Expression baseExpression, Expression powerExpression);
        T EvaluateLn(Expression expression, Expression argumentExpression);
        T EvaluateSign(Expression expression, Expression argumentExpression);
        T EvaluateSin(Expression expression, Expression argumentExpression);
        T EvaluateArcsin(Expression expression, Expression argumentExpression);
        T EvaluateArctan(Expression expression, Expression argumentExpression);
        T EvaluateFunction(Function function);
        T EvaluateOther(Expression other);
    }
}
