using Rationals;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;


namespace Algebra
{
    namespace Atoms
    {
        internal class Sin : AtomicMonad
        {
            new public static Expression SinOf(Expression argument)
            {
                return new Sin(argument);
            }

            public Sin(Expression argument)
                : base(argument)
            {

            }

            public override Expression GetDerivative(string wrt)
            {
                Expression derivative = Argument.GetDerivative(wrt);
                return derivative * CosOf(Argument);
            }

            protected override bool ExactlyEquals(Expression expression)
            {
                if (!(expression is Sin sin))
                {
                    return false;
                }

                return Argument.Equals(sin.Argument);
            }

            public override Func<Expression, Expression> GetSimplifyingConstructor()
            {
                return SinOf;
            }

            protected override int GetHashSeed()
            {
                return 507056861;
            }

            protected override string GetMonadFunctionName()
            {
                return "sin";
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateSin(Argument);
            }
        }
    }
}