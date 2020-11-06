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
                Expression derivative = _argument.GetDerivative(wrt);
                return derivative * CosOf(_argument);
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

            public override void Map(IMapping mapping)
            {
                mapping.EvaluateSin(_argument);
            }

            public override T Map<T>(IMapping<T> mapping)
            {
                return mapping.EvaluateSin(_argument);
            }

            public override T Map<T>(IExtendedMapping<T> mapping)
            {
                return mapping.EvaluateSin(this, _argument);
            }

            public override T Map<T>(Expression otherExpression, IDualMapping<T> mapping)
            {
                if (otherExpression is Sin other)
                {
                    return mapping.EvaluateSins(this._argument, other._argument);
                }
                return mapping.EvaluateOthers(this, otherExpression);
            }
        }
    }
}