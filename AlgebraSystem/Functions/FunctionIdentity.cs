using Algebra.Atoms;
using Algebra.Functions.HardcodedFunctionIdentities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Functions
{
    public class FunctionIdentity : FunctionGenerator
    {
        private readonly int hashSeed;

        public readonly Expression AtomicExpression;

        public delegate Expression.ExpressionDelegate GetDelegateDelegate(Dictionary<string, Expression.ExpressionDelegate> parameterDelegates);
        public delegate Expression GetDerivativeDelegate(Dictionary<string, Expression> parameterExpressions, Variable wrt);

        private readonly GetDelegateDelegate getDelegate;
        private readonly GetDerivativeDelegate getDerivative;

        public FunctionIdentity(string name, List<string> parameterNames, int hashSeed, Expression alternateExpression, GetDelegateDelegate getDelegate, GetDerivativeDelegate getDerivative)
            : base(name, parameterNames)
        {
            this.hashSeed = hashSeed;

            this.AtomicExpression = alternateExpression.GetAtomicExpression();

            this.getDelegate = getDelegate;
            this.getDerivative = getDerivative;
        }

        protected override Expression CreateExpressionImpl(Dictionary<string, Expression> nodes)
        {
            return new Function(this, nodes);
        }

        public int GetHashSeed()
        {
            return hashSeed;
        }

        public Expression.ExpressionDelegate GetDelegate(Dictionary<string, Expression.ExpressionDelegate> parameterDelegates)
        {
            return getDelegate(parameterDelegates);
        }

        public Expression GetDerivative(Function function, Variable wrt)
        {
            if (!function.GetIdentity().Equals(this))
            {
                throw new ArgumentException("Function is not of this identity");
            }

            Dictionary<string, Expression> parameterExpressions = function.GetParameters();
            return getDerivative(parameterExpressions, wrt);
        }
    }
}
