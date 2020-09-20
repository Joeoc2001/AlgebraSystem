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
            new public static Expression Multiply<T>(IEnumerable<T> eqs) where T : Expression
            {
                // Collate multiplications into one big multiplication
                List<Expression> collatedEqs = new List<Expression>();
                foreach (Expression eq in eqs)
                {
                    if (eq is Product multeq)
                    {
                        collatedEqs.AddRange(multeq._arguments);
                        continue;
                    }

                    collatedEqs.Add(eq);
                }

                List<Expression> newEqs = SimplifyArguments(collatedEqs, 1, (x, y) => x * y);

                if (newEqs.Count == 0)
                {
                    return One;
                }
                if (newEqs.Count == 1)
                {
                    return newEqs[0];
                }

                foreach (Expression eq in newEqs)
                {
                    if (eq.Equals(Zero))
                    {
                        return Zero;
                    }
                }

                // Collate exponents
                Dictionary<Expression, List<Expression>> exponents = new Dictionary<Expression, List<Expression>>();
                foreach (Expression eq in newEqs)
                {
                    Expression baseEq;
                    Expression exponentEq;
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

                    if (!exponents.TryGetValue(baseEq, out List<Expression> exponentList))
                    {
                        exponentList = new List<Expression>();
                        exponents.Add(baseEq, exponentList);
                    }
                    exponentList.Add(exponentEq);
                }
                // Put back into exponent form
                newEqs.Clear();
                foreach ((Expression eq, List<Expression> powers) in exponents)
                {
                    Expression newEq = Pow(eq, Add(powers));

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

            private Product(IList<Expression> eqs)
                : base(eqs)
            {

            }

            public override Expression GetDerivative(string wrt)
            {
                // Get all derivatives
                List<Expression> derivatives = new List<Expression>(_arguments.Count);
                foreach (Expression eq in _arguments)
                {
                    derivatives.Add(eq.GetDerivative(wrt));
                }

                // Collate into multi term product rule
                List<Expression> terms = new List<Expression>();
                for (int iDerivative = 0; iDerivative < _arguments.Count; iDerivative++)
                {
                    List<Expression> term = new List<Expression>()
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

            public override string EmptyName()
            {
                return "1";
            }

            public override string OperationSymbol()
            {
                return "*";
            }

            // Finds the first constant in the multiplication, or returns 1 if there are none
            public RationalConstant GetConstantCoefficient()
            {
                foreach (Expression eq in _arguments)
                {
                    if (eq is RationalConstant c)
                    {
                        return c;
                    }
                }
                return 1;
            }

            // Gets a multiplication term of all of the terms minus the first constant, or this if there are no constants
            public Expression GetVariable()
            {
                foreach (Expression eq in _arguments)
                {
                    if (eq is RationalConstant)
                    {
                        List<Expression> others = new List<Expression>(_arguments);
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

            public override Func<List<Expression>, Expression> GetSimplifyingConstructor()
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

            public override T Evaluate<T>(Expression otherExpression, IDualEvaluator<T> evaluator)
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