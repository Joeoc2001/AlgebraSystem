using Rationals;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;


namespace Algebra.Atoms
{
    public class Ln : Monad
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

        public bool Equals(Ln other)
        {
            if (other is null)
            {
                return false;
            }

            return Argument.Equals(other.Argument);
        }

        public override bool Equals(Expression obj)
        {
            return this.Equals(obj as Ln);
        }

        public override int GenHashCode()
        {
            return Argument.GenHashCode() ^ -1043105826;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("ln ");
            builder.Append(ToParenthesisedString(Argument));

            return builder.ToString();
        }

        [Obsolete]
        public override string ToRunnableString()
        {
            return $"Expression.LnOf({Argument.ToRunnableString()})";
        }

        public override int GetOrderIndex()
        {
            return 0;
        }

        public override Func<Expression, Expression> GetSimplifyingConstructor()
        {
            return LnOf;
        }
    }
}