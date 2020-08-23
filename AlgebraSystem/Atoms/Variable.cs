using System;
using System.Collections.Generic;


namespace Algebra
{
    namespace Atoms
    {
        internal class Variable : Expression
        {
            public class NotPresentException : ArgumentException
            {
                public NotPresentException(string message) : base(message)
                {
                }
            }

            public readonly string Name;

            public Variable(string name)
            {
                this.Name = name.ToLower();
            }

            public override Expression GetDerivative(string wrt)
            {
                if (wrt == Name)
                {
                    return 1;
                }
                return 0;
            }

            protected override bool ExactlyEquals(Expression expression)
            {
                if (!(expression is Variable variable))
                {
                    return false;
                }

                return Name.Equals(variable.Name);
            }

            protected override int GenHashCode()
            {
                return Name.GetHashCode() * 1513357220;
            }

            public override string ToString()
            {
                return Name;
            }

            public override int GetOrderIndex()
            {
                return 0;
            }

            public override Expression MapChildren(ExpressionMapping.ExpressionMap map)
            {
                return this;
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateVariable(Name);
            }
        }
    }
}