using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public abstract class SimilarityMetric : IExpressionMetric
    {
        public readonly Expression Expression;

        public SimilarityMetric(Expression expression)
        {
            this.Expression = expression;
        }

        public abstract double Calculate(Expression other);
    }
}
