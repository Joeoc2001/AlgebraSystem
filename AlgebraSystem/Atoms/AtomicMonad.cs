using System;
using System.Text;

namespace Algebra
{
    namespace Atoms
    {
        internal abstract class AtomicMonad : Expression
        {
            protected readonly IExpression _argument;

            protected AtomicMonad(IExpression argument)
            {
                this._argument = argument;
            }

            public abstract Func<IExpression, IExpression> GetSimplifyingConstructor();
            protected abstract int GetHashSeed();
            protected abstract string GetMonadFunctionName();

            protected override sealed int GenHashCode()
            {
                return _argument.GetHashCode() ^ GetHashSeed();
            }

            protected override IExpression GenAtomicExpression()
            {
                IExpression atomicArg = _argument.GetAtomicExpression();
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