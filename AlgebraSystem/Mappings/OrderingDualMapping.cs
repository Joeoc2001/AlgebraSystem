using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Mappings
{
    public class OrderingDualMapping : IDualMapping<int>, IComparer<Expression>
    {
        public static readonly OrderingDualMapping Instance = new OrderingDualMapping();

        protected OrderingDualMapping()
        {

        }

        public int Compare(Expression x, Expression y)
        {
            return x.Map(y, this);
        }

        private int CompareCommutative(ICollection<Expression> a, ICollection<Expression> b)
        {
            // Compare based on lowest component of each, break ties on later terms
            List<Expression> aSorted = new List<Expression>(a);
            List<Expression> bSorted = new List<Expression>(b);
            aSorted.Sort(this);
            bSorted.Sort(this);

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

            return a.Count.CompareTo(b.Count);
        }

        public int EvaluateArcsins(Expression argument1, Expression argument2)
        {
            return argument1.Map(argument2, this);
        }

        public int EvaluateArctans(Expression argument1, Expression argument2)
        {
            return argument1.Map(argument2, this);
        }

        public int EvaluateConstants(IConstant value1, IConstant value2)
        {
            return value1.CompareTo(value2);
        }

        public int EvaluateExponents(Expression baseArgument1, Expression powerArgument1, Expression baseArgument2, Expression powerArgument2)
        {
            int comp1 = baseArgument1.Map(baseArgument2, this);
            if (comp1 != 0)
            {
                return comp1;
            }
            return powerArgument1.Map(powerArgument2, this);
        }

        public int EvaluateFunctions(Function function1, Function function2)
        {
            // Sort by name first, then each parameter
            FunctionIdentity aId = function1.GetIdentity();
            FunctionIdentity bId = function2.GetIdentity();
            ReadOnlyCollection<string> aReq = aId.GetRequiredParameters();
            ReadOnlyCollection<string> bReq = bId.GetRequiredParameters();
            if (aId != bId)
            {
                // Less parameters => comes first
                int aLen = aReq.Count;
                int bLen = bReq.Count;
                int cmp1 = aLen.CompareTo(bLen);
                if (cmp1 != 0)
                {
                    return cmp1;
                }

                // Second sort on name
                int cmp2 = aId.GetName().CompareTo(bId.GetName());
                if (cmp2 != 0)
                {
                    return cmp2;
                }
            }

            // ASSERT: Function types are the same
            // Therefore parameter keys are the same
            // Sort on parameter values
            IDictionary<string, Expression> aParams = function1.GetParameters();
            IDictionary<string, Expression> bParams = function2.GetParameters();
            foreach (string parameterName in aReq)
            {
                Expression aExp = aParams[parameterName];
                Expression bExp = bParams[parameterName];
                int cmp3 = Compare(aExp, bExp);
                if (cmp3 != 0)
                {
                    return cmp3;
                }
            }

            // Else functions are identical
            return 0;
        }

        public int EvaluateLns(Expression argument1, Expression argument2)
        {
            return argument1.Map(argument2, this);
        }

        public int EvaluateOthers(Expression expression1, Expression expression2)
        {
            int a = expression1.Map(RankMapping.Instance);
            int b = expression2.Map(RankMapping.Instance);
            return a.CompareTo(b);
        }

        public int EvaluateProducts(ICollection<Expression> arguments1, ICollection<Expression> arguments2)
        {
            return CompareCommutative(arguments1, arguments2);
        }

        public int EvaluateSigns(Expression argument1, Expression argument2)
        {
            return argument1.Map(argument2, this);
        }

        public int EvaluateSins(Expression argument1, Expression argument2)
        {
            return argument1.Map(argument2, this);
        }

        public int EvaluateSums(ICollection<Expression> arguments1, ICollection<Expression> arguments2)
        {
            return CompareCommutative(arguments1, arguments2);
        }

        public int EvaluateVariables(IVariable value1, IVariable value2)
        {
            return value1.GetName().CompareTo(value2.GetName());
        }
    }
}
