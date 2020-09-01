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
                IExpression derivative = _argument.GetDerivative(wrt);
                return derivative / _argument;
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
                return evaluator.EvaluateLn(_argument);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateLn(this, _argument);
            }

            public override T Evaluate<T>(IExpression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Ln other)
                {
                    return evaluator.EvaluateLns(this._argument, other._argument);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}