using Algebra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Libs
{
    class StandardExpressions : IEnumerable<Expression>
    {
        public static readonly Expression[] SingleExpressions = new Expression[]
        {
            1,
            Expression.E,
            Expression.VarA,
            Expression.VarX,
        };

        public static IEnumerable<Expression> GetMonads(IEnumerable<Expression> expressions)
        {
            foreach (Expression expression in expressions)
            {
                yield return expression;
                foreach (MonadBuilder.Monad monad in Enum.GetValues(typeof(MonadBuilder.Monad)))
                {
                    yield return MonadBuilder.Build(expression, monad);
                }
            }
        }

        public static IEnumerable<Expression> GetDyads(IEnumerable<Expression> expressions)
        {
            foreach (Expression expression1 in expressions)
            {
                yield return expression1;
                foreach (Expression expression2 in expressions)
                {
                    foreach (DyadBuilder.Dyad dyad in Enum.GetValues(typeof(DyadBuilder.Dyad)))
                    {
                        yield return DyadBuilder.Build(expression1, expression2, dyad);
                    }
                }
            }
        }

        public static IEnumerable<Expression> GetSelection()
        {
            foreach (var item in GetDyads(SingleExpressions))
            {
                yield return item;
            }
            foreach (var item in GetMonads(GetDyads(SingleExpressions)))
            {
                yield return item;
            }
        }

        public IEnumerator<Expression> GetEnumerator()
        {
            return GetSelection().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetSelection().GetEnumerator();
        }
    }
}
