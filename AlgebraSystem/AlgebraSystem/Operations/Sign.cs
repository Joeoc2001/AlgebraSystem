using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Algebra.Operations
{
    public class Sign : Monad
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

        public bool Equals(Sign other)
        {
            if (other is null)
            {
                return false;
            }

            return Argument.Equals(other.Argument);
        }

        public override bool Equals(Expression obj)
        {
            return this.Equals(obj as Sign);
        }

        public override int GenHashCode()
        {
            return Argument.GenHashCode() ^ -322660314;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("sign ");
            builder.Append(ToParenthesisedString(Argument));

            return builder.ToString();
        }

        public override string ToRunnableString()
        {
            return $"Expression.SignOf({Argument.ToRunnableString()})";
        }

        public override int GetOrderIndex()
        {
            return 0;
        }

        public override Func<Expression, Expression> GetSimplifyingConstructor()
        {
            return SignOf;
        }
    }
}