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

                if (argument is Constant constant)
                {
                    if (constant.GetValue().IsZero)
                    {
                        return 0;
                    }
                    if (constant.GetValue() > 0)
                    {
                        return 1;
                    }
                    if (constant.GetValue() < 0)
                    {
                        return -1;
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
                return 0; // Not always true, but true 100% of the time :P
            }

            protected override bool ExactlyEquals(Expression expression)
            {
                if (!(expression is Sign sign))
                {
                    return false;
                }

                return Argument.Equals(sign.Argument, EqualityLevel.Exactly);
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
                return evaluator.EvaluateSign(Argument);
            }
        }
    }
}