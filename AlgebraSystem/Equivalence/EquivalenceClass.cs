using Algebra.Metrics;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Equivalence
{
    public class EquivalenceClass
    {
        private readonly Expression _anchorExpression; // The expression used to define the equivalence class

        public EquivalenceClass(Expression anchorExpression)
        {
            this._anchorExpression = anchorExpression;
        }

        /// <summary>
        /// Checks if an equation is within this Equivalence Class using a BFS.
        /// This method is VERY SLOW and should not be used unless proven equality is absolutely needed.
        /// </summary>
        /// <param name="queryExpression">The equation to check if present</param>
        /// <param name="attempts">The number of alternate forms to check. Default is everything</param>
        /// <param name="equivalencies">The equivalencies to use to generate equivalent forms of the anchor expression</param>
        /// <param name="metric">The metric to use to evaluate how close an alternate form is to the query expression. Higher => closer. Default = shared subtrees</param>
        /// <returns>True if it can be proven that these expressions are equal, false otherwise</returns>
        public bool IsInClass(Expression queryExpression, ulong attempts = ulong.MaxValue, List<EquivalencePath> equivalencies = null, IExpressionMetric metric = null)
        {
            // Check trivial case
            if (_anchorExpression.Equals(queryExpression, EqualityLevel.Atomic))
            {
                return true;
            }

            equivalencies = equivalencies ?? EquivalencePaths.DefaultPaths;
            metric = metric ?? new SharedSubtreesMetric(queryExpression);

            // Which expressions are not equivalent and have been checked
            HashSet<Expression> checkedExpressions = new HashSet<Expression>() { _anchorExpression };
            // Which equations still have paths from to be explored
            SimplePriorityQueue<Expression, double> frontierExpressions = new SimplePriorityQueue<Expression, double>();
            frontierExpressions.Enqueue(_anchorExpression, 0); // Initial priority doesn't matter as it will always be checked first

            ulong cycles = 0; // Keep track of how many times we have tried already
            while (frontierExpressions.Count != 0 && cycles != attempts)
            {
                Expression frontierExpression = frontierExpressions.Dequeue();

                // Loop over all paths and apply them to all frontier expressions
                foreach (EquivalencePath equivalency in equivalencies)
                {
                    foreach (Expression newExpression in equivalency.GetAllFrom(frontierExpression))
                    {
                        if (checkedExpressions.Contains(newExpression))
                        {
                            continue; // Don't need to recheck
                        }

                        // Check for equality
                        if (newExpression.Equals(queryExpression, EqualityLevel.Atomic))
                        {
                            return true;
                        }

                        // ASSERT: Equations aren't equal
                        // Add to data structures
                        frontierExpressions.Enqueue(newExpression, -metric.Calculate(newExpression)); // Use -metric as smaller is dequeued first
                        checkedExpressions.Add(newExpression);
                    }
                }

                cycles++;
            }

            return false; // No equality was found
        }
    }
}
