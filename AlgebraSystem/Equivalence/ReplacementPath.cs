using Algebra.PatternMatching;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Equivalence
{
    public class ReplacementPath : EquivalencePath
    {
        private readonly ReplaceEvaluator _evaluator;

        public ReplacementPath(Expression pattern, Expression replacement)
        {
            _evaluator = new ReplaceEvaluator(pattern, replacement);
        }

        public override IEnumerable<Expression> GetAllFrom(Expression expression)
        {
            return expression.Evaluate(_evaluator);
        }
    }
}
