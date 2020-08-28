using System;
using System.Collections.Generic;


namespace Algebra
{
    namespace Atoms
    {
        internal class Variable : Expression, IAtomicExpression
        {
            private readonly string _name;

            public Variable(string name)
            {
                this._name = name.ToLower();
            }

            public override IExpression GetDerivative(string wrt)
            {
                if (wrt == _name)
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

                return _name.Equals(variable._name);
            }

            protected override int GenHashCode()
            {
                return _name.GetHashCode() * 1513357220;
            }

            public override string ToString()
            {
                return _name;
            }

            public override int GetOrderIndex()
            {
                return 0;
            }

            public override T Evaluate<T>(IEvaluator<T> evaluator)
            {
                return evaluator.EvaluateVariable(_name);
            }

            public override T Evaluate<T>(IExpandedEvaluator<T> evaluator)
            {
                return evaluator.EvaluateVariable(this, _name);
            }

            public override T Evaluate<T>(IExpression otherExpression, IDualEvaluator<T> evaluator)
            {
                if (otherExpression is Variable other)
                {
                    return evaluator.EvaluateVariables(this._name, other._name);
                }
                return evaluator.EvaluateOthers(this, otherExpression);
            }

            protected override IAtomicExpression GenAtomicExpression()
            {
                return this;
            }
        }
    }
}