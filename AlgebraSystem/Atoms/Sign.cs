﻿using Rationals;
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
            new public static IExpression SignOf(IExpression argument)
            {
                if (argument is Sign s)
                {
                    return s;
                }

                if (argument is Constant constant)
                {
                    if (constant.GetValue().IsZero)
                    {
                        return Zero;
                    }
                    if (constant.GetValue() > 0)
                    {
                        return One;
                    }
                    if (constant.GetValue() < 0)
                    {
                        return MinusOne;
                    }
                }

                return new Sign(argument);
            }

            private Sign(IExpression argument)
                : base(argument)
            {

            }

            public override IExpression GetDerivative(string wrt)
            {
                return Zero; // Not always true, but true 100% of the time :P
            }

            public override Func<IExpression, IExpression> GetSimplifyingConstructor()
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
                return evaluator.EvaluateSign(_argument);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateSign(this, _argument);
            }

            public override T Evaluate<T>(IExpression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Sign other)
                {
                    return evaluator.EvaluateSigns(this._argument, other._argument);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}