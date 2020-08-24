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
            new public static IExpression SinOf(IExpression argument)
            {
                return new Sin(argument);
            }

            public Sin(IExpression argument)
                : base(argument)
            {

            }

            public override IExpression GetDerivative(string wrt)
            {
                IExpression derivative = argument.GetDerivative(wrt);
                return derivative * CosOf(argument);
            }

            protected override bool ExactlyEquals(IExpression expression)
            {
                if (!(expression is Sin sin))
                {
                    return false;
                }

                return argument.Equals(sin.argument);
            }

            public override Func<IExpression, IExpression> GetSimplifyingConstructor()
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
                return evaluator.EvaluateSin(argument);
            }

            public override T DualEvaluate<T>(IExpression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Sin other)
                {
                    return evaluator.EvaluateSins(this.argument, other.argument);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}