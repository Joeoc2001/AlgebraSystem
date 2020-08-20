using System;
using System.Text;

namespace Algebra.Atoms
{
    public abstract class AtomicMonad : Expression
    {
        public readonly Expression Argument;

        protected AtomicMonad(Expression argument)
        {
            this.Argument = argument;
        }

        public abstract Func<Expression, Expression> GetSimplifyingConstructor();
        protected abstract int GetHashSeed();
        protected abstract string GetMonadFunctionName();

        protected override sealed int GenHashCode()
        {
            return Argument.GetHashCode() ^ GetHashSeed();
        }

        protected override sealed Expression GenAtomicExpression()
        {
            Expression atomicArg = Argument.GetAtomicExpression();
            return GetSimplifyingConstructor()(atomicArg);
        }

        public override sealed string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(GetMonadFunctionName());
            builder.Append(" ");
            builder.Append(ToParenthesisedString(Argument));

            return builder.ToString();
        }

        public override sealed int GetOrderIndex()
        {
            return 0;
        }

        public override sealed Expression MapChildren(ExpressionMapping.ExpressionMap map)
        {
            Expression mappedArg = map(Argument);
            return GetSimplifyingConstructor()(mappedArg);
        }
    }
}