﻿using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public abstract class DefaultEvaluator<T> : IEvaluator<T>
    {
        public abstract T Default();

        public virtual T EvaluateArcsin(Expression argumentExpression)
        {
            return Default();
        }

        public virtual T EvaluateArctan(Expression argumentExpression)
        {
            return Default();
        }

        public virtual T EvaluateConstant(Rational value)
        {
            return Default();
        }

        public virtual T EvaluateExponent(Expression baseExpression, Expression powerExpression)
        {
            return Default();
        }

        public virtual T EvaluateFunction(Function function)
        {
            return Default();
        }

        public virtual T EvaluateLn(Expression argumentExpression)
        {
            return Default();
        }

        public T EvaluateOther(Expression other)
        {
            return Default();
        }

        public virtual T EvaluateProduct(ICollection<Expression> expressions)
        {
            return Default();
        }

        public virtual T EvaluateSign(Expression argumentExpression)
        {
            return Default();
        }

        public virtual T EvaluateSin(Expression argumentExpression)
        {
            return Default();
        }

        public virtual T EvaluateSum(ICollection<Expression> expressions)
        {
            return Default();
        }

        public virtual T EvaluateVariable(string name)
        {
            return Default();
        }
    }
}
