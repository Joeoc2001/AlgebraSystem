using Algebra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Metrics
{
    public class SharedSubtreesMetric : SimilarityMetric
    {
        private readonly HashSet<Expression> _expressionSubtrees;

        private static HashSet<Expression> GetSubtrees(Expression expression)
        {
            return expression.Map(GetAllSubtreesMapping.Instance);
        }

        public SharedSubtreesMetric(Expression expression) : base(expression)
        {
            _expressionSubtrees = GetSubtrees(expression);
        }

        public override double Calculate(Expression other)
        {
            HashSet<Expression> otherSubtrees = GetSubtrees(other);

            // Return number of subtrees in common / total number of possible common subtrees
            int maxCount = Math.Max(_expressionSubtrees.Count, otherSubtrees.Count);
            otherSubtrees.IntersectWith(_expressionSubtrees);
            return (double)otherSubtrees.Count / maxCount;
        }
    }
}
