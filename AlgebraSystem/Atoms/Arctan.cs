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
            new public static IExpression ArctanOf(IExpression argument)
            {
                return new Arctan(argument);
            }

            public Arctan(IExpression argument)
                : base(argument)
            {

            }

            public override IExpression GetDerivative(string wrt)
            {
                IExpression derivative = _argument.GetDerivative(wrt);
                return derivative / (1 + Pow(_argument, ConstantFrom(2)));
            }

            public override Func<IExpression, IExpression> GetSimplifyingConstructor()
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

            public override T Evaluate<T>(IExpression otherExpression, IDualEvaluator<T> evaluator)
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