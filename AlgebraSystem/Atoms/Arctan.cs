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
                IExpression derivative = argument.GetDerivative(wrt);
                return derivative / (1 + Pow(argument, ConstantFrom(2)));
            }

            protected override bool ExactlyEquals(IExpression expression)
            {
                if (!(expression is Arctan arctan))
                {
                    return false;
                }

                return argument.Equals(arctan.argument);
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
                return evaluator.EvaluateArctan(argument);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateArctan(this, argument);
            }

            public override T Evaluate<T>(IExpression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Arctan other)
                {
                    return evaluator.EvaluateArctans(this.argument, other.argument);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}