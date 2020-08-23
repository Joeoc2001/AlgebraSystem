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

            Map = BraceExpansionMap;
            ShouldMapChildren = BraceExpansionShouldMapChildren;
            ShouldMapThis = BraceExpansionShouldMapThis;
        }

        private IExpression BraceExpansionMap(IExpression e)
        {
            if (!(e is Product p))
            {
                return e;
            }

            // Extract all sum terms from the product
            List<Sum> sums = new List<Sum>();
            List<IExpression> others = new List<IExpression>();
            foreach (IExpression arg in p.arguments)
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
                        IExpression newTerm = ExpandSums(s1, s2);
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

        private bool BraceExpansionShouldMapThis(IExpression e) => e is Product && index >= 0;
        private bool BraceExpansionShouldMapChildren(IExpression e) => index >= 0;

        private static IExpression ExpandSums(Sum s1, Sum s2)
        {
            List<IExpression> terms = new List<IExpression>();
            foreach (IExpression t1 in s1.arguments)
            {
                foreach (IExpression t2 in s2.arguments)
                {
                    terms.Add(t1 * t2);
                }
            }
            return Sum.Add(terms);
        }
    }
}
