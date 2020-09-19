using Algebra.Evaluators;
using Rationals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;

namespace Algebra
{
    namespace Atoms
    {
        internal abstract class CommutativeOperation : Expression
        {
            protected readonly ReadOnlyCollection<Expression> _arguments;

            public CommutativeOperation(IList<Expression> eqs)
            {
                this._arguments = new ReadOnlyCollection<Expression>(eqs);
            }

            public abstract int IdentityValue();
            public abstract float Perform(float a, float b);
            public delegate Rational Operation(Rational a, Rational b);
            public abstract string EmptyName();
            public abstract string OperationSymbol();
            public abstract Func<List<Expression>, Expression> GetSimplifyingConstructor();

            protected List<Expression> GetDisplaySortedArguments()
            {
                List<Expression> sortedArguments = new List<Expression>(_arguments);
                sortedArguments.Sort((a, b) => a.Evaluate(b, OrderingDualEvaluator.Instance));
                return sortedArguments;
            }

            protected override int GenHashCode()
            {
                int value = -1906136416 ^ OperationSymbol().GetHashCode();
                foreach (Expression eq in GetDisplaySortedArguments())
                {
                    value += eq.GetHashCode();
                    value *= 33;
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
                    if (eq is RationalConstant constEq)
                    {
                        collectedConstants = operation(collectedConstants, constEq.GetValue());
                        continue;
                    }

                    newEqs.Add(eq);
                }

                if (!collectedConstants.Equals(identity))
                {
                    newEqs.Add(ConstantFrom(collectedConstants));
                }

                return newEqs;
            }

            public override string ToString()
            {
                if (_arguments.Count == 0)
                {
                    return "()";
                }

                StringBuilder builder = new StringBuilder();

                List<Expression> displaySortedArguments = GetDisplaySortedArguments();

                builder.Append(ToParenthesisedString(this, displaySortedArguments[0]));
                for (int i = 1; i < displaySortedArguments.Count; i++)
                {
                    builder.Append(" ");
                    builder.Append(OperationSymbol());
                    builder.Append(" ");
                    builder.Append(ToParenthesisedString(this, displaySortedArguments[i]));
                }

                return builder.ToString();
            }

            protected override Expression GenAtomicExpression()
            {
                // Replace variables with their expressions
                List<Expression> atomicArguments = new List<Expression>();
                foreach (var argument in _arguments)
                {
                    atomicArguments.Add(argument.GetAtomicExpression());
                }
                return GetSimplifyingConstructor()(atomicArguments);
            }
        }
    }
}