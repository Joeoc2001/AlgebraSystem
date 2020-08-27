using Rationals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Evaluators
{
    public class GetOrderingDualEvaluator : IDualEvaluator<int>, IComparer<IExpression>
    {
        public static readonly GetOrderingDualEvaluator Instance = new GetOrderingDualEvaluator();

        protected GetOrderingDualEvaluator()
        {

        }

        public int Compare(IExpression x, IExpression y)
        {
            return x.Evaluate(y, this);
        }

        private int CompareCommutative(ICollection<IExpression> a, ICollection<IExpression> b)
        {
            // Compare based on lowest component of each, break ties on later terms
            List<IExpression> aSorted = new List<IExpression>(a);
            List<IExpression> bSorted = new List<IExpression>(b);
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

        public int EvaluateArcsins(IExpression argument1, IExpression argument2)
        {
            return argument1.Evaluate(argument2, this);
        }

        public int EvaluateArctans(IExpression argument1, IExpression argument2)
        {
            return argument1.Evaluate(argument2, this);
        }

        public int EvaluateConstants(Rational value1, Rational value2)
        {
            return value1.CompareTo(value2);
        }

        public int EvaluateExponents(IExpression baseArgument1, IExpression powerArgument1, IExpression baseArgument2, IExpression powerArgument2)
        {
            int comp1 = baseArgument1.Evaluate(baseArgument2, this);
            if (comp1 != 0)
            {
                return comp1;
            }
            return powerArgument1.Evaluate(powerArgument2, this);
        }

        public int EvaluateFunctions(IFunction function1, IFunction function2)
        {
            // Sort by name first, then each parameter
            IFunctionIdentity aId = function1.GetIdentity();
            IFunctionIdentity bId = function2.GetIdentity();
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
            IDictionary<string, IExpression> aParams = function1.GetParameters();
            IDictionary<string, IExpression> bParams = function2.GetParameters();
            foreach (string parameterName in aReq)
            {
                IExpression aExp = aParams[parameterName];
                IExpression bExp = bParams[parameterName];
                int cmp3 = Compare(aExp, bExp);
                if (cmp3 != 0)
                {
                    return cmp3;
                }
            }

            // Else functions are identical
            return 0;
        }

        public int EvaluateLns(IExpression argument1, IExpression argument2)
        {
            return argument1.Evaluate(argument2, this);
        }

        public int EvaluateOthers(IExpression expression1, IExpression expression2)
        {
            int a = expression1.Evaluate(RankEvaluator.Instance);
            int b = expression1.Evaluate(RankEvaluator.Instance);
            return a.CompareTo(b);
        }

        public int EvaluateProducts(ICollection<IExpression> arguments1, ICollection<IExpression> arguments2)
        {
            return CompareCommutative(arguments1, arguments2);
        }

        public int EvaluateSigns(IExpression argument1, IExpression argument2)
        {
            return argument1.Evaluate(argument2, this);
        }

        public int EvaluateSins(IExpression argument1, IExpression argument2)
        {
            return argument1.Evaluate(argument2, this);
        }

        public int EvaluateSums(ICollection<IExpression> arguments1, ICollection<IExpression> arguments2)
        {
            return CompareCommutative(arguments1, arguments2);
        }

        public int EvaluateVariables(string name1, string name2)
        {
            return name1.CompareTo(name2);
        }
    }
}
