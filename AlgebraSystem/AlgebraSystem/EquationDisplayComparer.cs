﻿using Algebra.Operations;
using System;
using System.CodeDom;
using System.Collections.Generic;


namespace Algebra
{
    public class EquationDisplayComparer : IComparer<Expression>
    {
        public static readonly EquationDisplayComparer COMPARER = new EquationDisplayComparer();

        private EquationDisplayComparer()
        {

        }

        public static int GetEquationOrdering(Expression e)
        {
            switch (e)
            {
                case Sum _:
                    return 7;
                case Product _:
                    return 6;
                case Constant _:
                    return 5;
                case Variable _:
                    return 4;
                case Exponent _:
                    return 3;
                case Ln _:
                    return 2;
                case Sign _:
                    return 1;
                case Sin _:
                    return 0;
                default:
                    throw new NotImplementedException($"Unsuported Expression type {e.GetType()}");
            };
        }

        public int Compare(Expression x, Expression y)
        {
            int cmp = GetEquationOrdering(x).CompareTo(GetEquationOrdering(y));

            if (cmp != 0)
            {
                return cmp;
            }

            switch (x)
            {
                case Constant c:
                    return CompareConstants(c, (Constant)y);
                case Variable c:
                    return CompareVariables(c, (Variable)y);
                case Sum c:
                    return CompareCommutative(c, (Sum)y);
                case Product c:
                    return CompareCommutative(c, (Product)y);
                case Exponent c:
                    return CompareExponents(c, (Exponent)y);
                case Ln c:
                    return CompareMonad(c, (Ln)y);
                case Sign c:
                    return CompareMonad(c, (Sign)y);
                case Sin c:
                    return CompareMonad(c, (Sin)y);
                default:
                    throw new NotImplementedException($"Unsuported Expression type {x.GetType()}");
            };
        }

        private int CompareConstants(Constant a, Constant b)
        {
            // Bigger numbers are more important and should come first,
            // hence the -
            return -a.GetValue().CompareTo(b.GetValue());
        }

        private int CompareVariables(Variable a, Variable b)
        {
            return a.Name.CompareTo(b.Name);
        }

        private int CompareCommutative(CommutativeOperation a, CommutativeOperation b)
        {
            // Compare based on lowest component of each, break ties on later terms
            List<Expression> aSorted = a.GetDisplaySortedArguments();
            List<Expression> bSorted = b.GetDisplaySortedArguments();

            int i = 0;
            while (i < aSorted.Count && i < bSorted.Count)
            {
                int cmp = Compare(aSorted[i], bSorted[i]);
                if (cmp != 0)
                {
                    return cmp;
                }
                i++;
            }

            return aSorted.Count.CompareTo(bSorted.Count);
        }

        private int CompareExponents(Exponent a, Exponent b)
        {
            // Sort by base first, then exponent
            int cmp1 = Compare(a.Base, b.Base);
            if (cmp1 != 0)
            {
                return cmp1;
            }
            return Compare(a.Power, b.Power);
        }

        private int CompareMonad(Monad a, Monad b)
        {
            return Compare(a.Argument, b.Argument);
        }
    }
}