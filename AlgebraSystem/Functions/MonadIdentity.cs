using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Functions
{
    public class MonadIdentity : FunctionIdentity
    {
        public readonly Variable Parameter;

        public delegate Expression.ExpressionDelegate GetMonadDelegateDelegate(Expression.ExpressionDelegate delegate1);
        public delegate Expression GetMonadDerivativeDelegate(Expression parameter1, Variable wrt);

        public MonadIdentity(string name, Variable parameter, int hashSeed, Expression atomicExpression, GetMonadDelegateDelegate getDelegate, GetMonadDerivativeDelegate getDerivative)
            : base(name, new List<string>() { parameter.Name }, hashSeed, atomicExpression,
                  dels => getDelegate(dels[parameter.Name]),
                  (exprs, wrt) => getDerivative(exprs[parameter.Name], wrt))
        {
            Parameter = parameter;
        }

        public Expression CreateExpression(Expression expression)
        {
            return CreateExpression(new Dictionary<string, Expression>()
            {
                { Parameter.Name, expression },
            });
        }
    }
}
