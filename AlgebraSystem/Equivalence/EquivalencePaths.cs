using Algebra.Atoms;
using System.Collections.Generic;

namespace Algebra.Equivalence
{
    public static class EquivalencePaths
    {
        public static readonly EquivalencePath EXPAND_BRACES = eq =>
        {
            int i = 0;
            List<Expression> newEqs = new List<Expression>();
            while (true)
            {
                ExpressionMapping expansionMapping = new BraceExpansionMapping(i);

                Expression newEq = eq.PostMap(expansionMapping);

                if (newEq.Equals(eq))
                {
                    break;
                }

                newEqs.Add(newEq);
                i++;
            }
            return newEqs;
        };
    }
}