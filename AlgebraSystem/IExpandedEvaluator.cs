using Algebra.Atoms;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IExpandedEvaluator<T>
    {
        T EvaluateConstant(IExpression expression, Rational value);
        T EvaluateVariable(IExpression expression, string name);
        T EvaluateSum(IExpression expression, ICollection<IExpression> expressions);
        T EvaluateProduct(IExpression expression, ICollection<IExpression> expressions);
        T EvaluateExponent(IExpression expression, IExpression baseExpression, IExpression powerExpression);
        T EvaluateLn(IExpression expression, IExpression argumentExpression);
        T EvaluateSign(IExpression expression, IExpression argumentExpression);
        T EvaluateSin(IExpression expression, IExpression argumentExpression);
        T EvaluateArcsin(IExpression expression, IExpression argumentExpression);
        T EvaluateArctan(IExpression expression, IExpression argumentExpression);
        T EvaluateFunction(IFunction function);
        T EvaluateOther(IExpression other);
    }
}
