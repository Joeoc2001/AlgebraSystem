using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IExpressionMetric
    {
        double Calculate(Expression expression);
    }
}
