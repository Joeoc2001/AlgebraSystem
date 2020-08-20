using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Algebra.Atoms
{
    public class Sign : AtomicMonad
    {
        new public static Expression SignOf(Expression argument)
        {
            if (argument is Sign)
            {
                return argument;
            }

            if (argument is Constant constant)
            {
                if (constant.GetValue().IsZero)
                {
                    return 0;
                }
                if (constant.GetValue() > 0)
                {
                    return 1;
                }
                if (constant.GetValue() < 0)
                {
                    return -1;
                }
            }

            return new Sign(argument);
        }

        private Sign(Expression argument)
            : base(argument)
        {

        }

        public override Expression GetDerivative(Variable wrt)
        {
            return 0; // Not always true, but true 100% of the time :P
        }

        public override ExpressionDelegate GetDelegate(VariableInputSet set)
        {
            ExpressionDelegate eqExpression = Argument.GetDelegate(set);
            return () => Math.Sign(eqExpression());
        }

        protected override bool ExactlyEquals(Expression expression)
        {
            if (!(expression is Sign sign))
            {
                return false;
            }

            return Argument.Equals(sign.Argument);
        }

        public override Func<Expression, Expression> GetSimplifyingConstructor()
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
    }
}