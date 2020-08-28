using Algebra.PatternMatching;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Equivalence
{
    public class ReplacementPath : EquivalencePath
    {
        private readonly ReplaceEvaluator _evaluator;

        public ReplacementPath(IExpression pattern, IExpression replacement)
        {
            _evaluator = new ReplaceEvaluator(pattern, replacement);
        }

        public override IEnumerable<IExpression> GetAllFrom(IExpression expression)
        {
            return expression.Evaluate(_evaluator);
        }
    }
}
