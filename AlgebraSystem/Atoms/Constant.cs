using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Atoms
{
    internal abstract class Constant : Expression
    {
        public static implicit operator Constant(int r) => RationalConstant.FromValue(r);
        public static implicit operator Constant(long r) => RationalConstant.FromValue(r);
        public static implicit operator Constant(float r) => Rational.Approximate(r);
        public static implicit operator Constant(double r) => Rational.Approximate(r);
        public static implicit operator Constant(decimal r) => Rational.Approximate(r);
        public static implicit operator Constant(Rational r) => RationalConstant.FromValue(r);

        public override Expression GetDerivative(string wrt)
        {
            return Zero;
        }

        public abstract IConstantValue GetConstantValue();

        protected override int GenHashCode()
        {
            return GetConstantValue().GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetConstantValue()}";
        }

        public override int GetOrderIndex()
        {
            return 0;
        }

        public override T Evaluate<T>(IEvaluator<T> evaluator)
        {
            return evaluator.EvaluateConstant(GetConstantValue());
        }

        public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
        {
            return evaluator.EvaluateConstant(this, GetConstantValue());
        }

        public override T Evaluate<T>(Expression otherExpression, IDualEvaluator<T> evaluator)
        {
            if (otherExpression is RationalConstant other)
            {
                return evaluator.EvaluateConstants(this.GetConstantValue(), other.GetConstantValue());
            }
            return evaluator.EvaluateOthers(this, otherExpression);
        }

        protected override Expression GenAtomicExpression()
        {
            return this;
        }
    }
}
