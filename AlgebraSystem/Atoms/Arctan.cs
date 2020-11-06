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
            new public static Expression ArctanOf(Expression argument)
            {
                return new Arctan(argument);
            }

            public Arctan(Expression argument)
                : base(argument)
            {

            }

            public override Expression GetDerivative(string wrt)
            {
                Expression derivative = _argument.GetDerivative(wrt);
                return derivative / (1 + Pow(_argument, ConstantFrom(2)));
            }

            public override Func<Expression, Expression> GetSimplifyingConstructor()
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

            public override void Map(IMapping mapping)
            {
                mapping.EvaluateArctan(_argument);
            }

            public override T Map<T>(IMapping<T> mapping)
            {
                return mapping.EvaluateArctan(_argument);
            }

            public override T Map<T>(IExtendedMapping<T> mapping)
            {
                return mapping.EvaluateArctan(this, _argument);
            }

            public override T Map<T>(Expression otherExpression, IDualMapping<T> mapping)
            {
                if (otherExpression is Arctan other)
                {
                    return mapping.EvaluateArctans(this._argument, other._argument);
                }
                return mapping.EvaluateOthers(this, otherExpression);
            }
        }
    }
}