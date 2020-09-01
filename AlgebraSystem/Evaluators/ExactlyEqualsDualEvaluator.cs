using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public class ExactlyEqualsDualEvaluator : IDualEvaluator<bool>
    {
        public static readonly ExactlyEqualsDualEvaluator Instance = new ExactlyEqualsDualEvaluator();

        private ExactlyEqualsDualEvaluator()
        {

        }

        public bool EvaluateArcsins(IExpression argument1, IExpression argument2)
        {
            return argument1.Evaluate(argument2, this);
        }

        public bool EvaluateArctans(IExpression argument1, IExpression argument2)
        {
            return argument1.Evaluate(argument2, this);
        }

        public bool EvaluateConstants(Rational value1, Rational value2)
        {
            return value1.Equals(value2);
        }

        public bool EvaluateExponents(IExpression baseArgument1, IExpression powerArgument1, IExpression baseArgument2, IExpression powerArgument2)
        {
            return baseArgument1.Evaluate(baseArgument2, this) && powerArgument1.Evaluate(powerArgument2, this);
        }

        public bool EvaluateFunctions(IFunction function1, IFunction function2)
        {
            if (!function1.GetIdentity().Equals(function2.GetIdentity()))
            {
                return false;
            }

            IDictionary<string, IExpression> p1 = function1.GetParameters();
            IDictionary<string, IExpression> p2 = function2.GetParameters();

            if (p1.Count != p2.Count)
            {
                return false;
            }

            foreach (string parameterName in p1.Keys)
            {
                if (!p2.TryGetValue(parameterName, out IExpression expression2))
                {
                    return false; // The parameter with given name doesn't exist in p2
                }
                IExpression expression1 = p1[parameterName];
                if (!expression1.Equals(expression2, EqualityLevel.Exactly))
                {
                    return false;
                }
            }

            return true;
        }

        public bool EvaluateLns(IExpression argument1, IExpression argument2)
        {
            return argument1.Evaluate(argument2, this);
        }

        public bool EvaluateOthers(IExpression expression1, IExpression expression2)
        {
            return false;
        }

        protected bool CommutativeExactlyEquals(ICollection<IExpression> args1, ICollection<IExpression> args2)
        {
            // Check for commutativity
            // Add all parameters to dict by hash
            Dictionary<int, List<IExpression>> expressionsByHashes = new Dictionary<int, List<IExpression>>();
            foreach (IExpression arg2 in args2)
            {
                int hash = arg2.GetHashCode();
                if (!expressionsByHashes.TryGetValue(hash, out List<IExpression> expressions))
                {
                    expressions = new List<IExpression>();
                    expressionsByHashes.Add(hash, expressions);
                }
                expressions.Add(arg2);
            }

            // Check all parameters in this are present
            foreach (IExpression arg1 in args1)
            {
                int hash = arg1.GetHashCode();
                if (!expressionsByHashes.TryGetValue(hash, out List<IExpression> expressions))
                {
                    return false;
                }

                // Perform linear search on all equations with same hash
                bool found = false;
                foreach (IExpression otherArg in expressions)
                {
                    if (otherArg.Equals(arg1, EqualityLevel.Exactly))
                    {
                        found = true;
                        expressions.Remove(otherArg);
                        break;
                    }
                }

                // If linear search failed then args are different
                if (!found)
                {
                    return false;
                }
            }

            return true;
        }
        public bool EvaluateProducts(ICollection<IExpression> arguments1, ICollection<IExpression> arguments2)
        {
            return CommutativeExactlyEquals(arguments1, arguments2);
        }

        public bool EvaluateSigns(IExpression argument1, IExpression argument2)
        {
            return argument1.Evaluate(argument2, this);
        }

        public bool EvaluateSins(IExpression argument1, IExpression argument2)
        {
            return argument1.Evaluate(argument2, this);
        }

        public bool EvaluateSums(ICollection<IExpression> arguments1, ICollection<IExpression> arguments2)
        {
            return CommutativeExactlyEquals(arguments1, arguments2);
        }

        public bool EvaluateVariables(string name1, string name2)
        {
            return string.Equals(name1, name2);
        }
    }
}
