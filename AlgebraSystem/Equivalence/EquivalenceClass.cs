﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Equivalence
{
    public class EquivalenceClass
    {
        public static readonly List<EquivalencePath> DEFAULT_PATHS = new List<EquivalencePath> {
            EquivalencePaths.EXPAND_BRACES
        };

        private readonly Expression anchorExpression; // The expression used to define the equivalence class

        public EquivalenceClass(Expression anchorExpression)
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
        public bool IsInClass(Expression queryExpression, int searchDepth = -1, List<EquivalencePath> paths = null)
        {
            // Check trivial case
            if (anchorExpression.Equals(queryExpression, EqualityLevel.Atomic))
            {
                return true;
            }

            paths = paths ?? DEFAULT_PATHS;

            // Which expressions are not equivalent and have been checked
            HashSet<Expression> checkedExpressions = new HashSet<Expression>() { anchorExpression };
            // Which equations still have paths from to be explored
            HashSet<Expression> frontierExpressions = new HashSet<Expression>(checkedExpressions);

            int cycles = 0; // Keep track of how many times we have tried already
            while (frontierExpressions.Count != 0 && cycles != searchDepth)
            {
                HashSet<Expression> newFrontier = new HashSet<Expression>();

                // Loop over all paths and apply them to all frontier expressions
                foreach (Expression frontier in frontierExpressions)
                {
                    foreach (EquivalencePath path in paths)
                    {
                        List<Expression> newExpressions = path(frontier);
                        foreach (Expression newExpression in newExpressions)
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
