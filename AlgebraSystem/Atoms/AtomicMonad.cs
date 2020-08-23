using System;
using System.Text;

namespace Algebra
{
    namespace Atoms
    {
        internal abstract class AtomicMonad : Expression
        {
            protected readonly IExpression argument;

            protected AtomicMonad(IExpression argument)
            {
                this.argument = argument;
            }

            public abstract Func<IExpression, IExpression> GetSimplifyingConstructor();
            protected abstract int GetHashSeed();
            protected abstract string GetMonadFunctionName();

            protected override sealed int GenHashCode()
            {
                return argument.GetHashCode() ^ GetHashSeed();
            }

            protected override sealed IAtomicExpression GenAtomicExpression()
            {
                IAtomicExpression atomicArg = argument.GetAtomicExpression();
                IExpression expression = GetSimplifyingConstructor()(atomicArg);
                return AtomicExpression.GetAtomicExpression(expression);
            }

            public override sealed string ToString()
            {
                StringBuilder builder = new StringBuilder();

                builder.Append(GetMonadFunctionName());
                builder.Append(" ");
                builder.Append(ToParenthesisedString(this, argument));

                return builder.ToString();
            }

            public override sealed int GetOrderIndex()
            {
                return 0;
            }
        }
    }
}