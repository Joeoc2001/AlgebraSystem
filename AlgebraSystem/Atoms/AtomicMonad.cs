﻿using System;
using System.Text;

namespace Algebra
{
    namespace Atoms
    {
        internal abstract class AtomicMonad : Expression
        {
            protected readonly Expression _argument;

            protected AtomicMonad(Expression argument)
            {
                this._argument = argument ?? throw new ArgumentNullException(nameof(argument));
            }

            public abstract Func<Expression, Expression> GetSimplifyingConstructor();
            protected abstract int GetHashSeed();
            protected abstract string GetMonadFunctionName();

            protected override sealed int GenHashCode()
            {
                return _argument.GetHashCode() ^ GetHashSeed();
            }

            protected override Expression GenAtomicExpression()
            {
                Expression atomicArg = _argument.GetAtomicExpression();
                return GetSimplifyingConstructor()(atomicArg);
            }

            public override sealed string ToString()
            {
                StringBuilder builder = new StringBuilder();

                builder.Append(GetMonadFunctionName());
                builder.Append(" ");
                builder.Append(ToParenthesisedString(this, _argument));

                return builder.ToString();
            }

            public override sealed int GetOrderIndex()
            {
                return 0;
            }
        }
    }
}