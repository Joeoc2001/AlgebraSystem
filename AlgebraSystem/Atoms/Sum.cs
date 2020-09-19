using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Algebra
{
    namespace Atoms
    {
        internal class Sum : CommutativeOperation
        {
            new public static Expression Add<T>(IEnumerable<T> eqs) where T : Expression
            {
                // Loop and find all other addition nodes and put them into this one
                List<Expression> collatedEqs = new List<Expression>();
                foreach (Expression eq in eqs)
                {
                    if (eq is Sum addeq)
                    {
                        collatedEqs.AddRange(addeq._arguments);
                    }
                    else
                    {
                        collatedEqs.Add(eq);
                    }
                }

                // Put all of the constants together, and other generic commutative operations
                List<Expression> newEqs = SimplifyArguments(collatedEqs, 0, (x, y) => x + y);

                if (newEqs.Count() == 0)
                {
                    return Zero;
                }
                if (newEqs.Count() == 1)
                {
                    return newEqs[0];
                }

                // Collate Multiplication terms
                Dictionary<Expression, RationalConstant> terms = new Dictionary<Expression, RationalConstant>();
                foreach (Expression eq in newEqs)
                {
                    Expression baseEq;
                    RationalConstant newCoefficient;
                    if (eq is Product multeq)
                    {
                        baseEq = multeq.GetVariable();
                        newCoefficient = multeq.GetConstantCoefficient();
                    }
                    else
                    {
                        baseEq = eq;
                        newCoefficient = 1;
                    }

                    if (terms.TryGetValue(baseEq, out RationalConstant coefficient))
                    {
                        newCoefficient = RationalConstant.FromValue(newCoefficient.GetValue() + coefficient.GetValue());
                    }
                    terms[baseEq] = newCoefficient;
                }
                // Put back into exponent form
                newEqs.Clear();
                foreach ((Expression eq, RationalConstant coefficient) in terms)
                {
                    Expression newEq = eq * coefficient;

                    if (newEq.Equals(Zero))
                    {
                        continue;
                    }

                    newEqs.Add(newEq);
                }

                if (newEqs.Count() == 0)
                {
                    return Zero;
                }
                if (newEqs.Count() == 1)
                {
                    return newEqs[0];
                }

                return new Sum(newEqs);
            }

            private Sum(IList<Expression> eqs)
                : base(eqs)
            {

            }

            public override Expression GetDerivative(string wrt)
            {
                List<Expression> derivatives = new List<Expression>();
                foreach (Expression e in _arguments)
                {
                    derivatives.Add(e.GetDerivative(wrt));
                }
                return Add(derivatives);
            }

            public override int IdentityValue()
            {
                return 0;
            }

            public override float Perform(float a, float b)
            {
                return a + b;
            }

            public override string EmptyName()
            {
                return "[EMPTY SUM]";
            }

            public override string OperationSymbol()
            {
                return "+";
            }

            public override int GetOrderIndex()
            {
                return 30;
            }

            public override Func<List<Expression>, Expression> GetSimplifyingConstructor()
            {
                return Add;
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateSum(_arguments);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateSum(this, _arguments);
            }

            public override T Evaluate<T>(Expression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Sum other)
                {
                    return evaluator.EvaluateSums(this._arguments, other._arguments);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}