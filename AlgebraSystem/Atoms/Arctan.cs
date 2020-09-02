using Rationals;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;


namespace Algebra
{
    namespace Atoms
    {
        internal class Arctan : AtomicMonad
        {
            new public static Expression ArctanOf(Expression argument)
            {
                return new Arctan(argument);
            }

            public Arctan(Expression argument)
                : base(argument)
            {

            }

            public override Expression GetDerivative(string wrt)
            {
                Expression derivative = _argument.GetDerivative(wrt);
                return derivative / (1 + Pow(_argument, ConstantFrom(2)));
            }

            public override Func<Expression, Expression> GetSimplifyingConstructor()
            {
                return ArctanOf;
            }

            protected override int GetHashSeed()
            {
                return 1726209880;
            }

            protected override string GetMonadFunctionName()
            {
                return "arctan";
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateArctan(_argument);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateArctan(this, _argument);
            }

            public override T Evaluate<T>(Expression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Arctan other)
                {
                    return evaluator.EvaluateArctans(this._argument, other._argument);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}