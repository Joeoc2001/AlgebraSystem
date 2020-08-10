using Rationals;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;


namespace Algebra.Operations
{
    public class Sin : Monad
    {
        new public static Expression SinOf(Expression argument)
        {
            return new Sin(argument);
        }

        public Sin(Expression argument)
            : base(argument)
        {

        }

        public override ExpressionDelegate GetDelegate(VariableInputSet set)
        {
            ExpressionDelegate expression = Argument.GetDelegate(set);

            // TODO: This can be better
            return () => (float)Math.Sin(expression());
        }

        public override Expression GetDerivative(Variable wrt)
        {
            Expression derivative = Argument.GetDerivative(wrt);
            return derivative * CosOf(Argument);
        }

        public bool Equals(Sin other)
        {
            if (other is null)
            {
                return false;
            }

            return Argument.Equals(other.Argument);
        }

        public override bool Equals(Expression obj)
        {
            return this.Equals(obj as Sin);
        }

        public override int GenHashCode()
        {
            return Argument.GenHashCode() ^ -1010034057;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("sin ");
            builder.Append(ToParenthesisedString(Argument));

            return builder.ToString();
        }

        [Obsolete]
        public override string ToRunnableString()
        {
            return $"Expression.SinOf({Argument.ToRunnableString()})";
        }

        public override int GetOrderIndex()
        {
            return 0;
        }

        public override Func<Expression, Expression> GetSimplifyingConstructor()
        {
            return SinOf;
        }
    }
}