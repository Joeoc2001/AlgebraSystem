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
        T EvaluateSum(ICollection<IExpression> expressions);
        T EvaluateProduct(ICollection<IExpression> expressions);
        T EvaluateExponent(IExpression baseExpression, IExpression powerExpression);
        T EvaluateLn(IExpression argumentExpression);
        T EvaluateSign(IExpression argumentExpression);
        T EvaluateSin(IExpression argumentExpression);
        T EvaluateFunction(IFunction function);
    }
}
