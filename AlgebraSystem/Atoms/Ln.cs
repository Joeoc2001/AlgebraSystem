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
            new public static IExpression LnOf(IExpression argument)
            {
                return new Ln(argument);
            }

            public Ln(IExpression argument)
                : base(argument)
            {

            }

            public override IExpression GetDerivative(string wrt)
            {
                IExpression derivative = argument.GetDerivative(wrt);
                return derivative / argument;
            }

            protected override bool ExactlyEquals(IExpression expression)
            {
                if (!(expression is Ln ln))
                {
                    return false;
                }

                return argument.Equals(ln.argument);
            }

            public override Func<IExpression, IExpression> GetSimplifyingConstructor()
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
                return evaluator.EvaluateLn(argument);
            }

            public override T DualEvaluate<T>(IExpression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Ln other)
                {
                    return evaluator.EvaluateLns(this.argument, other.argument);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}