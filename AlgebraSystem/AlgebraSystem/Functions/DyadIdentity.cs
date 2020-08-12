using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Functions
{
    public class DyadIdentity : FunctionIdentity
    {
        public readonly Variable Parameter1;
        public readonly Variable Parameter2;

        public delegate Expression.ExpressionDelegate GetDyadDelegateDelegate(Expression.ExpressionDelegate delegate1, Expression.ExpressionDelegate delegate2);
        public delegate Expression GetDyadDerivativeDelegate(Expression parameter1, Expression parameter2, Variable wrt);

        public DyadIdentity(Variable parameter1, Variable parameter2, int hashSeed, Expression atomicExpression, GetDyadDelegateDelegate getDelegate, GetDyadDerivativeDelegate getDerivative)
            : base(new List<string>() { parameter1.Name, parameter2.Name }, hashSeed, atomicExpression,
                  dels => getDelegate(dels[parameter1.Name], dels[parameter2.Name]),
                  (exprs, wrt) => getDerivative(exprs[parameter1.Name], exprs[parameter2.Name], wrt))
        {
            Parameter1 = parameter1;
            Parameter2 = parameter2;
        }

        public Expression CreateExpression(Expression p1, Expression p2)
        {
            return CreateExpression(new Dictionary<string, Expression>()
            {
                { Parameter1.Name, p1 },
                { Parameter2.Name, p2 },
            });
        }
    }
}
