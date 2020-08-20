using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Equivalence
{
    internal class BraceExpansionMapping : ExpressionMapping
    {
        private int index;

        public BraceExpansionMapping(int i)
        {
            this.index = i;

            Map = map;
            ShouldMapChildren = shouldMapChildren;
            ShouldMapThis = shouldMapThis;
        }

        private Expression map(Expression e)
        {
            if (!(e is Product p))
            {
                return e;
            }

            // Extract all sum terms from the product
            List<Sum> sums = new List<Sum>();
            List<Expression> others = new List<Expression>();
            foreach (Expression arg in p.Arguments)
            {
                if (arg is Sum s)
                {
                    sums.Add(s);
                }
                else
                {
                    others.Add(arg);
                }
            }

            // Loop through pairs to find where i is 0
            for (int j = 0; j < sums.Count - 1; j++)
            {
                for (int k = j + 1; k < sums.Count; k++)
                {
                    if (index-- == 0)
                    {
                        // Extract the two sum terms
                        Sum s1 = sums[j];
                        Sum s2 = sums[k];
                        sums.RemoveAt(k);
                        sums.RemoveAt(j);

                        // Multiply them
                        Expression newTerm = ExpandSums(s1, s2);
                        others.Add(newTerm);

                        // Create a new product
                        others.AddRange(sums);
                        return Product.Multiply(others);
                    }
                }
            }

            // If i didn't hit 0 then the pair isn't in this multiplication
            return e;
        }

        private bool shouldMapThis(Expression e) => e is Product && index >= 0;
        private bool shouldMapChildren(Expression e) => index >= 0;

        private static Expression ExpandSums(Sum s1, Sum s2)
        {
            List<Expression> terms = new List<Expression>();
            foreach (Expression t1 in s1.Arguments)
            {
                foreach (Expression t2 in s2.Arguments)
                {
                    terms.Add(t1 * t2);
                }
            }
            return Sum.Add(terms);
        }
    }
}
