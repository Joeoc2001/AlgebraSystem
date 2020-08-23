using Rationals;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;


namespace Algebra
{
    namespace Atoms
    {
        internal class Ln : AtomicMonad
        {
            new public static Expression LnOf(Expression argument)
            {
                return new Ln(argument);
            }

            public Ln(Expression argument)
                : base(argument)
            {

            }

            public override Expression GetDerivative(string wrt)
            {
                Expression derivative = Argument.GetDerivative(wrt);
                return derivative / Argument;
            }

            protected override bool ExactlyEquals(Expression expression)
            {
                if (!(expression is Ln ln))
                {
                    return false;
                }

                return Argument.Equals(ln.Argument);
            }

            public override Func<Expression, Expression> GetSimplifyingConstructor()
            {
                return LnOf;
            }

            protected override int GetHashSeed()
            {
                return -1043105826;
            }

            protected override string GetMonadFunctionName()
            {
                return "ln";
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateLn(Argument);
            }
        }
    }
}