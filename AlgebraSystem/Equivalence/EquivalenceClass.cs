using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Equivalence
{
    internal class EquivalenceClass : IEquivalenceClass
    {
        public static readonly List<EquivalencePath> DEFAULT_PATHS = new List<EquivalencePath> {
            EquivalencePaths.EXPAND_BRACES
        };

        private readonly IExpression anchorExpression; // The expression used to define the equivalence class

        public EquivalenceClass(IExpression anchorExpression)
        {
            this.anchorExpression = anchorExpression;
        }

        /// <summary>
        /// Checks if an equation is within this Equivalence Class using a BFS.
        /// This method is VERY SLOW and should not be used unless proven equality is absolutely needed.
        /// </summary>
        /// <param name="queryExpression">The equation to check if present</param>
        /// <param name="searchDepth">The depth to search to. Default is search everywhere</param>
        /// <returns>True if it can be proven that these expressions are equal, false otherwise</returns>
        public bool IsInClass(IExpression queryExpression, int searchDepth = -1, List<EquivalencePath> paths = null)
        {
            // Check trivial case
            if (anchorExpression.Equals(queryExpression, EqualityLevel.Atomic))
            {
                return true;
            }

            paths = paths ?? DEFAULT_PATHS;

            // Which expressions are not equivalent and have been checked
            HashSet<IExpression> checkedExpressions = new HashSet<IExpression>() { anchorExpression };
            // Which equations still have paths from to be explored
            HashSet<IExpression> frontierExpressions = new HashSet<IExpression>(checkedExpressions);

            int cycles = 0; // Keep track of how many times we have tried already
            while (frontierExpressions.Count != 0 && cycles != searchDepth)
            {
                HashSet<IExpression> newFrontier = new HashSet<IExpression>();

                // Loop over all paths and apply them to all frontier expressions
                foreach (IExpression frontier in frontierExpressions)
                {
                    foreach (EquivalencePath path in paths)
                    {
                        List<IExpression> newExpressions = path(frontier);
                        foreach (IExpression newExpression in newExpressions)
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
                            newFrontier.Add(newExpression);
                            checkedExpressions.Add(newExpression);
                        }
                    }
                }

                frontierExpressions = newFrontier;
                cycles++;
            }

            return false; // No equality was found
        }
    }
}
