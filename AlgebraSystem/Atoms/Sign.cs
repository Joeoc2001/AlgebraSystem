using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Algebra
{
    namespace Atoms
    {
        internal class Sign : AtomicMonad
        {
            new public static Expression SignOf(Expression argument)
            {
                if (argument is Sign s)
                {
                    return s;
                }

                if (argument is RationalConstant constant)
                {
                    if (constant.GetValue().IsZero)
                    {
                        return Zero;
                    }
                    if (constant.GetValue() > 0)
                    {
                        return One;
                    }
                    if (constant.GetValue() < 0)
                    {
                        return MinusOne;
                    }
                }

                return new Sign(argument);
            }

            private Sign(Expression argument)
                : base(argument)
            {

            }

            public override Expression GetDerivative(string wrt)
            {
                return Zero; // Not always true, but true 100% of the time :P
            }

            public override Func<Expression, Expression> GetSimplifyingConstructor()
            {
                return SignOf;
            }

            protected override int GetHashSeed()
            {
                return -322660314;
            }

            protected override string GetMonadFunctionName()
            {
                return "sign";
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateSign(_argument);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateSign(this, _argument);
            }

            public override T Evaluate<T>(Expression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Sign other)
                {
                    return evaluator.EvaluateSigns(this._argument, other._argument);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}