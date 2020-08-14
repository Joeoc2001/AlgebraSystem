using Rationals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Algebra.Atoms
{
    public class Product : CommutativeOperation, IEquatable<Product>
    {
        public static Expression Multiply<T>(List<T> eqs) where T : Expression
        {
            // Collate multiplications into one big multiplication
            List<Expression> collatedEqs = new List<Expression>();
            foreach (Expression eq in eqs)
            {
                if (eq is Product multeq)
                {
                    collatedEqs.AddRange(multeq.Arguments);
                    continue;
                }

                collatedEqs.Add(eq);
            }

            List<Expression> newEqs = SimplifyArguments(collatedEqs, 1, (x, y) => x * y);

            if (newEqs.Count == 0)
            {
                return 1;
            }
            if (newEqs.Count == 1)
            {
                return newEqs[0];
            }

            foreach (Expression eq in newEqs)
            {
                if (eq.Equals(Constant.ZERO))
                {
                    return 0;
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
                    baseEq = expeq.Base;
                    exponentEq = expeq.Power;
                }
                else
                {
                    baseEq = eq;
                    exponentEq = 1;
                }

                if (exponents.TryGetValue(baseEq, out List<Expression> exponentList))
                {
                    exponentList.Add(exponentEq);
                }
                else
                {
                    exponents.Add(baseEq, new List<Expression>() { exponentEq });
                }
            }
            // Put back into exponent form
            newEqs.Clear();
            foreach (Expression eq in exponents.Keys)
            {
                List<Expression> powers = exponents[eq];

                Expression newEq = Pow(eq, Add(powers));

                if (newEq.Equals(Constant.ONE))
                {
                    continue;
                }

                newEqs.Add(newEq);
            }

            if (newEqs.Count == 0)
            {
                return 1;
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

        public override Expression GetDerivative(Variable wrt)
        {
            // Get all derivatives
            List<Expression> derivatives = new List<Expression>(Arguments.Count);
            foreach (Expression eq in Arguments)
            {
                derivatives.Add(eq.GetDerivative(wrt));
            }

            // Collate into multi term product rule
            List<Expression> terms = new List<Expression>();
            for (int iDerivative = 0; iDerivative < Arguments.Count; iDerivative++)
            {
                List<Expression> term = new List<Expression>()
            {
                derivatives[iDerivative]
            };
                for (int iCoefficient = 0; iCoefficient < Arguments.Count; iCoefficient++)
                {
                    if (iCoefficient == iDerivative)
                    {
                        continue;
                    }

                    term.Add(Arguments[iCoefficient]);
                }
                terms.Add(Multiply(term));
            }
            return Sum.Add(terms);
        }

        public bool Equals(Product obj)
        {
            if (obj is null)
            {
                return false;
            }

            // Check for commutativity
            return OperandsEquals(obj.Arguments);
        }

        public override bool Equals(Expression obj)
        {
            return this.Equals(obj as Product);
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
            foreach (Expression eq in Arguments)
            {
                if (eq is Constant c)
                {
                    return c;
                }
            }
            return 1;
        }

        // Gets a multiplication term of all of the terms minus the first constant, or this if there are no constants
        public Expression GetVariable()
        {
            foreach (Expression eq in Arguments)
            {
                if (eq is Constant)
                {
                    List<Expression> others = new List<Expression>(Arguments);
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
    }
}
