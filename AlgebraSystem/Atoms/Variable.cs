using System;
using System.Collections.Generic;


namespace Algebra
{
    namespace Atoms
    {
        internal class Variable : Expression, IVariable
        {
            private readonly string _name;

            public Variable(string name)
            {
                this._name = name ?? throw new ArgumentNullException(nameof(name));
            }

            public override Expression GetDerivative(string wrt)
            {
                if (wrt == _name)
                {
                    return One;
                }
                return Zero;
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

            public override void Map(IMapping mapping)
            {
                mapping.EvaluateVariable(this);
            }

            public override T Map<T>(IMapping<T> mapping)
            {
                return mapping.EvaluateVariable(this);
            }

            public override T Map<T>(IExtendedMapping<T> mapping)
            {
                return mapping.EvaluateVariable(this, this);
            }

            public override T Map<T>(Expression otherExpression, IDualMapping<T> mapping)
            {
                if (otherExpression is Variable other)
                {
                    return mapping.EvaluateVariables(this, other);
                }
                return mapping.EvaluateOthers(this, otherExpression);
            }

            protected override Expression GenAtomicExpression()
            {
                return this;
            }

            public string GetName()
            {
                return _name;
            }

            public Expression ToExpression()
            {
                return this;
            }

            public bool Equals(IVariable other)
            {
                return GetName().Equals(other.GetName());
            }
        }
    }
}