using Algebra.Atoms;
using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IMapping<T>
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
    }

    public interface IMapping
    {
        void EvaluateConstant(IConstant value);
        void EvaluateVariable(IVariable value);
        void EvaluateSum(ICollection<Expression> expressions);
        void EvaluateProduct(ICollection<Expression> expressions);
        void EvaluateExponent(Expression baseExpression, Expression powerExpression);
        void EvaluateLn(Expression argumentExpression);
        void EvaluateSign(Expression argumentExpression);
        void EvaluateSin(Expression argumentExpression);
        void EvaluateArcsin(Expression argumentExpression);
        void EvaluateArctan(Expression argumentExpression);
        void EvaluateFunction(Function function);
    }
}
