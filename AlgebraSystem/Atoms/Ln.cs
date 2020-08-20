using Rationals;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;


namespace Algebra.Atoms
{
    public class Ln : AtomicMonad
    {
        new public static Expression LnOf(Expression argument)
        {
            return new Ln(argument);
        }

        public Ln(Expression argument)
            : base(argument)
        {

        }

        public override ExpressionDelegate GetDelegate(VariableInputSet set)
        {
            if (Argument is Constant constant)
            {
                float value = (float)Rational.Log(constant.GetValue());
                return () => value;
            }

            ExpressionDelegate expression = Argument.GetDelegate(set);

            // TODO: This can be better
            return () => (float)Math.Log(expression());
        }

        public override Expression GetDerivative(Variable wrt)
        {
            Expression derivative = Argument.GetDerivative(wrt);
            return derivative / Argument;
        }

        protected override bool ExactlyEquals(Expression expression)
        {
            if (!(expression is Ln ln))
            {
                return false;
            }

            return Argument.Equals(ln.Argument);
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
    }
}