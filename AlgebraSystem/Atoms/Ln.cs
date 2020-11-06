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
                Expression derivative = _argument.GetDerivative(wrt);
                return derivative / _argument;
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

            public override void Map(IMapping mapping)
            {
                mapping.EvaluateLn(_argument);
            }

            public override T Map<T>(IMapping<T> mapping)
            {
                return mapping.EvaluateLn(_argument);
            }

            public override T Map<T>(IExtendedMapping<T> mapping)
            {
                return mapping.EvaluateLn(this, _argument);
            }

            public override T Map<T>(Expression otherExpression, IDualMapping<T> mapping)
            {
                if (otherExpression is Ln other)
                {
                    return mapping.EvaluateLns(this._argument, other._argument);
                }
                return mapping.EvaluateOthers(this, otherExpression);
            }
        }
    }
}