using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    internal class AtomicExpression : IAtomicExpression
    {
        private readonly IExpression atomicExpression;

        public static IAtomicExpression GetAtomicExpression(IExpression expression)
        {
            if (expression is IAtomicExpression atomic)
            {
                return atomic;
            }
            return new AtomicExpression(expression);
        }

        private AtomicExpression(IExpression expression)
        {
            if (!expression.IsAtomic())
            {
                expression = expression.GetAtomicExpression();
            }
            atomicExpression = expression;
        }

        public bool Equals(IExpression e, EqualityLevel level)
        {
            switch (level)
            {
                // We can shortcut this bit slightly as this doesn't need to be put into atomic form
                case EqualityLevel.Atomic:
                    return atomicExpression.Equals(e.GetAtomicExpression(), EqualityLevel.Exactly);
                default:
                    return atomicExpression.Equals(e, level);
            }
        }

        public T Evaluate<T>(IEvaluator<T> evaluator)
        {
            return atomicExpression.Evaluate(evaluator);
        }

        public T Evaluate<T>(IExpandedEvaluator<T> evaluator)
        {
            return atomicExpression.Evaluate(evaluator);
        }

        public T Evaluate<T>(IExpression secondary, IDualEvaluator<T> evaluator)
        {
            return atomicExpression.Evaluate(secondary, evaluator);
        }

        public IAtomicExpression GetAtomicExpression()
        {
            return this;
        }

        public IExpression GetDerivative(string wrt)
        {
            return atomicExpression.GetDerivative(wrt);
        }

        public IEquivalenceClass GetEquivalenceClass()
        {
            return atomicExpression.GetEquivalenceClass();
        }

        public int GetOrderIndex()
        {
            return atomicExpression.GetOrderIndex();
        }

        public bool IsAtomic()
        {
            return true;
        }

        public IEnumerable<string> GetVariables()
        {
            return atomicExpression.GetVariables();
        }

        public int CompareTo(IExpression other)
        {
            return atomicExpression.CompareTo(other);
        }
    }
}
