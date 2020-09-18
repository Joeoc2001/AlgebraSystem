using System.Collections.Generic;

namespace Algebra.Equivalence
{
    public abstract class EquivalencePath
    {
        public abstract IEnumerable<Expression> GetAllFrom(Expression expression);
    }
}