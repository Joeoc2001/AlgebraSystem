using System;
using System.Collections.Generic;


namespace Algebra
{
    namespace Atoms
    {
        internal class Variable : Expression, IAtomicExpression
        {
            public readonly string Name;

            public Variable(string name)
            {
                this.Name = name.ToLower();
            }

            public override IExpression GetDerivative(string wrt)
            {
                if (wrt == Name)
                {
                    return One;
                }
                return Zero;
            }

            protected override bool ExactlyEquals(IExpression expression)
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

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateVariable(Name);
            }

            protected override IAtomicExpression GenAtomicExpression()
            {
                return this;
            }
        }
    }
}