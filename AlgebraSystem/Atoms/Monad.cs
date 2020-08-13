using System;


namespace Algebra.Atoms
{
    public abstract class Monad : Expression
    {
        public readonly Expression Argument;

        protected Monad(Expression argument)
        {
            this.Argument = argument;
        }
        public abstract Func<Expression, Expression> GetSimplifyingConstructor();

        public override Expression Map(ExpressionMapping map)
        {
            Expression currentThis = this;

            if (map.ShouldMapChildren(this))
            {
                Expression mappedArg = Argument.Map(map);
                currentThis = GetSimplifyingConstructor()(mappedArg);
            }

            if (map.ShouldMapThis(this))
            {
                currentThis = map.PostMap(currentThis);
            }

            return currentThis;
        }
    }
}