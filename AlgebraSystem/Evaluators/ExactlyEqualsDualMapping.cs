using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.mappings
{
    public class ExactlyEqualsDualMapping : IDualMapping<bool>
    {
        public static readonly ExactlyEqualsDualMapping Instance = new ExactlyEqualsDualMapping();

        private ExactlyEqualsDualMapping()
        {

        }

        public bool EvaluateArcsins(Expression argument1, Expression argument2)
        {
            return argument1.Map(argument2, this);
        }

        public bool EvaluateArctans(Expression argument1, Expression argument2)
        {
            return argument1.Map(argument2, this);
        }

        public bool EvaluateConstants(IConstant value1, IConstant value2)
        {
            return value1.Equals(value2);
        }

        public bool EvaluateExponents(Expression baseArgument1, Expression powerArgument1, Expression baseArgument2, Expression powerArgument2)
        {
            return baseArgument1.Map(baseArgument2, this) && powerArgument1.Map(powerArgument2, this);
        }

        public bool EvaluateFunctions(Function function1, Function function2)
        {
            if (!function1.GetIdentity().Equals(function2.GetIdentity()))
            {
                return false;
            }

            IDictionary<string, Expression> p1 = function1.GetParameters();
            IDictionary<string, Expression> p2 = function2.GetParameters();

            if (p1.Count != p2.Count)
            {
                return false;
            }

            foreach (string parameterName in p1.Keys)
            {
                if (!p2.TryGetValue(parameterName, out Expression expression2))
                {
                    return false; // The parameter with given name doesn't exist in p2
                }
                Expression expression1 = p1[parameterName];
                if (!expression1.Equals(expression2, EqualityLevel.Exactly))
                {
                    return false;
                }
            }

            return true;
        }

        public bool EvaluateLns(Expression argument1, Expression argument2)
        {
            return argument1.Map(argument2, this);
        }

        public bool EvaluateOthers(Expression expression1, Expression expression2)
        {
            return false;
        }

        protected bool CommutativeExactlyEquals(ICollection<Expression> args1, ICollection<Expression> args2)
        {
            // Check for commutativity
            // Add all parameters to dict by hash
            Dictionary<int, List<Expression>> expressionsByHashes = new Dictionary<int, List<Expression>>();
            foreach (Expression arg2 in args2)
            {
                int hash = arg2.GetHashCode();
                if (!expressionsByHashes.TryGetValue(hash, out List<Expression> expressions))
                {
                    expressions = new List<Expression>();
                    expressionsByHashes.Add(hash, expressions);
                }
                expressions.Add(arg2);
            }

            // Check all parameters in this are present
            foreach (Expression arg1 in args1)
            {
                int hash = arg1.GetHashCode();
                if (!expressionsByHashes.TryGetValue(hash, out List<Expression> expressions))
                {
                    return false;
                }

                // Perform linear search on all equations with same hash
                bool found = false;
                foreach (Expression otherArg in expressions)
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
        public bool EvaluateProducts(ICollection<Expression> arguments1, ICollection<Expression> arguments2)
        {
            return CommutativeExactlyEquals(arguments1, arguments2);
        }

        public bool EvaluateSigns(Expression argument1, Expression argument2)
        {
            return argument1.Map(argument2, this);
        }

        public bool EvaluateSins(Expression argument1, Expression argument2)
        {
            return argument1.Map(argument2, this);
        }

        public bool EvaluateSums(ICollection<Expression> arguments1, ICollection<Expression> arguments2)
        {
            return CommutativeExactlyEquals(arguments1, arguments2);
        }

        public bool EvaluateVariables(IVariable value1, IVariable value2)
        {
            return value1.Equals(value2);
        }
    }
}
