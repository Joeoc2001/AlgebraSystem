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
            protected readonly ReadOnlyCollection<IExpression> _arguments;

            public CommutativeOperation(IList<IExpression> eqs)
            {
                this._arguments = new ReadOnlyCollection<IExpression>(eqs);
            }

            public abstract int IdentityValue();
            public abstract float Perform(float a, float b);
            public delegate Rational Operation(Rational a, Rational b);
            public abstract string EmptyName();
            public abstract string OperationSymbol();
            public abstract Func<List<IExpression>, IExpression> GetSimplifyingConstructor();

            protected override int GenHashCode()
            {
                int value = -1906136416 * OperationSymbol().GetHashCode();
                foreach (IExpression eq in _arguments)
                {
                    value ^= eq.GetHashCode(); // This is bad practice but it will have to do
                }
                return value;
            }

            protected static List<IExpression> SimplifyArguments<T>(List<T> eqs, Rational identity, Operation operation) where T : IExpression
            {
                List<IExpression> newEqs = new List<IExpression>(eqs.Count);

                Rational collectedConstants = identity;

                // Loop & simplify
                foreach (IExpression eq in eqs)
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

                builder.Append(ToParenthesisedString(this, _arguments[0]));
                for (int i = 1; i < _arguments.Count; i++)
                {
                    builder.Append(" ");
                    builder.Append(OperationSymbol());
                    builder.Append(" ");
                    builder.Append(ToParenthesisedString(this, _arguments[i]));
                }

                return builder.ToString();
            }

            protected override IExpression GenAtomicExpression()
            {
                // Replace variables with their expressions
                List<IExpression> atomicArguments = new List<IExpression>();
                foreach (var argument in _arguments)
                {
                    atomicArguments.Add(argument.GetAtomicExpression());
                }
                return GetSimplifyingConstructor()(atomicArguments);
            }
        }
    }
}