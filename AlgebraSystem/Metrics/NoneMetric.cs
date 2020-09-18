using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Metrics
{
    public class NoneMetric : IExpressionMetric
    {
        public static NoneMetric Instance = new NoneMetric();

        private NoneMetric()
        {

        }

        public double Calculate(Expression expression)
        {
            return 0.5;
        }
    }
}
