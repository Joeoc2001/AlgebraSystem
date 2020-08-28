using Rationals;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;


namespace Algebra
{
    namespace Atoms
    {
        internal class Arcsin : AtomicMonad
        {
            new public static IExpression ArcsinOf(IExpression argument)
            {
                return new Arcsin(argument);
            }

            public Arcsin(IExpression argument)
                : base(argument)
            {

            }

            public override IExpression GetDerivative(string wrt)
            {
                IExpression derivative = _argument.GetDerivative(wrt);
                return derivative * (1 / Pow(1 - Pow(_argument, ConstantFrom(2)), ConstantFrom(0.5)));
            }

            protected override bool ExactlyEquals(IExpression expression)
            {
                if (!(expression is Arcsin arcsin))
                {
                    return false;
                }

                return _argument.Equals(arcsin._argument);
            }

            public override Func<IExpression, IExpression> GetSimplifyingConstructor()
            {
                return ArcsinOf;
            }

            protected override int GetHashSeed()
            {
                return 2093291169;
            }

            protected override string GetMonadFunctionName()
            {
                return "arcsin";
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateArcsin(_argument);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateArcsin(this, _argument);
            }

            public override T Evaluate<T>(IExpression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Arcsin other)
                {
                    return evaluator.EvaluateArcsins(this._argument, other._argument);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}