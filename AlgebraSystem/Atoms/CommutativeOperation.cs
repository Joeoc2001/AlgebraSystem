﻿using Rationals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;

namespace Algebra.Atoms
{
    public abstract class CommutativeOperation : Expression
    {
        public readonly ReadOnlyCollection<Expression> Arguments;

        public CommutativeOperation(IList<Expression> eqs)
        {
            this.Arguments = new ReadOnlyCollection<Expression>(eqs);
        }

        public abstract int IdentityValue();
        public abstract float Perform(float a, float b);
        public delegate Rational Operation(Rational a, Rational b);
        public abstract string EmptyName();
        public abstract string OperationSymbol();
        public abstract Func<List<Expression>, Expression> GetSimplifyingConstructor();

        public override sealed ExpressionDelegate GetDelegate(VariableInputSet set)
        {
            List<ExpressionDelegate> expressions = new List<ExpressionDelegate>(Arguments.Count());
            foreach (Expression e in Arguments)
            {
                expressions.Add(e.GetDelegate(set));
            }

            float identityValue = IdentityValue();

            return () =>
            {
                float value = identityValue;
                foreach (ExpressionDelegate f in expressions)
                {
                    value = Perform(value, f());
                }
                return value;
            };
        }

        protected bool OperandsExactlyEquals(IList<Expression> otherArgs)
        {
            // Check for commutativity
            // Add all parameters to dict by hash
            Dictionary<int, List<Expression>> expressionsByHashes = new Dictionary<int, List<Expression>>();
            foreach (Expression otherArg in otherArgs)
            {
                int hash = otherArg.GetHashCode();
                if (!expressionsByHashes.TryGetValue(hash, out List<Expression> expressions))
                {
                    expressions = new List<Expression>();
                    expressionsByHashes.Add(hash, expressions);
                }
                expressions.Add(otherArg);
            }

            // Check all parameters in this are present
            IList<Expression> thisArgs = Arguments;
            foreach (Expression thisArg in thisArgs)
            {
                int hash = thisArg.GetHashCode();
                if (!expressionsByHashes.TryGetValue(hash, out List<Expression> expressions))
                {
                    return false;
                }

                // Perform linear search on all equations with same hash
                bool found = false;
                foreach (Expression otherArg in expressions)
                {
                    if (otherArg.Equals(thisArg, EqalityLevel.Exactly))
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

        public List<Expression> GetDisplaySortedArguments()
        {
            return GetDisplaySortedArguments(Arguments);
        }

        private static List<Expression> GetDisplaySortedArguments(IList<Expression> eqs)
        {
            List<Expression> sortedEqs = new List<Expression>(eqs);
            sortedEqs.Sort(ExpressionDisplayComparer.COMPARER);
            return sortedEqs;
        }

        protected override int GenHashCode()
        {
            int value = -1906136416 * OperationSymbol().GetHashCode();
            foreach (Expression eq in GetDisplaySortedArguments())
            {
                value *= 33;
                value ^= eq.GetHashCode();
            }
            return value;
        }

        protected static List<Expression> SimplifyArguments<T>(List<T> eqs, Rational identity, Operation operation) where T : Expression
        {
            List<Expression> newEqs = new List<Expression>(eqs.Count);

            Rational collectedConstants = identity;

            // Loop & simplify
            foreach (Expression eq in eqs)
            {
                if (eq is Constant constEq)
                {
                    collectedConstants = operation(collectedConstants, constEq.GetValue());
                    continue;
                }

                newEqs.Add(eq);
            }

            if (!collectedConstants.Equals(identity))
            {
                newEqs.Add(Constant.From(collectedConstants));
            }

            return newEqs;
        }

        public override string ToString()
        {
            if (Arguments.Count == 0)
            {
                return "()";
            }

            StringBuilder builder = new StringBuilder();

            builder.Append(ToParenthesisedString(Arguments[0]));
            for (int i = 1; i < Arguments.Count; i++)
            {
                builder.Append(" ");
                builder.Append(OperationSymbol());
                builder.Append(" ");
                builder.Append(ToParenthesisedString(Arguments[i]));
            }

            return builder.ToString();
        }

        public override Expression Map(ExpressionMapping map)
        {
            Expression currentThis = this;

            if (map.ShouldMapChildren(this))
            {
                List<Expression> mappedEqs = new List<Expression>(Arguments.Count);

                foreach (Expression eq in Arguments)
                {
                    mappedEqs.Add(eq.Map(map));
                }

                currentThis = GetSimplifyingConstructor()(mappedEqs);
            }

            if (map.ShouldMapThis(this))
            {
                currentThis = map.PostMap(currentThis);
            }

            return currentThis;
        }
    }
}