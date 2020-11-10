using Algebra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public class ExpressionComparer : IComparer<Expression>
    {
        public static readonly ExpressionComparer Instance = new ExpressionComparer();

        private ExpressionComparer()
        {

        }

        public int Compare(Expression x, Expression y)
        {
            return x.Map(y, OrderingDualMapping.Instance);
        }
    }
}
