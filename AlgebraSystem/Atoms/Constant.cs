using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Atoms
    {
        internal abstract class Constant : Expression, IConstant
        {

            protected override abstract int GenHashCode();
            public override abstract string ToString();
            public abstract bool IsRational();
            public abstract Rational GetRationalApproximation();
            public abstract double GetDoubleApproximation();
            public abstract bool Equals(IConstant other);

            public virtual int CompareTo(IConstant other)
            {
                return GetRationalApproximation().CompareTo(other.GetRationalApproximation());
            }

            public override int GetOrderIndex()
            {
                return 0;
            }

            public override Expression GetDerivative(string wrt)
            {
                return Zero;
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateConstant(this);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateConstant(this, this);
            }

            public override T Evaluate<T>(Expression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is IConstant other)
                {
                    return evaluator.EvaluateConstants(this, other);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }

            protected override Expression GenAtomicExpression()
            {
                return this;
            }

            public Expression ToExpression()
            {
                return this;
            }
        }
    }
}
