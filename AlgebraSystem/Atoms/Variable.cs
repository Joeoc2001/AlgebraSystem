using System;
using System.Collections.Generic;


namespace Algebra
{
    namespace Atoms
    {
        internal class Variable : Expression, IAtomicExpression
        {
            private readonly string name;

            public Variable(string name)
            {
                this.name = name.ToLower();
            }

            public override IExpression GetDerivative(string wrt)
            {
                if (wrt == name)
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

                return name.Equals(variable.name);
            }

            protected override int GenHashCode()
            {
                return name.GetHashCode() * 1513357220;
            }

            public override string ToString()
            {
                return name;
            }

            public override int GetOrderIndex()
            {
                return 0;
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateVariable(name);
            }

            protected override IAtomicExpression GenAtomicExpression()
            {
                return this;
            }

            public override T DualEvaluate<T>(IExpression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Variable other)
                {
                    return evaluator.EvaluateVariables(this.name, other.name);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }
        }
    }
}