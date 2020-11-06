using Algebra.PatternMatching;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Equivalence
{
    public class ReplacementPath : EquivalencePath
    {
        private readonly ReplaceMapping _mapping;

        public ReplacementPath(Expression pattern, Expression replacement)
        {
            _mapping = new ReplaceMapping(pattern, replacement);
        }

        public override IEnumerable<Expression> GetAllFrom(Expression expression)
        {
            return expression.Map(_mapping);
        }
    }
}
