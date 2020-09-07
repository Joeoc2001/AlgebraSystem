using System.Collections.Generic;

namespace Algebra.Equivalence
{
    public abstract class EquivalencePath
    {
        public abstract IEnumerable<Expression> GetAllFrom(Expression expression);
        public IEnumerable<Expression> GetAllFrom(IEnumerable<Expression> expressions)
        {
            foreach (Expression expression in expressions)
            {
                foreach (Expression result in GetAllFrom(expression))
                {
                    yield return result;
                }
            }
        }
    }
}