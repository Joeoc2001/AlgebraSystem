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
            new public static IExpression Add<T>(IEnumerable<T> eqs) where T : IExpression
            {
                // Loop and find all other addition nodes and put them into this one
                List<IExpression> collatedEqs = new List<IExpression>();
                foreach (IExpression eq in eqs)
                {
                    if (eq is Sum addeq)
                    {
                        collatedEqs.AddRange(addeq.arguments);
                    }
                    else
                    {
                        collatedEqs.Add(eq);
                    }
                }

                // Put all of the constants together, and other generic commutative operations
                List<IExpression> newEqs = SimplifyArguments(collatedEqs, 0, (x, y) => x + y);

                if (newEqs.Count() == 0)
                {
                    return Zero;
                }
                if (newEqs.Count() == 1)
                {
                    return newEqs[0];
                }

                // Collate Multiplication terms
                Dictionary<IExpression, Constant> terms = new Dictionary<IExpression, Constant>();
                foreach (IExpression eq in newEqs)
                {
                    IExpression baseEq;
                    Constant newCoefficient;
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

                    if (terms.TryGetValue(baseEq, out Constant coefficient))
                    {
                        newCoefficient = Constant.FromValue(newCoefficient.GetValue() + coefficient.GetValue());
                    }
                    terms[baseEq] = newCoefficient;
                }
                // Put back into exponent form
                newEqs.Clear();
                foreach (IExpression eq in terms.Keys)
                {
                    IExpression newEq = eq * terms[eq];

                    if (newEq.Equals(Constant.Zero))
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

            private Sum(IList<IExpression> eqs)
                : base(eqs)
            {

            }

            public override IExpression GetDerivative(string wrt)
            {
                List<IExpression> derivatives = new List<IExpression>();
                foreach (IExpression e in arguments)
                {
                    derivatives.Add(e.GetDerivative(wrt));
                }
                return Add(derivatives);
            }

            protected override bool ExactlyEquals(IExpression expression)
            {
                if (!(expression is Sum sum))
                {
                    return false;
                }

                // Check for commutativity
                return OperandsExactlyEquals(sum.arguments);
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

            public override Func<List<IExpression>, IExpression> GetSimplifyingConstructor()
            {
                return Add;
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateSum(arguments);
            }
        }
    }
}