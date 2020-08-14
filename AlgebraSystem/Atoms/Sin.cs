﻿using Rationals;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;


namespace Algebra.Atoms
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

        protected override bool ExactlyEquals(Expression expression)
        {
            if (!(expression is Sin sin))
            {
                return false;
            }

            return Argument.Equals(sin.Argument);
        }

        protected override int GenHashCode()
        {
            return Argument.GetHashCode() ^ -1010034057;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("sin ");
            builder.Append(ToParenthesisedString(Argument));

            return builder.ToString();
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