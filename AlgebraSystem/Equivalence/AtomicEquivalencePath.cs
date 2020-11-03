using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Equivalence
{
    public class AtomicEquivalencePath : EquivalencePath
    {
        public static readonly AtomicEquivalencePath Instance = new AtomicEquivalencePath();

        private AtomicEquivalencePath()
        {

        }

        public override IEnumerable<Expression> GetAllFrom(Expression expression)
        {
            yield return expression.GetAtomicExpression();
        }
    }
}
