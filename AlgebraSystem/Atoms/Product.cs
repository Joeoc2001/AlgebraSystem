using Rationals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Algebra
{
    namespace Atoms
    {
        internal class Product : CommutativeOperation
        {
            new public static IExpression Multiply<T>(IEnumerable<T> eqs) where T : IExpression
            {
                // Collate multiplications into one big multiplication
                List<IExpression> collatedEqs = new List<IExpression>();
                foreach (IExpression eq in eqs)
                {
                    if (eq is Product multeq)
                    {
                        collatedEqs.AddRange(multeq._arguments);
                        continue;
                    }

                    collatedEqs.Add(eq);
                }

                List<IExpression> newEqs = SimplifyArguments(collatedEqs, 1, (x, y) => x * y);

                if (newEqs.Count == 0)
                {
                    return One;
                }
                if (newEqs.Count == 1)
                {
                    return newEqs[0];
                }

                foreach (IExpression eq in newEqs)
                {
                    if (eq.Equals(Zero))
                    {
                        return Zero;
                    }
                }

                // Collate exponents
                Dictionary<IExpression, List<IExpression>> exponents = new Dictionary<IExpression, List<IExpression>>();
                foreach (IExpression eq in newEqs)
                {
                    IExpression baseEq;
                    IExpression exponentEq;
                    if (eq is Exponent expeq)
                    {
                        baseEq = expeq.GetTerm();
                        exponentEq = expeq.GetPower();
                    }
                    else
                    {
                        baseEq = eq;
                        exponentEq = One;
                    }

                    if (!exponents.TryGetValue(baseEq, out List<IExpression> exponentList))
                    {
                        exponentList = new List<IExpression>();
                        exponents.Add(baseEq, exponentList);
                    }
                    exponentList.Add(exponentEq);
                }
                // Put back into exponent form
                newEqs.Clear();
                foreach ((IExpression eq, List<IExpression> powers) in exponents)
                {
                    IExpression newEq = Pow(eq, Add(powers));

                    if (newEq.Equals(One))
                    {
                        continue;
                    }

                    newEqs.Add(newEq);
                }

                if (newEqs.Count == 0)
                {
                    return One;
                }
                if (newEqs.Count == 1)
                {
                    return newEqs[0];
                }

                return new Product(newEqs);
            }

            private Product(IList<IExpression> eqs)
                : base(eqs)
            {

            }

            public override IExpression GetDerivative(string wrt)
            {
                // Get all derivatives
                List<IExpression> derivatives = new List<IExpression>(_arguments.Count);
                foreach (IExpression eq in _arguments)
                {
                    derivatives.Add(eq.GetDerivative(wrt));
                }

                // Collate into multi term product rule
                List<IExpression> terms = new List<IExpression>();
                for (int iDerivative = 0; iDerivative < _arguments.Count; iDerivative++)
                {
                    List<IExpression> term = new List<IExpression>()
                    {
                        derivatives[iDerivative]
                    };
                    for (int iCoefficient = 0; iCoefficient < _arguments.Count; iCoefficient++)
                    {
                        if (iCoefficient == iDerivative)
                        {
                            continue;
                        }

                        term.Add(_arguments[iCoefficient]);
                    }
                    terms.Add(Multiply(term));
                }
                return Sum.Add(terms);
            }

            public override int IdentityValue()
            {
                return 1;
            }

            public override float Perform(float a, float b)
            {
                return a * b;
            }

            public override string EmptyName()
            {
                return "[EMPTY PRODUCT]";
            }

            public override string OperationSymbol()
            {
                return "*";
            }

            // Finds the first constant in the multiplication, or returns 1 if there are none
            public Constant GetConstantCoefficient()
            {
                foreach (IExpression eq in _arguments)
                {
                    if (eq is Constant c)
                    {
                        return c;
                    }
                }
                return 1;
            }

            // Gets a multiplication term of all of the terms minus the first constant, or this if there are no constants
            public IExpression GetVariable()
            {
                foreach (IExpression eq in _arguments)
                {
                    if (eq is Constant)
                    {
                        List<IExpression> others = new List<IExpression>(_arguments);
                        others.Remove(eq);
                        if (others.Count == 1)
                        {
                            return others[0];
                        }
                        return new Product(others);
                    }
                }
                return this;
            }

            public override int GetOrderIndex()
            {
                return 20;
            }

            public override Func<List<IExpression>, IExpression> GetSimplifyingConstructor()
            {
                return Multiply;
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateProduct(_arguments);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateProduct(this, _arguments);
            }

            public override T Evaluate<T>(IExpression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Product other)
                {
                    return evaluator.EvaluateProducts(this._arguments, other._arguments);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}