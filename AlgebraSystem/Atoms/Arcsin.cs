using Rationals;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;


namespace Algebra
{
    namespace Atoms
    {
        internal class Arcsin : AtomicMonad
        {
            new public static Expression ArcsinOf(Expression argument)
            {
                return new Arcsin(argument);
            }

            public Arcsin(Expression argument)
                : base(argument)
            {

            }

            public override Expression GetDerivative(string wrt)
            {
                Expression derivative = _argument.GetDerivative(wrt);
                return derivative * (1 / Pow(1 - Pow(_argument, ConstantFrom(2)), ConstantFrom(0.5)));
            }

            public override Func<Expression, Expression> GetSimplifyingConstructor()
            {
                return ArcsinOf;
            }

            protected override int GetHashSeed()
            {
                return 2093291169;
            }

            protected override string GetMonadFunctionName()
            {
                return "arcsin";
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateArcsin(_argument);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateArcsin(this, _argument);
            }

            public override T Evaluate<T>(Expression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Arcsin other)
                {
                    return evaluator.EvaluateArcsins(this._argument, other._argument);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}