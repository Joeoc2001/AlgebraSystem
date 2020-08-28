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
                IExpression derivative = _argument.GetDerivative(wrt);
                return derivative * CosOf(_argument);
            }

            protected override bool ExactlyEquals(IExpression expression)
            {
                if (!(expression is Sin sin))
                {
                    return false;
                }

                return _argument.Equals(sin._argument);
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
                return evaluator.EvaluateSin(_argument);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateSin(this, _argument);
            }

            public override T Evaluate<T>(IExpression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Sin other)
                {
                    return evaluator.EvaluateSins(this._argument, other._argument);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}