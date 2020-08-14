﻿using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Algebra.Atoms
{
    public class Sum : CommutativeOperation
    {
        public static Expression Add<T>(List<T> eqs) where T : Expression
        {
            // Loop and find all other addition nodes and put them into this one
            List<Expression> collatedEqs = new List<Expression>();
            foreach (Expression eq in eqs)
            {
                if (eq is Sum addeq)
                {
                    collatedEqs.AddRange(addeq.Arguments);
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
                return 0;
            }
            if (newEqs.Count() == 1)
            {
                return newEqs[0];
            }

            // Collate Multiplication terms
            Dictionary<Expression, Constant> terms = new Dictionary<Expression, Constant>();
            foreach (Expression eq in newEqs)
            {
                Expression baseEq;
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
                    newCoefficient = Constant.From(newCoefficient.GetValue() + coefficient.GetValue());
                }
                terms[baseEq] = newCoefficient;
            }
            // Put back into exponent form
            newEqs.Clear();
            foreach (Expression eq in terms.Keys)
            {
                Expression newEq = eq * terms[eq];

                if (newEq.Equals(Constant.ZERO))
                {
                    continue;
                }

                newEqs.Add(newEq);
            }

            if (newEqs.Count() == 0)
            {
                return 0;
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

        public override Expression GetDerivative(Variable wrt)
        {
            List<Expression> derivatives = new List<Expression>();
            foreach (Expression e in Arguments)
            {
                derivatives.Add(e.GetDerivative(wrt));
            }
            return Add(derivatives);
        }

        protected override bool ExactlyEquals(Expression expression)
        {
            if (!(expression is Sum sum))
            {
                return false;
            }

            // Check for commutativity
            return OperandsExactlyEquals(sum.Arguments);
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
    }
}